import "dotenv/config";
const response = await fetch(process.env.VERCEL_POLL_URL, {
  headers: { Authorization: `Bearer ${process.env.DISCORD_POLL_SECRET}` },
  signal: AbortSignal.timeout(15000),
});
console.log("HTTP", response.status, await response.text());
process.exit(response.ok ? 0 : 1);
