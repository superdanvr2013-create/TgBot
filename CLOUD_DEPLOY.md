# Облачный Discord-бот: Vercel + GitHub Actions

## Архитектура

`GitHub Actions (каждые 15 секунд) -> Vercel /api/discord-poll -> Discord REST API + Supabase + Groq/Mistral`

Это REST-режим, а не Gateway. Бот не будет показываться как `online`, не получает реакции в реальном времени и может отвечать с задержкой. Gateway-версия остаётся в `discord/bot.mjs` для запуска на VPS/Windows Server.

## Vercel environment variables

Добавить в Production и Preview при необходимости:

- `DISCORD_TOKEN`
- `DISCORD_CLIENT_ID=1567282235680358400`
- `DISCORD_CHANNEL_IDS=1512198085319589898`
- `GROQ_API_KEY`
- `MISTRAL_API_KEY`
- `DATABASE_URL` (Session Pooler, порт 5432)
- `POLL_SECRET` (случайная длинная строка)
- `OWNER_USERNAME=danvr2013`

## GitHub repository secrets

В GitHub → Settings → Secrets and variables → Actions добавить:

- `VERCEL_POLL_URL=https://<project>.vercel.app/api/discord-poll`
- `DISCORD_POLL_SECRET` — то же значение, что `POLL_SECRET` в Vercel

## Включение

1. Импортировать репозиторий в Vercel и выбрать корень проекта `TgBot`.
2. Deploy.
3. Проверить `GET /api/discord-poll` с заголовком `Authorization: Bearer <POLL_SECRET>`.
4. Включить workflow `.github/workflows/discord-poll.yml` через Actions → Discord REST poller → Run workflow.
5. Первый запуск только запоминает последнее сообщение канала; старую историю бот не обрабатывает.
6. Следующие циклы читают новые сообщения и отвечают через REST.

## Ограничения

- GitHub schedule может запускаться с задержкой. После старта workflow цикл держится до 55 минут и вызывает Vercel примерно каждые 15 секунд.
- Vercel/GitHub Actions не поддерживают текущий Discord Gateway.
- Реакции и псевдоанимация рулетки в REST-версии отключены; доступны ручные команды добавления/удаления участников и подсчёт.
- В Discord REST polling бот не имеет `online` presence.
