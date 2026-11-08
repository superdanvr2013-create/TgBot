// Seed the knowledge base with initial Dan Games entries.
// For each entry: embed (theme + description) via Mistral, then upsert into Supabase.
// Usage: node seed_knowledge.mjs
import "dotenv/config";
import pg from "pg";

const MISTRAL_API_KEY = process.env.MISTRAL_API_KEY;
const DATABASE_URL = process.env.DATABASE_URL;
const EMBED_URL = process.env.EMBED_URL || "https://api.mistral.ai/v1/embeddings";
const EMBED_MODEL = process.env.EMBED_MODEL || "mistral-embed";

if (!MISTRAL_API_KEY) { console.error("MISTRAL_API_KEY not set"); process.exit(1); }
if (!DATABASE_URL) { console.error("DATABASE_URL not set"); process.exit(1); }

const SEED = [
  ["Режим работы", "Dan Games работает ежедневно с 10:00 до 22:00 по московскому времени. В остальное время серверы на техническом обслуживании."],
  ["Минимальные требования", "Для игры на сервере Dan Games нужен ПК с процессором уровня Intel Core i5-4460, 8 ГБ оперативной памяти, видеокартой уровня GTX 1060 и 20 ГБ свободного места на диске."],
  ["Способы пополнения", "Пополнить баланс игрового аккаунта можно через банковскую карту, СБП (систему быстрых платежей) или криптовалюту USDT. При пополнении от 1000 рублей через криптовалюту действует скидка 5%."],
  ["Правила сервера", "На сервере запрещены читы, гриферство и оскорбления других игроков. За первое нарушение выдаётся предупреждение, за повторное — временный бан на 7 дней."],
];

async function embed(texts) {
  const res = await fetch(EMBED_URL, {
    method: "POST",
    headers: { Authorization: `Bearer ${MISTRAL_API_KEY}`, "Content-Type": "application/json" },
    body: JSON.stringify({ model: EMBED_MODEL, input: texts }),
  });
  if (!res.ok) throw new Error(`embed ${res.status}: ${await res.text()}`);
  const data = await res.json();
  return data.data.map((d) => d.embedding);
}

const pool = new pg.Pool({ connectionString: DATABASE_URL, max: 4, ssl: { rejectUnauthorized: false } });
try {
  const texts = SEED.map(([t, d]) => `${t}\n${d}`);
  console.log("Embedding", texts.length, "seed entries via", EMBED_MODEL, "...");
  const vecs = await embed(texts);

  const client = await pool.connect();
  try {
    for (let i = 0; i < SEED.length; i++) {
      const [theme, description] = SEED[i];
      const vec = "[" + vecs[i].join(",") + "]";
      await client.query(
        `INSERT INTO knowledge (theme, description, embedding)
         VALUES ($1, $2, $3::vector)
         ON CONFLICT (theme) DO UPDATE
           SET description = EXCLUDED.description,
               embedding = EXCLUDED.embedding,
               updated_at = now()`,
        [theme, description, vec]
      );
      console.log(`  seeded: ${theme} (${vecs[i].length} dims)`);
    }
    const cnt = await client.query("SELECT count(*)::int AS n FROM knowledge");
    console.log(`DONE. knowledge rows = ${cnt.rows[0].n}`);
  } finally {
    client.release();
  }
} catch (e) {
  console.error("SEED FAILED:", e.message);
  process.exit(2);
} finally {
  await pool.end().catch(() => {});
}