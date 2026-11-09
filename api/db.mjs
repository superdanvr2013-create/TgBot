// SQL-прокси для KnowledgeEditor.exe: CRUD для knowledge и moderation_words.
// Десктоп из РФ не может напрямую в Supabase — этот endpoint выполняет SQL в облаке Vercel.
// Безопасность: только whitelist-операции, авторизация по POLL_SECRET (Bearer).
import pg from "pg";

const { DATABASE_URL, POLL_SECRET } = process.env;

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
    pool.on("error", (err) => {
      console.error("[db api pool] idle error:", err.message);
      pool = null;
    });
  }
  return pool;
}

function auth(req) {
  const bearer = req.headers.authorization?.match(/^Bearer\s+(.+)$/i)?.[1];
  return Boolean(POLL_SECRET && bearer === POLL_SECRET);
}

function json(res, status, body) {
  res.status(status).json(body);
}

async function withRetry(fn, attempts = 3) {
  let last;
  for (let i = 1; i <= attempts; i++) {
    try {
      return await fn();
    } catch (err) {
      last = err;
      if (i < attempts) await new Promise((r) => setTimeout(r, i * 1000));
      else throw last;
    }
  }
}

// ── Валидация входных данных ────────────────────────────────────────────────

function bad(value, name) {
  if (typeof value !== "string" || !value.trim()) return `${name} обязателен`;
  return null;
}

