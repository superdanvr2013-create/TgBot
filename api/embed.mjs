// Прокси эмбеддингов: KnowledgeEditor (РФ) → Vercel (США) → Mistral.
// Решает проблему DPI: из РФ Mistral рвёт TLS при payload > ~12 КБ.
// Лимит Mistral — 8192 токена на вход (~8 тыс. слов). Прокси сам режет текст
// на чанки по ~6000 токенов (≈18 КБ), кодирует каждый и усредняет векторы.
import pg from "pg";

const { MISTRAL_API_KEY, POLL_SECRET } = process.env;
const EMBED_URL = process.env.EMBED_URL || "https://api.mistral.ai/v1/embeddings";
const EMBED_MODEL = process.env.EMBED_MODEL || "mistral-embed";

// безопасный размер чанка: 8192 токенов ≈ 4 символа/токен для mixed text.
// 6000 символов * ~1.5 токена ≈ безопасно ниже лимита даже для русского.
const CHUNK_CHARS = 6000;

function auth(req) {
  const bearer = req.headers.authorization?.match(/^Bearer\s+(.+)$/i)?.[1];
  return Boolean(POLL_SECRET && bearer === POLL_SECRET);
}

function json(res, status, body) {
  res.status(status).json(body);
}

async function embedChunk(text, attempt = 1) {
  const r = await fetch(EMBED_URL, {
    method: "POST",
    headers: {
      Authorization: `Bearer ${MISTRAL_API_KEY}`,
      "Content-Type": "application/json",
    },
    body: JSON.stringify({ model: EMBED_MODEL, input: text }),
    signal: AbortSignal.timeout(60000),
  });
  if (!r.ok) {
    const t = await r.text();
    throw new Error(`Mistral ${r.status}: ${t.slice(0, 300)}`);
  }
  return (await r.json()).data[0].embedding;
}

async function embedChunkRetry(text, attempts = 3) {
  let last;
  for (let i = 1; i <= attempts; i++) {
    try {
      return await embedChunk(text);
    } catch (err) {
      last = err;
      await new Promise((r) => setTimeout(r, i * 2000));
    }
  }
  throw last;
}

export default async function handler(req, res) {
  if (!auth(req)) return json(res, 401, { error: "Unauthorized" });
  if (req.method !== "POST") return json(res, 405, { error: "Method not allowed" });

  let body;
  try {
    body = typeof req.body === "string" ? JSON.parse(req.body) : req.body;
  } catch {
    return json(res, 400, { error: "Invalid JSON body" });
  }
  const input = body?.input;
  if (typeof input !== "string" || !input.trim()) {
    return json(res, 400, { error: "input required" });
  }
  if (!MISTRAL_API_KEY) return json(res, 500, { error: "MISTRAL_API_KEY not set" });
  if (input.length > 1_000_000) {
    return json(res, 400, { error: "input too large (max 1M chars)" });
  }

  try {
    // Маленький текст — один запрос
    if (input.length <= CHUNK_CHARS) {
      const vec = await embedChunkRetry(input);
      return json(res, 200, { ok: true, embedding: vec, dims: vec.length, chunks: 1 });
    }

    // Большой: режем по границам строк
    const chunks = [];
    let remaining = input;
    while (remaining.length > 0) {
      if (remaining.length <= CHUNK_CHARS) {
        chunks.push(remaining);
        break;
      }
      let cut = remaining.lastIndexOf("\n", CHUNK_CHARS);
      if (cut < CHUNK_CHARS * 0.5) cut = CHUNK_CHARS; // нет переносов — режем жёстко
      chunks.push(remaining.slice(0, cut));
      remaining = remaining.slice(cut);
    }

    // Последовательное кодирование чанков (стабильнее параллельного)
    const vecs = [];
    for (const chunk of chunks) {
      vecs.push(await embedChunkRetry(chunk));
    }

    // Усреднение + L2-нормализация
    const dim = vecs[0].length;
    const avg = new Array(dim).fill(0);
    for (const v of vecs) {
      for (let d = 0; d < dim; d++) avg[d] += v[d];
    }
    for (let d = 0; d < dim; d++) avg[d] /= vecs.length;
    const norm = Math.sqrt(avg.reduce((s, x) => s + x * x, 0));
    const final = norm > 0 ? avg.map((x) => x / norm) : avg;

    return json(res, 200, { ok: true, embedding: final, dims: dim, chunks: chunks.length });
  } catch (err) {
    console.error("embed proxy error:", err.message);
    return json(res, 500, { ok: false, error: err.message });
  }
}
