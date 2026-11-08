# Telegram-бот "Dan Games"

Бот @DanGames2013_bot на Supabase Edge Functions (Deno).

## Команды
- `/start` — приветствие
- `/chat` — начать чат
- любое сообщение — бот отвечает: **привет это чат-бот Dan Games**

## Структура
```
TgBot/
├── supabase/
│   └── functions/
│       └── telegram/
│           └── index.ts    # обработчик вебхука Telegram (Edge Function)
└── README.md
```

## Как это работает
1. Supabase Edge Function `telegram` принимает POST-апдейты от Telegram Bot API.
2. На любой апдейт (включая `/start` и `/chat`) бот отвечает `привет это чат-бот Dan Games`.
3. Вебхук зарегистрирован через `/register` (или вручную через `setWebhook`).
4. Токен бота хранится в секрете `TELEGRAM_BOT_TOKEN` и не попадает в репозиторий.

## Деплой (Supabase CLI)
```
supabase login
supabase link --project-ref mozbapnovlngcyishjxw
supabase secrets set TELEGRAM_BOT_TOKEN=<telegram_bot_token>
supabase functions deploy telegram --no-verify-jwt
```

## Регистрация вебхука
```
curl https://mozbapnovlngcyishjxw.functions.supabase.co/telegram/register
```

## Примечания
- Функция публичная (`verify_jwt: false`), т.к. Telegram не присылает Supabase JWT.
- Токен бота не коммитится.