export default async function handler(req, res) {
  if (!auth(req)) return json(res, 401, { error: "Unauthorized" });

  // GET /api/db?table=knowledge|words[&filter=...]
  if (req.method === "GET") {
    const table = req.query?.table;
    const filter = req.query?.filter;
    try {
      if (table === "knowledge") {
        const rows = await withRetry(() =>
          getPool().query(
            `SELECT id, theme, description, created_at, updated_at,
                    (embedding IS NOT NULL) AS has_embedding
               FROM knowledge ORDER BY updated_at DESC LIMIT 500`
          )
        );
        return json(res, 200, { ok: true, rows: rows.rows });
      }
      if (table === "providers") {
        const rows = await withRetry(() =>
          getPool().query(
            `SELECT id, name, base_url, api_key, model, description, active, sort_order, updated_at
               FROM ai_providers ORDER BY active DESC, sort_order ASC, id ASC`
          )
        );
        return json(res, 200, { ok: true, rows: rows.rows });
      }
      if (table === "individual") {
        const rows = await withRetry(() =>
          getPool().query(
            "SELECT id, nickname, description, updated_at FROM individual ORDER BY nickname"
          )
        );
        return json(res, 200, { ok: true, rows: rows.rows });
      }
      if (table === "words") {
        const params = [];
        let where = "";
        if (filter) {
          params.push(`%${filter}%`);
          where = "WHERE word ILIKE $1";
        }
        const rows = await withRetry(() =>
          getPool().query(
            `SELECT id, word, note, added_at FROM moderation_words ${where} ORDER BY word`,
            params
          )
        );
        return json(res, 200, { ok: true, rows: rows.rows });
      }
      return json(res, 400, { error: "table must be knowledge or words" });
    } catch (err) {
      console.error("db GET error:", err.message);
      return json(res, 500, { ok: false, error: err.message });
    }
  }

  // POST /api/db — команда в теле JSON
  if (req.method === "POST") {
    let body;
    try {
      body = typeof req.body === "string" ? JSON.parse(req.body) : req.body;
    } catch {
      return json(res, 400, { error: "Invalid JSON body" });
    }
    const { op, table } = body || {};
    if (!op || !table) return json(res, 400, { error: "op and table required" });

    try {
      // ── knowledge ──────────────────────────────────────────────
      if (table === "knowledge") {
        if (op === "upsert") {
          const err = bad(body.theme, "theme") || bad(body.description, "description") || bad(body.embedding, "embedding");
          if (err) return json(res, 400, { error: err });
          const r = await withRetry(() =>
            getPool().query(
              `INSERT INTO knowledge (theme, description, embedding)
               VALUES ($1, $2, $3::vector)
               ON CONFLICT (theme) DO UPDATE
                 SET description = EXCLUDED.description,
                     embedding = EXCLUDED.embedding,
                     updated_at = now()
               RETURNING id`,
              [body.theme, body.description, body.embedding]
            )
          );
          return json(res, 200, { ok: true, id: r.rows[0].id });
        }
        if (op === "update") {
          const err = bad(body.theme, "theme") || bad(body.description, "description") || bad(body.embedding, "embedding");
          if (err || !Number.isInteger(body.id)) return json(res, 400, { error: err || "id обязателен" });
          const r = await withRetry(() =>
            getPool().query(
              `UPDATE knowledge SET theme=$1, description=$2, embedding=$3::vector, updated_at=now()
               WHERE id=$4 RETURNING id`,
              [body.theme, body.description, body.embedding, body.id]
            )
          );
          if (!r.rows.length) return json(res, 404, { ok: false, error: `Запись id=${body.id} не найдена` });
          return json(res, 200, { ok: true, id: r.rows[0].id });
        }
        if (op === "delete") {
          if (!Number.isInteger(body.id)) return json(res, 400, { error: "id обязателен" });
          await withRetry(() => getPool().query("DELETE FROM knowledge WHERE id=$1", [body.id]));
          return json(res, 200, { ok: true });
        }
      }

      // ── moderation_words ───────────────────────────────────────
      if (table === "words") {
        if (op === "add") {
          const err = bad(body.word, "word");
          if (err) return json(res, 400, { error: err });
          const r = await withRetry(() =>
            getPool().query(
              `INSERT INTO moderation_words (word, note)
               VALUES ($1, $2)
               ON CONFLICT (word) DO UPDATE SET note = EXCLUDED.note
               RETURNING id`,
              [body.word, body.note ?? null]
            )
          );
          return json(res, 200, { ok: true, id: r.rows[0].id });
        }
        if (op === "update") {
          const err = bad(body.word, "word");
          if (err || !Number.isInteger(body.id)) return json(res, 400, { error: err || "id обязателен" });
          const r = await withRetry(() =>
            getPool().query(
              `UPDATE moderation_words SET word=$1, note=$2 WHERE id=$3 RETURNING id`,
              [body.word, body.note ?? null, body.id]
            )
          );
          if (!r.rows.length) return json(res, 404, { ok: false, error: `Слово id=${body.id} не найдено` });
          return json(res, 200, { ok: true, id: r.rows[0].id });
        }
        if (op === "delete") {
          if (!Number.isInteger(body.id)) return json(res, 400, { error: "id обязателен" });
          await withRetry(() => getPool().query("DELETE FROM moderation_words WHERE id=$1", [body.id]));
          return json(res, 200, { ok: true });
        }
      }

      // ── ai_providers ───────────────────────────────────────────
      if (table === "providers") {
        if (op === "add") {
          const err = bad(body.name, "name") || bad(body.base_url, "base_url") || bad(body.api_key, "api_key") || bad(body.model, "model");
          if (err) return json(res, 400, { error: err });
          const active = body.active === true;
          if (active) {
            // снять галочку со всех, активна только одна
            await withRetry(() => getPool().query("UPDATE ai_providers SET active=false"));
          }
          const r = await withRetry(() =>
            getPool().query(
              `INSERT INTO ai_providers (name, base_url, api_key, model, description, active, sort_order)
               VALUES ($1,$2,$3,$4,$5,$6,$7)
               ON CONFLICT DO NOTHING
               RETURNING id`,
              [body.name, body.base_url, body.api_key, body.model, body.description ?? null, active, Number.isInteger(body.sort_order) ? body.sort_order : 100]
            )
          );
          return json(res, 200, { ok: true, id: r.rows[0].id });
        }
        if (op === "update") {
          const err = bad(body.name, "name") || bad(body.base_url, "base_url") || bad(body.api_key, "api_key") || bad(body.model, "model");
          if (err || !Number.isInteger(body.id)) return json(res, 400, { error: err || "id обязателен" });
          const active = body.active === true;
          if (active) {
            await withRetry(() => getPool().query("UPDATE ai_providers SET active=false WHERE id<>$1", [body.id]));
          }
          const r = await withRetry(() =>
            getPool().query(
              `UPDATE ai_providers
                  SET name=$1, base_url=$2, api_key=$3, model=$4, description=$5, active=$6, sort_order=$7, updated_at=now()
                WHERE id=$8 RETURNING id`,
              [body.name, body.base_url, body.api_key, body.model, body.description ?? null, active, Number.isInteger(body.sort_order) ? body.sort_order : 100, body.id]
            )
          );
          if (!r.rows.length) return json(res, 404, { ok: false, error: `Провайдер id=${body.id} не найден` });
          return json(res, 200, { ok: true, id: r.rows[0].id });
        }
        if (op === "activate") {
          // поставить галочку приоритета на одну строку
          if (!Number.isInteger(body.id)) return json(res, 400, { error: "id обязателен" });
          await withRetry(() => getPool().query("UPDATE ai_providers SET active=false"));
          await withRetry(() => getPool().query("UPDATE ai_providers SET active=true, updated_at=now() WHERE id=$1", [body.id]));
          return json(res, 200, { ok: true });
        }
        if (op === "delete") {
          if (!Number.isInteger(body.id)) return json(res, 400, { error: "id обязателен" });
          await withRetry(() => getPool().query("DELETE FROM ai_providers WHERE id=$1", [body.id]));
          return json(res, 200, { ok: true });
        }
      }

      // ── individual (персональные обращения) ────────────────────
      if (table === "individual") {
        if (op === "add") {
          const err = bad(body.nickname, "nickname") || bad(body.description, "description");
          if (err) return json(res, 400, { error: err });
          const r = await withRetry(() =>
            getPool().query(
              `INSERT INTO individual (nickname, description)
               VALUES ($1, $2)
               ON CONFLICT (nickname) DO UPDATE SET description = EXCLUDED.description, updated_at = now()
               RETURNING id`,
              [body.nickname, body.description]
            )
          );
          return json(res, 200, { ok: true, id: r.rows[0].id });
        }
        if (op === "update") {
          const err = bad(body.nickname, "nickname") || bad(body.description, "description");
          if (err || !Number.isInteger(body.id)) return json(res, 400, { error: err || "id обязателен" });
          const r = await withRetry(() =>
            getPool().query(
              `UPDATE individual SET nickname=$1, description=$2, updated_at=now()
               WHERE id=$3 RETURNING id`,
              [body.nickname, body.description, body.id]
            )
          );
          if (!r.rows.length) return json(res, 404, { ok: false, error: `Запись id=${body.id} не найдена` });
          return json(res, 200, { ok: true, id: r.rows[0].id });
        }
        if (op === "delete") {
          if (!Number.isInteger(body.id)) return json(res, 400, { error: "id обязателен" });
          await withRetry(() => getPool().query("DELETE FROM individual WHERE id=$1", [body.id]));
          return json(res, 200, { ok: true });
        }
      }

      return json(res, 400, { error: `unknown table/op: ${table}/${op}` });
    } catch (err) {
      console.error("db POST error:", err.message);
      return json(res, 500, { ok: false, error: err.message });
    }
  }

  return json(res, 405, { error: "Method not allowed" });
}
