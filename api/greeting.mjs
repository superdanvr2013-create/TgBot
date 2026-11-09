// Персональные обращения (вариант 2): ИИ выбирает вариант из списка по контексту.
// Кэш 60 сек: правки из C#-редактора подхватываются за минуту.
import pg from "pg";

const { DATABASE_URL } = process.env;

let pool;
function getPool() {
  if (!DATABASE_URL) throw new Error("DATABASE_URL not set");
  if (!pool) {
    pool = new pg.Pool({
      connectionString: DATABASE_URL,
      max: 2,
      connectionTimeoutMillis: 10000,
      idleTimeoutMillis: 15000,
    });
  }
  return pool;
}

let cache = null;
let cacheTime = 0;
const TTL = 60000;

async function loadAll() {
  const now = Date.now();
  if (cache && now - cacheTime < TTL) return cache;
  const r = await getPool().query("SELECT nickname, description FROM individual");
  cache = r.rows;
  cacheTime = now;
  return cache;
}

/**
 * Возвращает строку-инструкцию для system-промпта, если у ника есть персональные
 * обращения. Модель обязана начать ответ одним из вариантов, выбирая по контексту.
 * @returns {string} "" если ника нет в таблице
 */
export async function getPersonalGreetingInstruction(username) {
  if (!username) return "";
  try {
    const rows = await loadAll();
    const row = rows.find((r) => r.nickname.toLowerCase() === username.toLowerCase());
    if (!row) return "";
    const variants = row.description
      .split(";")
      .map((s) => s.trim())
      .filter(Boolean);
    if (!variants.length) return "";
    const list = variants.map((v) => `«${v}»`).join("\n");
    return (
      `\n\nОбращение к пользователю: твой собеседник — ${row.nickname}. ` +
      `Начни свой ответ с одного из вариантов персонального обращения (выбери одно, наиболее подходящее по контексту вопроса, времени суток и настроению — строго из списка):\n${list}\n`
    );
  } catch (err) {
    console.warn("greeting load skipped:", err.message);
    return "";
  }
}
