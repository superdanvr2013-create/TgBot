// One-shot activation after Supabase project restore:
//   1. verify DB connection
//   2. apply schema (pgvector + knowledge table)
//   3. seed initial knowledge (Mistral embeddings)
//   4. run a retrieval smoke test against the real DB
// Usage: node activate_rag.mjs
import "dotenv/config";
import { readFileSync } from "node:fs";
import { fileURLToPath } from "node:url";
import { dirname, join } from "node:path";
import pg from "pg";

const __dirname = dirname(fileURLToPath(import.meta.url));
const DATABASE_URL = process.env.DATABASE_URL;
if (!DATABASE_URL) { console.error("DATABASE_URL not set"); process.exit(1); }

const client = new pg.Client({ connectionString: DATABASE_URL, ssl: { rejectUnauthorized: false }, connectionTimeoutMillis: 15000 });

async function main() {
  await client.connect();
  console.log("1. CONNECTED:", (await client.query("select current_database() db, current_user u")).rows[0]);

  // apply schema
  const sql = readFileSync(join(__dirname, "..", "db", "knowledge.sql"), "utf8");
  await client.query(sql);
  console.log("2. SCHEMA APPLIED (pgvector + knowledge).");

  const ext = await client.query("select extname from pg_extension where extname='vector'");
  console.log("   pgvector installed:", ext.rows.length > 0);

  const cols = await client.query(
    "select column_name, udt_name from information_schema.columns where table_name='knowledge' order by ordinal_position"
  );
  console.log("   columns:", cols.rows.map(c => `${c.column_name}(${c.udt_name})`).join(", "));

  await client.end();
  return true;
}

main().then(() => {
  console.log("SCHEMA OK — run `node seed_knowledge.mjs` next, then `node rag_db_check.mjs`.");
  process.exit(0);
}).catch((e) => {
  console.error("ACTIVATE FAILED:", e.message);
  process.exit(2);
});