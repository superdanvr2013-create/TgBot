// Windows Server poller for the Vercel bots (Discord + Telegram).
// Never connects to Discord/Telegram directly; only Vercel over HTTPS.
// Run with: node poller.mjs
import "dotenv/config";
import { ProxyAgent, setGlobalDispatcher } from "undici";

if (process.env.HTTPS_PROXY) {
  setGlobalDispatcher(new ProxyAgent(process.env.HTTPS_PROXY));
  console.log("HTTPS proxy enabled for Vercel requests.");
}

const DISCORD_URL = process.env.VERCEL_POLL_URL;
const TELEGRAM_URL = process.env.VERCEL_TELEGRAM_URL;
const pollSecret = process.env.DISCORD_POLL_SECRET;
const intervalMs = Math.max(5000, Number(process.env.POLL_INTERVAL_MS || 5000));
const requestTimeoutMs = Math.max(5000, Number(process.env.POLL_REQUEST_TIMEOUT_MS || 15000));

if (!DISCORD_URL) throw new Error("VERCEL_POLL_URL is not set");
if (!pollSecret) throw new Error("DISCORD_POLL_SECRET is not set");
if (!TELEGRAM_URL) console.warn("VERCEL_TELEGRAM_URL not set: Telegram polling disabled.");

let stopping = false;
process.on("SIGINT", () => { stopping = true; console.log("Stopping..."); });
process.on("SIGTERM", () => { stopping = true; console.log("Stopping..."); });

async function pollOnce(label, url) {
  const response = await fetch(url, {
    method: "POST",
    headers: {
      Authorization: `Bearer ${pollSecret}`,
      "Content-Type": "application/json",
    },
    signal: AbortSignal.timeout(requestTimeoutMs),
  });
  const body = await response.text();
  if (!response.ok) throw new Error(`Vercel[${label}] HTTP ${response.status}: ${body.slice(0, 300)}`);
  return body;
}

async function pollAll() {
  const targets = [["discord", DISCORD_URL]];
  if (TELEGRAM_URL) targets.push(["telegram", TELEGRAM_URL]);
  for (const [label, url] of targets) {
    try {
      const body = await pollOnce(label, url);
      console.log(`${new Date().toISOString()} [${label}] OK ${body.slice(0, 300)}`);
    } catch (error) {
      console.error(`${new Date().toISOString()} [${label}] ERROR ${error.message}`);
    }
  }
}

console.log(`Vercel poller started; interval=${intervalMs}ms; targets=${TELEGRAM_URL ? "discord+telegram" : "discord"}`);
while (!stopping) {
  const started = Date.now();
  await pollAll();
  const wait = Math.max(0, intervalMs - (Date.now() - started));
  if (wait) await new Promise((resolve) => setTimeout(resolve, wait));
}
console.log("Vercel poller stopped.");
