import "dotenv/config";
const token = process.env.DISCORD_TOKEN;
console.log("DISCORD_TOKEN:", token ? `SET (length ${token.length}, prefix ${token.slice(0, 4)}...)` : "MISSING");
console.log("GROQ_API_KEY:", process.env.GROQ_API_KEY ? "SET" : "MISSING");
console.log("DATABASE_URL:", process.env.DATABASE_URL ? "SET" : "MISSING");
if (!token) process.exit(2);
const headers = { Authorization: `Bot ${token}` };
try {
  const me = await fetch("https://discord.com/api/v10/users/@me", { headers, signal: AbortSignal.timeout(15000) });
  console.log("Discord token API:", me.status, me.status === 200 ? (await me.json()).username : (await me.text()).slice(0, 200));
} catch (e) { console.log("Discord token API error:", e.message); }
try {
  const gw = await fetch("https://discord.com/api/v10/gateway/bot", { headers, signal: AbortSignal.timeout(15000) });
  console.log("Gateway bot API:", gw.status, (await gw.text()).slice(0, 200));
} catch (e) { console.log("Gateway API error:", e.message); }
