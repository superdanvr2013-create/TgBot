// Применяет roulette.sql к базе и проверяет наличие таблицы.
import "dotenv/config";
import { readFileSync } from "node:fs";
import pg from "pg";

const client = new pg.Client({ connectionString: process.env.DATABASE_URL, ssl: false, max: 1 });
await client.connect();
const sql = readFileSync(new URL("../db/roulette.sql", import.meta.url), "utf8");
await client.query(sql);
const check = await client.query(
  "select column_name, data_type from information_schema.columns where table_name='roulette_participants' order by ordinal_position"
);
console.log("SCHEMA OK:", check.rows.map((r) => `${r.column_name}(${r.data_type})`).join(", "));
await client.end();