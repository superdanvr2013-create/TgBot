-- Состояние REST-опроса Discord.
-- Нужна для Vercel/GitHub Actions версии бота, где нет Gateway-соединения.
create table if not exists discord_poll_state (
  channel_id      text primary key,
  last_message_id text,
  updated_at      timestamptz not null default now()
);

create index if not exists idx_discord_poll_state_updated
  on discord_poll_state (updated_at);
