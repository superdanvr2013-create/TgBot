import "dotenv/config";
import { readFileSync } from "node:fs";

// Сид через Vercel db API — обход локального DPI к Supabase.
const SECRET = readFileSync(process.env.SECRET_FILE, "utf8").trim();
const BASE = "https://dan-games-discord-poller.vercel.app/api/db";

const mutations = JSON.parse(readFileSync(new URL("../mutations.json", import.meta.url), "utf8"));
const traits = JSON.parse(readFileSync(new URL("../traits.json", import.meta.url), "utf8"));

async function post(payload) {
  for (let i = 1; i <= 4; i++) {
    try {
      const r = await fetch(BASE, {
        method: "POST",
        headers: {
          Authorization: `Bearer ${SECRET}`,
          "Content-Type": "application/json",
        },
        body: JSON.stringify(payload),
        signal: AbortSignal.timeout(30000),
      });
      const text = await r.text();
      if (!r.ok) throw new Error(`HTTP ${r.status}: ${text.slice(0, 200)}`);
      return JSON.parse(text);
    } catch (e) {
      if (i === 4) throw e;
      await new Promise((r) => setTimeout(r, i * 2000));
    }
  }
}

console.log("сидим mutations:", mutations.length);
for (const m of mutations) {
  await post({ op: "add", table: "mutations", name: m.name, multi: m.multi, icon: m.icon, description: m.desc });
}

console.log("сидим traits:", traits.length);
for (const t of traits) {
  await post({ op: "add", table: "traits", name: t.name, multiplier: t.multiplier, icon: t.icon, description: t.desc });
}

console.log("готово");