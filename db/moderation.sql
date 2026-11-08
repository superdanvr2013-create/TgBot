-- Модерация: счётчики сообщений, оскорблений и активные баны.
-- Счётчики в БД, а не в памяти, чтобы переживать перезапуски Vercel-функции.

create table if not exists moderation_events (
  id          bigint primary key generated always as identity,
  discord_id  text not null,
  channel_id  text not null,
  kind        text not null check (kind in ('message', 'abuse')),
  created_at  timestamptz not null default now()
);

create index if not exists idx_moderation_events_lookup
  on moderation_events (discord_id, kind, created_at desc);

create table if not exists moderation_bans (
  discord_id   text primary key,
  strikes      int not null default 0,
  banned_until timestamptz,
  updated_at   timestamptz not null default now()
);
