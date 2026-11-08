// Windows Server poller for the Vercel Discord REST gateway.
// It never connects to Discord directly; only Vercel is contacted over HTTPS.
import "dotenv/config";
import { ProxyAgent, setGlobalDispatcher } from "undici";

if (process.env.HTTPS_PROXY) {
  setGlobalDispatcher(new ProxyAgent(process.env.HTTPS_PROXY));
  console.log("HTTPS proxy enabled for Vercel requests.");
}

const pollUrl = process.env.VERCEL_POLL_URL;
const pollSecret = process.env.DISCORD_POLL_SECRET;
const intervalMs = Math.max(5000, Number(process.env.POLL_INTERVAL_MS || 5000));
const requestTimeoutMs = Math.max(5000, Number(process.env.POLL_REQUEST_TIMEOUT_MS || 15000));

if (!pollUrl) throw new Error("VERCEL_POLL_URL is not set");
if (!pollSecret) throw new Error("DISCORD_POLL_SECRET is not set");

let stopping = false;
process.on("SIGINT", () => { stopping = true; console.log("Stopping..."); });
process.on("SIGTERM", () => { stopping = true; console.log("Stopping..."); });

async function pollOnce() {
  const response = await fetch(pollUrl, {
    method: "POST",
    headers: {
      Authorization: `Bearer ${pollSecret}`,
      "Content-Type": "application/json",
    },
    signal: AbortSignal.timeout(requestTimeoutMs),
  });
  const body = await response.text();
  if (!response.ok) throw new Error(`Vercel HTTP ${response.status}: ${body.slice(0, 300)}`);
  return body;
}

console.log(`Vercel poller started; interval=${intervalMs}ms; target=${pollUrl}`);
while (!stopping) {
  const started = Date.now();
  try {
    const body = await pollOnce();
    console.log(`${new Date().toISOString()} OK ${body.slice(0, 500)}`);
  } catch (error) {
    console.error(`${new Date().toISOString()} ERROR ${error.message}`);
  }
  const wait = Math.max(0, intervalMs - (Date.now() - started));
  if (wait) await new Promise((resolve) => setTimeout(resolve, wait));
}
console.log("Vercel poller stopped.");
