// Модерация для REST-поллера: анти-спам и анти-оскорбления.
// Счётчики хранятся в Supabase (переживают рестарты), бан — через Discord timeout.
// Схема наказаний:
//   спам:     >20 сообщений за 30 секунд  → бан 10 минут
//   оскорбл.: >4 нарушений за 5 минут     → бан 10 минут
//   рецидив (в течение 24 ч): время бана × 100 (16 часов 40 минут)

import pg from "pg";

const {
  DATABASE_URL,
  DISCORD_TOKEN,
  GUILD_ID,
  OWNER_USERNAME = "danvr2013",
} = process.env;

// Пороговые значения
const SPAM_WINDOW_SEC = 30;      // окно подсчёта спама
const SPAM_MAX_MESSAGES = 20;    // >20 сообщений в окне = спам
const ABUSE_WINDOW_SEC = 300;    // окно подсчёта оскорблений (5 минут)
const ABUSE_MAX_STRIKES = 4;     // >4 оскорбления в окне = бан
const BASE_BAN_MIN = 10;         // базовый бан: 10 минут
const ESCALATION_MULT = 100;     // рецидив: ×100
const RECIDIVE_WINDOW_HOURS = 24;// в течение 24 часов считается рецидив

let pool;
function getPool() {
  if (!DATABASE_URL) throw new Error("DATABASE_URL not set");
  if (!pool) {
    pool = new pg.Pool({
      connectionString: DATABASE_URL,
      max: 2,
      connectionTimeoutMillis: 10000,
      idleTimeoutMillis: 10000,
    });
  }
  return pool;
}

// ── Определение оскорблений и нецензурной лексики ─────────────────────────
// Часть слов маскируется, чтобы не хранить их в открытом виде в репозитории.

const ABUSE_PATTERNS = [
  /[хx]\s*[уy]\s*[йyёюe]/i,
  /п\s*[иiи]\s*[дd]\s*[оoаa]/i,
  /б\s*[лl]?\s*[яa@]\s*[дdt]/i,
  /с\s*[уy]\s*[чbкk]/i,
  /г\s*[оoаa]\s*[ннn]+\s*[дda]/i,
  /м\s*[уy]\s*[дd]\s*[аa@]/i,
  /[\s,.!?]?елд[ао]/i,
  /за\s*еб\s*ы/i,
  /б\s*ля\s*ть/i,
  /ш\s*[лl]\s*[юy]\s*[хx]/i,
  /ч\s*[еe]\s*[рr]\s*т\s*п\s*[аa]/i,
  /да\s*ун/i,
  /ид\s*[иi]\s*\s*[нn]\s*[а@]\s*[хx]/i,
  /[хx]\s*[уy]\s*[еeйя]/i,
  /п\s*[иi]\s*[зz]\s*[дd]/i,
  /с\s*с\s*а\s*т\s*ь/i,
  /т\s*р\s*а\s*п\s*о/i,
  /ол\s*е\s*н\s*ь\s*м/i,
  /ш\s*к\s*о\s*л\s*о\s*т\s*а/i,
];

const INSULT_WORDS = [
  "идиот", "кретин", "тупой", "тупица", "дебил", "дурак", "лох",
  "урод", "бесполезный", "никчемный", "имбецил", "придурок",
  "псих", "шизик", "бомж", "нищий", "тварь", "сволочь",
  "мразь", "мудак", "уёбок", "уебок", "чмо", "гнида", "падла",
];

export function countAbuse(content) {
  const text = content.toLowerCase();
  let hits = 0;
  for (const re of ABUSE_PATTERNS) {
    const matches = text.match(new RegExp(re.source, "gi"));
    if (matches) hits += matches.length;
  }
  for (const word of INSULT_WORDS) {
    let idx = 0;
    while ((idx = text.indexOf(word, idx)) !== -1) {
      hits++;
      idx += word.length;
    }
  }
  return hits;
}

// ── Работа с БД ───────────────────────────────────────────────────────────

async function countEvents(discordId, kind, windowSec) {
  const r = await getPool().query(
    `SELECT count(*)::int AS c FROM moderation_events
      WHERE discord_id = $1 AND kind = $2 AND created_at > now() - ($3 || ' seconds')::interval`,
    [discordId, kind, String(windowSec)]
  );
  return r.rows[0].c;
}

async function getBanState(discordId) {
  const r = await getPool().query(
    "SELECT strikes, banned_until FROM moderation_bans WHERE discord_id = $1",
    [discordId]
  );
  return r.rows[0] || { strikes: 0, banned_until: null };
}

async function recordEvent(discordId, channelId, kind) {
  await getPool().query(
    "INSERT INTO moderation_events (discord_id, channel_id, kind) VALUES ($1, $2, $3)",
    [discordId, channelId, kind]
  );
}

