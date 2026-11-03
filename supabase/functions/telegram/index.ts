// Telegram bot "Dan Games" — Supabase Edge Function (Deno)
//
// Commands handled: /start, /chat, and any message.
// Reply to anything from the chat: "привет это чат-бот Dan Games".
//
// Deploy this as an Edge Function named "telegram".
// IMPORTANT: the function must be PUBLIC (JWT verification OFF), because
// Telegram cannot send a Supabase JWT.
//
// One-time setup: open  https://<project-ref>.functions.supabase.co/telegram/register
// in a browser (or invoke it once) to register the webhook with Telegram.
// Supabase's servers can reach api.telegram.org even though your local PC cannot.

import { serve } from "https://deno.land/std@0.168.0/http/server.ts";

const TELEGRAM_TOKEN = Deno.env.get("TELEGRAM_BOT_TOKEN") ?? "";
const TELEGRAM_API = `https://api.telegram.org/bot${TELEGRAM_TOKEN}`;

const REPLY_TEXT = "привет это чат-бот Dan Games";

async function handler(req: Request): Promise<Response> {
  const url = new URL(req.url);

  // --- bootstrap: point Telegram's webhook at THIS function ---
  if (url.pathname.endsWith("/register") || url.pathname.endsWith("/register/")) {
    if (!TELEGRAM_TOKEN) {
      return new Response(
        JSON.stringify({ ok: false, description: "TELEGRAM_BOT_TOKEN secret is not set" }),
        { headers: { "Content-Type": "application/json" } },
      );
    }
    const webhookUrl = `${url.origin}/telegram`;
    const res = await fetch(`${TELEGRAM_API}/setWebhook`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ url: webhookUrl, drop_pending_updates: true }),
    });
    const body = await res.text();
    return new Response(body, {
      status: res.status,
      headers: { "Content-Type": "application/json" },
    });
  }

  // --- Telegram webhook (POST) ---
  if (req.method === "POST") {
    const update = await req.json().catch(() => null);
    const chatId: number | undefined = update?.message?.chat?.id;

    if (chatId) {
      await fetch(`${TELEGRAM_API}/sendMessage`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ chat_id: chatId, text: REPLY_TEXT }),
      }).catch(() => null);
    }

    // Always 200 so Telegram does not retry.
    return new Response("OK");
  }

  // --- GET health check ---
  return new Response("Dan Games Telegram bot is running");
}

serve(handler);