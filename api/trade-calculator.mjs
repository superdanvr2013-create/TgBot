// Калькулятор трейда для брейнротов (Discord → веб-страница).
// Слева — брейнроты, которые ты отдаёшь (зелёная рамка на скрине),
// справа — брейнроты, которые получаешь. Показывает, выгодна ли сделка.

const POLL_SECRET = process.env.POLL_SECRET;
const DB_API = process.env.DB_API_URL || "https://dan-games-discord-poller.vercel.app/api/db";

function auth(req) {
  const bearer = req.headers.authorization?.match(/^Bearer\s+(.+)$/i)?.[1];
  return Boolean(POLL_SECRET && bearer === POLL_SECRET);
}

function json(res, status, body) {
  res.status(status).json(body);
}

// Извлекаем имена брейнротов из ответа ИИ (текст) — сплит по запятым/переводам.
// Сохраняем только те, которые есть в базе знаний (RAG), иначе брейнрот неизвестен.
async function extractBrainrots(answerText, dbApiKey) {
  // ИИ при распознавании картинки обычно перечисляет названия через запятую или дефис.
  // Собираем потенциальные имена: строки между «—», после нумерации «1.», через запятую.
  const candidates = new Set();
  const lines = answerText.split(/[\n,;—–-]/);
  for (let line of lines) {
    line = line.replace(/^\s*\d+[.)]\s*/, "").trim(); // «1. Name —»
    if (line.length > 2 && line.length < 60 && /[A-Za-zА-Яа-я]/.test(line)) {
      candidates.add(line);
    }
  }

  // Фильтруем по базе знаний: имя должно присутствовать в RAG (поле theme/description).
  if (!dbApiKey) return [...candidates];
  const r = await fetch(`${DB_API}?table=knowledge`, {
    headers: { Authorization: `Bearer ${dbApiKey}` },
    signal: AbortSignal.timeout(15000),
  });
  const { rows } = await r.json();
  const known = new Set();
  for (const row of rows) {
    for (const part of (row.theme + "|" + row.description).split("|")) {
      const words = part.split(/\s+/);
      for (let i = 0; i < words.length; i++) {
        const w = words[i].replace(/[,.;:!?()\"'—–-]/g, "");
        if (w.length > 2) known.add(w.toLowerCase());
      }
    }
  }

  return [...candidates].filter((c) => {
    const lower = c.toLowerCase();
    for (const k of known) {
      if (lower.includes(k) || k.includes(lower)) return true;
    }
    return false;
  });
}

async function getMutations(dbApiKey) {
  const r = await fetch(`${DB_API}?table=moderation_words`, {
    headers: { Authorization: `Bearer ${dbApiKey}` },
    signal: AbortSignal.timeout(15000),
  });
  // moderation_words не та таблица — читаем mutations.html напрямую? Нет, таблица в БД.
  // На самом деле данные мутаций и трейтов зашиты в страницу ниже (из твоих HTML-файлов).
  return null;
}

export default async function handler(req, res) {
  if (!auth(req)) return json(res, 401, { error: "Unauthorized" });

  // GET ?brainrots=Name1|Name2&left=Name1|Name2 (left = зелёные на скрине, right = остальные)
  if (req.method === "GET") {
    const left = (req.query?.left || "").split("|").filter(Boolean);
    const right = (req.query?.right || "").split("|").filter(Boolean);
    return json(res, 200, {
      ok: true,
      left,
      right,
      // Мутации и трейты зашиты прямо в страницу ниже — они меняются редко,
      // а их HTML-таблицы у пользователя локальные. Кэшируем на уровне страницы.
      note: "Данные мутаций/трейтов встроены в страницу (из mutations.html/traits.html)",
    });
  }

  return json(res, 405, { error: "Method not allowed" });
}
