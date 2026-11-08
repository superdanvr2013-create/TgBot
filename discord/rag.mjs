// RAG module for the Discord bot.
// - embedQuery(): vectorize a query via Mistral AI (mistral-embed, 1024 dims)
// - searchKnowledge(): cosine-similarity query against Supabase (pgvector)
// - buildRagContext(): top-k -> context string injected into the Groq prompt
import pg from "pg";

const {
  DATABASE_URL,
  MISTRAL_API_KEY,
  EMBED_URL = "https://api.mistral.ai/v1/embeddings",
  EMBED_MODEL = "mistral-embed",
  TOP_K = "4",
} = process.env;
const topK = Number.parseInt(TOP_K, 10) || 4;

let pool;
function getPool() {
  if (!pool) {
    if (!DATABASE_URL) throw new Error("DATABASE_URL not set");
    pool = new pg.Pool({
      connectionString: DATABASE_URL,
      max: 4,
      idleTimeoutMillis: 30000,
      connectionTimeoutMillis: 15000,
      keepAlive: true,
      keepAliveInitialDelayMillis: 10000,
    });
    pool.on("error", (err) => {
      console.error("[pg pool] idle client error:", err.message);
      pool = null; // force reconnect on next use
    });
  }
  return pool;
}

// Vectorize a text using Mistral AI (mistral-embed, openai-compatible /v1/embeddings).
export async function embedQuery(text) {
  if (!text || !text.trim()) return null;
  if (!MISTRAL_API_KEY) throw new Error("MISTRAL_API_KEY not set");
  const res = await fetch(EMBED_URL, {
    method: "POST",
    headers: {
      Authorization: `Bearer ${MISTRAL_API_KEY}`,
      "Content-Type": "application/json",
    },
    body: JSON.stringify({ input: text.trim(), model: EMBED_MODEL }),
    signal: AbortSignal.timeout(30000),
  });
  if (!res.ok) {
    const detail = await res.text();
    throw new Error(`Embed ${res.status}: ${detail.slice(0, 300)}`);
  }
  const data = await res.json();
  const vec = data?.data?.[0]?.embedding;
  if (!Array.isArray(vec) || vec.length === 0) throw new Error("Empty embedding");
  return vec;
}

// Search knowledge by cosine similarity (<=> operator for cosine similarity).
export async function searchKnowledge(embedding, limit = topK) {
  const client = await getPool().connect();
  try {
    const { rows } = await client.query(
      `SELECT id, theme, description,
              1 - (embedding <=> $1::vector) AS similarity
         FROM knowledge
        WHERE embedding IS NOT NULL
        ORDER BY embedding <=> $1::vector
        LIMIT $2`,
      [JSON.stringify(embedding), limit]
    );
    return rows;
  } finally {
    client.release();
  }
}

// Build a context block from retrieved rows.
export function buildRagContext(rows) {
  if (!rows || rows.length === 0) return "";
  return (
    "Ниже — информация из базы знаний Dan Games, которая может помочь в ответе:\n" +
    rows
      .map(
        (r, i) =>
          `${i + 1}. Тема: ${r.theme}\n   ${r.description}` +
          (r.similarity != null ? ` (релевантность ${r.similarity.toFixed(3)})` : "")
      )
      .join("\n")
  );
}

// Full RAG pipeline: returns { answer } or { ragContext } in case of retrieval failure (graceful).
export async function retrieve(question) {
  const embedding = await embedQuery(question);
  const rows = await searchKnowledge(embedding);
  return buildRagContext(rows);
}