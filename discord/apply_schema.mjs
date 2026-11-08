// Apply knowledge.sql + optional seed to a Supabase Postgres database.
// Usage: node apply_schema.mjs "postgresql://..." [--seed]
import { readFileSync } from "node:fs";
import { fileURLToPath } from "node:url";
import { dirname, join } from "node:path";
import pg from "pg";

const __dirname = dirname(fileURLToPath(import.meta.url));
const DATABASE_URL = process.argv[2] || process.env.DATABASE_URL;
if (!DATABASE_URL) {
  console.error("Usage: node apply_schema.mjs <DATABASE_URL> [--seed]");
  process.exit(1);
}

const sql = readFileSync(join(__dirname, "..", "db", "knowledge.sql"), "utf8");

const client = new pg.Client({ connectionString: DATABASE_URL, connectionTimeoutMillis: 15000 });
try {
  await client.connect();
  console.log("CONNECTED. Applying schema...");
  await client.query(sql);
  console.log("SCHEMA APPLIED (pgvector + knowledge table).");

  // verify
  const ext = await client.query("select extname from pg_extension where extname='vector'");
  console.log("pgvector installed:", ext.rows.length > 0);

  const cols = await client.query(
    "select column_name, data_type, udt_name from information_schema.columns where table_name='knowledge' order by ordinal_position"
  );
  console.log("knowledge columns:", JSON.stringify(cols.rows, null, 2));

  const seed = process.argv.includes("--seed");
  if (seed) {
    console.log("Seeding skipped (embedding required).");
  }
} catch (e) {
  console.error("FAILED:", e.message);
  process.exit(2);
} finally {
  await client.end().catch(() => {});
}