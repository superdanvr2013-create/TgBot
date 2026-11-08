// Verify RAG retrieval against the real Supabase DB:
//   embed a query via Mistral -> cosine search in pgvector -> print top-k.
// Usage: node rag_db_check.mjs "как пополнить баланс?"
import "dotenv/config";

const DATABASE_URL = process.env.DATABASE_URL;
const MISTRAL_API_KEY = process.env.MISTRAL_API_KEY;
if (!DATABASE_URL) { console.error("DATABASE_URL not set"); process.exit(1); }
if (!MISTRAL_API_KEY) { console.error("MISTRAL_API_KEY not set"); process.exit(1); }

const { embedQuery, searchKnowledge, buildRagContext } = await import("./rag.mjs");

const question = process.argv[2] || "какие способы пополнения есть?";
console.log("Query:", question);

const vec = await embedQuery(question);
console.log("Embedded:", vec.length, "dims");

const rows = await searchKnowledge(vec, 4);
console.log("\nTop-k from knowledge table:");
rows.forEach((r, i) => console.log(`${i + 1}. [${(r.similarity ?? 0).toFixed(4)}] ${r.theme}`));

const ctx = buildRagContext(rows);
console.log("\n--- Injected context (first 300 chars) ---");
console.log(ctx.slice(0, 300));

if (rows.length === 0) {
  console.error("\nFAIL: no rows returned — seed the DB first (node seed_knowledge.mjs)");
  process.exit(2);
}
console.log("\nPASS: real-DB retrieval works.");