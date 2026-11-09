// Транзит эмбеддингов через Vercel: большой текст одним запросом.
// Vercel (США) → Mistral: без DPI-ограничений РФ.
const SECRET = (await import("node:fs")).readFileSync(process.env.SECRET_FILE, "utf8").trim();
const full = (await import("node:fs")).readFileSync("C:\\Даня\\TelegramBot\\Кусочек текста знаний.txt", "utf8");
const theme = "Брейнрот, Трейд, Brainrot, Trade, W или L, брейн, В или Л";
const input = `${theme}\n${full}`;
console.log("размер:", input.length, "символов");

const t = Date.now();
const res = await fetch("https://dan-games-discord-poller.vercel.app/api/embed", {
  method: "POST",
  headers: {
    Authorization: `Bearer ${SECRET}`,
    "Content-Type": "application/json",
  },
  body: JSON.stringify({ input }),
  signal: AbortSignal.timeout(120000),
});
console.log("HTTP", res.status, `${Date.now() - t}ms`);
const body = await res.text();
console.log(body.slice(0, 300));