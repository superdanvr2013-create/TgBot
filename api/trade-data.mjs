// Калькулятор трейда брейнротов (статическая HTML-страница на Vercel).
// Слева — мои брейнроты (зелёные на скрине), справа — их (на которые меняюсь).
// Формула: basePrice × mutation.multi × traits.multiplier(сумма выбранных).

const POLL_SECRET = process.env.POLL_SECRET;
const DB_API = process.env.DB_API_URL || "https://dan-games-discord-poller.vercel.app/api/db";

function auth(req) {
  const bearer = req.headers.authorization?.match(/^Bearer\s+(.+)$/i)?.[1];
  return Boolean(POLL_SECRET && bearer === POLL_SECRET);
}

function json(res, status, body) {
  res.status(status).json(body);
}

export default async function handler(req, res) {
  if (!auth(req)) return json(res, 401, { error: "Unauthorized" });

  const { method, query } = req;

  if (method === "GET") {
    // ?left=Name1|Name2&right=Name3
    const left = (query?.left || "").split("|").filter(Boolean);
    const right = (query?.right || "").split("|").filter(Boolean);

    const dbKey = process.env.POLL_SECRET;
    let knownNames = [];
    let basePrices = {};
    try {
      const r = await fetch(`${DB_API}?table=knowledge`, {
        headers: { Authorization: `Bearer ${dbKey}` },
        signal: AbortSignal.timeout(15000),
      });
      const { rows } = await r.json();
      for (const row of rows) {
        // 1) имена из темы
        const candidates = row.theme
          .split(/[,;]|\s+и\s+|\s+or\s+/i)
          .map((s) => s.trim())
          .filter((s) => s.length > 2);
        
        // 2) имена из описания: строки «N. Name — income: $X/s»
        const descNames = [];
        const descLines = row.description.split(/\r?\n/);
        for (const line of descLines) {
          const m = line.match(/^\s*\d+[.)]\s*(.+?)\s*[—–-]\s*income:\s*\$?([\d.]+)\s*([KMB])/i);
          if (m) {
            const name = m[1].trim();
            let price = parseFloat(m[2]);
            const unit = m[3].toUpperCase();
            if (unit === "M") price *= 1e6;
            else if (unit === "K") price *= 1e3;
            else if (unit === "B") price *= 1e9;
            descNames.push(name);
            basePrices[name.toLowerCase()] = price;
          }
        }
        
        const allNames = [...candidates, ...descNames];
        for (const name of allNames) {
          knownNames.push(name);
        }
      }
    } catch {
      // база недоступна — отдаём пустые списки
    }

    return json(res, 200, {
      ok: true,
      left,
      right,
      knownNames,
      basePrices,
    });
  }

  return json(res, 405, { error: "Method not allowed" });
}
