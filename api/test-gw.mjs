const body = {
  model: "deepseek-v4.1-flash-1017",
  messages: [{ role: "user", content: "привет, ответь одним словом" }],
  max_tokens: 20,
};
const r = await fetch("https://177-3-220-29.sslip.io/v1/chat/completions", {
  method: "POST",
  headers: {
    Authorization: "Bearer gw_sk_uOJYhGLXQvrxgwBFt9ui83uXhEdGppwWwEUJ5GokD40",
    "Content-Type": "application/json",
  },
  body: JSON.stringify(body),
  signal: AbortSignal.timeout(60000),
});
console.log("HTTP", r.status);
const text = await r.text();
console.log(text.slice(0, 500));