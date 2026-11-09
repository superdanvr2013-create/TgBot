// Мульти-провайдерный ИИ-клиент для ботов.
// Читает конфигурацию из таблицы ai_providers (Supabase):
//   - строка с active=true — приоритетная (галочка в редакторе);
//   - при сбое (401/403/429/5xx/timeout) — фолбэк на следующие по sort_order.
// Кэш конфигурации 60 сек: правки в редакторе подхватываются за минуту.
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
      keepAlive: true,
    });
  }
  return pool;
}

let providersCache = null;
let providersCacheTime = 0;
const PROVIDERS_TTL_MS = 60000;

async function loadProviders() {
  const now = Date.now();
  if (providersCache && now - providersCacheTime < PROVIDERS_TTL_MS) return providersCache;
  const r = await getPool().query(
    "SELECT id, name, base_url, api_key, model FROM ai_providers ORDER BY active DESC, sort_order ASC, id ASC"
  );
  providersCache = r.rows;
  providersCacheTime = now;
  return providersCache;
}

// Память о временно недоступных провайдерах: providerId → время до которого не пробуем.
const cooldowns = new Map();
const COOLDOWN_MS = 60000; // 401/403/429/5xx → отдых 60 сек

function isCooling(id) {
  const until = cooldowns.get(id);
  return Boolean(until && until > Date.now());
}

function markCooldown(id) {
  cooldowns.set(id, Date.now() + COOLDOWN_MS);
}

/**
 * Спрашивает ИИ с ротацией провайдеров.
 * @returns {string} ответ модели
 * @throws если все провайдеры недоступны
 */
export async function askModel(messages, maxTokens = 800) {
  const providers = await loadProviders();
  if (!providers.length) throw new Error("Нет настроенных ИИ-провайдеров (ai_providers)");

  const errors = [];
  for (const p of providers) {
    if (isCooling(p.id)) {
      errors.push(`${p.name}: cooldown`);
      continue;
    }
    try {
      const response = await fetch(`${p.base_url.replace(/\/$/, "")}/chat/completions`, {
        method: "POST",
        headers: {
          Authorization: `Bearer ${p.api_key}`,
          "Content-Type": "application/json",
        },
        body: JSON.stringify({
          model: p.model,
          messages,
          max_tokens: maxTokens,
          temperature: 0.7,
        }),
        signal: AbortSignal.timeout(30000),
      });
      if (!response.ok) {
        const detail = await response.text();
        // невалидный ключ / лимит / серверная ошибка → следующий провайдер
        if ([401, 402, 403, 429, 500, 502, 503, 504].includes(response.status)) {
          markCooldown(p.id);
          errors.push(`${p.name}: HTTP ${response.status}`);
          continue;
        }
        throw new Error(`${p.name}: HTTP ${response.status}: ${detail.slice(0, 200)}`);
      }
      const data = await response.json();
      const answer = data?.choices?.[0]?.message?.content;
      if (!answer?.trim()) throw new Error(`${p.name}: пустой ответ`);
      return answer.trim();
    } catch (err) {
      if (err.name === "TimeoutError" || err.name === "AbortError" || err.name === "TypeError") {
        markCooldown(p.id);
        errors.push(`${p.name}: timeout/сеть`);
        continue;
      }
      errors.push(`${p.name}: ${err.message}`);
    }
  }
  throw new Error(`Все ИИ-провайдеры недоступны: ${errors.join("; ")}`);
}
