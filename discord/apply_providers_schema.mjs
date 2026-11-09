import "dotenv/config";
import { readFileSync } from "node:fs";
import pg from "pg";
const client = new pg.Client({ connectionString: process.env.DATABASE_URL, ssl: false, connectionTimeoutMillis: 15000 });
for (let i = 1; i <= 5; i++) {
  try { await client.connect(); break; } catch (e) { if (i === 5) throw e; await new Promise(r => setTimeout(r, i * 3000)); }
}
await client.query(readFileSync(new URL("../db/ai_providers.sql", import.meta.url), "utf8"));
const r = await client.query("select column_name, data_type from information_schema.columns where table_name='ai_providers' order by ordinal_position");
console.log("ai_providers:", r.rows.map(x => `${x.column_name}(${x.data_type})`).join(", "));
// seed: твой новый провайдер как активный
await client.query(
  `INSERT INTO ai_providers (name, base_url, api_key, model, description, active, sort_order)
   VALUES ($1,$2,$3,$4,$5,true,1)
   ON CONFLICT DO NOTHING`,
  [
    "Gateway (sslip)",
    "https://177-3-220-29.sslip.io/v1",
    "gw_sk_uOJYhGLXQvrxgwBFt9ui83uXhEdGppwWwEUJ5GokD40",
    "deepseek-v4.1-flash-1017",
    "Платный шлюз, deepseek-v4.1-flash-1017",
  ]
);
const c = await client.query("select id,name,model,active from ai_providers");
console.log("rows:", JSON.stringify(c.rows));
await client.end();
