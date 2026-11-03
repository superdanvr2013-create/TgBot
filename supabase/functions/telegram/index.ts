// Telegram bot "Dan Games" — Supabase Edge Function (Deno) на grammY.
//
// Команды: /start, /chat, а также любой текст.
// Ответ на всё: "привет это чат-бот Dan Games".
//
// Деплой: supabase functions deploy telegram --no-verify-jwt --use-api --yes
// (public-функция, т.к. Telegram не присылает Supabase JWT).

import { Bot, webhookCallback } from "npm:grammy@1.46.0";

const TELEGRAM_TOKEN = Deno.env.get("TELEGRAM_BOT_TOKEN");
if (!TELEGRAM_TOKEN) {
  throw new Error("TELEGRAM_BOT_TOKEN secret is not set");
}

const bot = new Bot(TELEGRAM_TOKEN);

const REPLY_TEXT = "привет это чат-бот Dan Games";

bot.catch((err) => {
  const ctx = err.ctx;
  console.error(
    `Ошибка при обработке update_id=${ctx.update.update_id}:`,
    err.error,
  );
});

bot.command("start", (ctx) => ctx.reply(REPLY_TEXT));
bot.command("chat", (ctx) => ctx.reply(REPLY_TEXT));

// Любое другое текстовое сообщение (вопрос в чате) — тот же ответ.
// Команды не трогаем: они уже обработаны выше.
bot.on("message:text", (ctx) => {
  const text = ctx.message?.text ?? "";
  if (text.startsWith("/")) return;
  return ctx.reply(REPLY_TEXT);
});

const handleUpdate = webhookCallback(bot, "std/http");

Deno.serve(async (req) => {
  const url = new URL(req.url);

  // Разовый bootstrap: привязать вебхук Telegram к этой функции.
  if (url.pathname.endsWith("/register")) {
    try {
      const webhookUrl = `${url.origin}/telegram`;
      const ok = await bot.api.setWebhook(webhookUrl, {
        drop_pending_updates: true,
      });
      return new Response(JSON.stringify({ ok, webhookUrl }), {
        headers: { "Content-Type": "application/json" },
      });
    } catch (err) {
      return new Response(JSON.stringify({ ok: false, error: String(err) }), {
        status: 500,
        headers: { "Content-Type": "application/json" },
      });
    }
  }

  // GET — health check.
  if (req.method !== "POST") {
    return new Response("Dan Games Telegram bot is running (grammY)");
  }

  // POST — апдейт от Telegram.
  try {
    return await handleUpdate(req);
  } catch (err) {
    console.error("webhook handler error:", err);
    return new Response("Internal Server Error", { status: 500 });
  }
});