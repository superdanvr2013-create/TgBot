import "dotenv/config";
import { readFileSync } from "node:fs";
import pg from "pg";
const client = new pg.Client({ connectionString: process.env.DATABASE_URL, ssl: false, connectionTimeoutMillis: 15000 });
await client.connect();
await client.query(readFileSync(new URL("../db/moderation_words.sql", import.meta.url), "utf8"));
const r = await client.query("select column_name, data_type from information_schema.columns where table_name='moderation_words' order by ordinal_position");
console.log("moderation_words:", r.rows.map(x => `${x.column_name}(${x.data_type})`).join(", "));
await client.end();