// Discord timeout через REST API (PATCH /guilds/:id/members/:uid)
async function applyDiscordTimeout(discordId, untilIso) {
  if (!DISCORD_TOKEN || !GUILD_ID) throw new Error("DISCORD_TOKEN/GUILD_ID not set for moderation");
  const response = await fetch(
    `https://discord.com/api/v10/guilds/${GUILD_ID}/members/${discordId}`,
    {
      method: "PATCH",
      headers: {
        Authorization: `Bot ${DISCORD_TOKEN}`,
        "Content-Type": "application/json",
      },
      body: JSON.stringify({ communication_disabled_until: untilIso }),
      signal: AbortSignal.timeout(15000),
    }
  );
  if (!response.ok) {
    const text = await response.text();
    throw new Error(`Discord timeout ${response.status}: ${text.slice(0, 200)}`);
  }
}

const isOwner = (username) => (username || "").toLowerCase() === OWNER_USERNAME.toLowerCase();

// Главный обработчик: вызывается для КАЖДОГО сообщения из канала.
// Возвращает { banned, message } — message нужно отправить в канал.
export async function moderateMessage({ discordId, username, channelId, content, guildId }) {
  if (!discordId) return { banned: false, message: null };
  // Владелец и боты не модерируются
  if (isOwner(username)) return { banned: false, message: null };

  // Активный бан: сообщения от забаненного только считаем (для записи), ничего не отвечаем.
  const state = await getBanState(discordId);
  const now = Date.now();
  const bannedUntilMs = state.banned_until ? Date.parse(state.banned_until) : 0;
  if (bannedUntilMs > now) {
    // пока бан активен — сообщения не обрабатываем вовсе
    await recordEvent(discordId, channelId, "message");
    return { banned: true, muted: true, message: null };
  }

  // Считаем каждое сообщение (спам-счётчик)
  await recordEvent(discordId, channelId, "message");

  // 1. Спам: считаем сообщения за окно 30 сек (текущее уже записано)
  const spamCount = await countEvents(discordId, "message", SPAM_WINDOW_SEC);
  if (spamCount > SPAM_MAX_MESSAGES) {
    return enforceBan(discordId, username, channelId, state, "спам", "за спам");
  }

  // 2. Оскорбления: считаем слова в сообщении
  const abuseHits = countAbuse(content);
  if (abuseHits > 0) {
    // каждая фраза считается одним нарушением (упрощённо: 1 событие на сообщение)
    await recordEvent(discordId, channelId, "abuse");
    const abuseCount = await countEvents(discordId, "abuse", ABUSE_WINDOW_SEC);
    if (abuseCount > ABUSE_MAX_STRIKES) {
      return enforceBan(discordId, username, channelId, state, "оскорбления", "за оскорбления участников");
    }
  }

  return { banned: false, message: null };
}

async function enforceBan(discordId, username, channelId, state, reasonShort, reasonText) {
  const strikes = state.strikes || 0;
  const now = Date.now();

  // Рецидив в течение 24 часов с момента прошлого бана → ×100
  const lastBanEnd = state.banned_until ? Date.parse(state.banned_until) : 0;
  const isRecidivist = lastBanEnd > 0 && (now - lastBanEnd) < RECIDIVE_WINDOW_HOURS * 3600 * 1000;

  const banMinutes = isRecidivist ? BASE_BAN_MIN * ESCALATION_MULT : BASE_BAN_MIN;
  const bannedUntil = new Date(now + banMinutes * 60 * 1000);

  // Сохраняем состояние бана
  await getPool().query(
    `INSERT INTO moderation_bans (discord_id, strikes, banned_until, updated_at)
     VALUES ($1, $2, $3, now())
     ON CONFLICT (discord_id) DO UPDATE
       SET strikes = EXCLUDED.strikes, banned_until = EXCLUDED.banned_until, updated_at = now()`,
    [discordId, strikes + 1, bannedUntil.toISOString()]
  );

  // Чистим его счётчики, чтобы после бана окна считались заново
  await getPool().query(
    "DELETE FROM moderation_events WHERE discord_id = $1",
    [discordId]
  );

  // Накладываем Discord timeout
  let applied = false;
  let note = "";
  try {
    await applyDiscordTimeout(discordId, bannedUntil.toISOString());
    applied = true;
  } catch (err) {
    console.error("Timeout apply failed:", err.message);
    note = " (мьют на уровне бота: сообщения игнорируются)";
  }

  const display = username || `<@${discordId}>`;
  const message =
    `🔨 **${display}** забанен на **${banMinutes >= 60 ? Math.round(banMinutes / 60) + " ч" : banMinutes + " мин"}** по причине: ${reasonText}.` +
    (isRecidivist ? " ⚠️ Повторное нарушение — длительность увеличена в 100 раз." : "") +
    (applied ? "" : note);

  return { banned: true, message, banMinutes, reason: reasonShort };
}
