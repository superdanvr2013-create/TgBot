import "dotenv/config";
import { readFileSync } from "node:fs";
import pg from "pg";
const client = new pg.Client({ connectionString: process.env.DATABASE_URL, ssl: false, connectionTimeoutMillis: 15000 });
for (let i = 1; i <= 5; i++) {
  try { await client.connect(); break; } catch (e) { if (i === 5) throw e; await new Promise(r => setTimeout(r, i * 3000)); }
}
await client.query(readFileSync(new URL("../db/individual.sql", import.meta.url), "utf8"));
const r = await client.query("select column_name, data_type from information_schema.columns where table_name='individual' order by ordinal_position");
console.log("individual:", r.rows.map(x => `${x.column_name}(${x.data_type})`).join(", "));
// seed: обращения для владельца
await client.query(
  `INSERT INTO individual (nickname, description) VALUES ($1,$2)
   ON CONFLICT (nickname) DO NOTHING`,
  [
    "danvr2013",
    "Привет босс; Доброго денечка Дэн; Добрый вечер босс; Да конечно босс, сейчас скажу; Слушаю внимательно, босс",
  ]
);
const c = await client.query("select nickname, description from individual");
console.log("rows:", JSON.stringify(c.rows));
await client.end();
