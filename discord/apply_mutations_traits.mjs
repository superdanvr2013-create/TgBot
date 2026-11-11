import "dotenv/config";
import { readFileSync } from "node:fs";
import pg from "pg";

const client = new pg.Client({ connectionString: process.env.DATABASE_URL, ssl: false, connectionTimeoutMillis: 15000 });

async function query(text, params) {
  for (let i = 1; i <= 5; i++) {
    try {
      return await client.query(text, params);
    } catch (e) {
      if (i === 5) throw e;
      await new Promise(r => setTimeout(r, i * 3000));
    }
  }
}

for (let i = 1; i <= 5; i++) {
  try { await client.connect(); break; } catch (e) { if (i === 5) throw e; await new Promise(r => setTimeout(r, i * 3000)); }
}

await query(readFileSync(new URL("../db/mutations_traits.sql", import.meta.url), "utf8"));
console.log("tables created");

// Сид данных из JSON-файлов
const mutations = JSON.parse(readFileSync(new URL("../mutations.json", import.meta.url), "utf8"));
const traits = JSON.parse(readFileSync(new URL("../traits.json", import.meta.url), "utf8"));

for (const m of mutations) {
  await query(
    `INSERT INTO mutations (name, multi, icon, description) VALUES ($1,$2,$3,$4)
     ON CONFLICT (name) DO UPDATE SET multi=EXCLUDED.multi, icon=EXCLUDED.icon, description=EXCLUDED.description`,
    [m.name, m.multi, m.icon, m.desc]
  );
}

for (const t of traits) {
  await query(
    `INSERT INTO traits (name, multiplier, icon, description) VALUES ($1,$2,$3,$4)
     ON CONFLICT (name) DO UPDATE SET multiplier=EXCLUDED.multiplier, icon=EXCLUDED.icon, description=EXCLUDED.description`,
    [t.name, t.multiplier, t.icon, t.desc]
  );
}

const cm = await query("select count(*)::int as c from mutations");
const ct = await query("select count(*)::int as c from traits");
console.log(`mutations: ${cm.rows[0].c}, traits: ${ct.rows[0].c}`);
await client.end();
