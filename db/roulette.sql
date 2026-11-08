-- Таблица участников чат-рулетки. Идемпотентно.
create table if not exists roulette_participants (
  id           bigint primary key generated always as identity,
  discord_id   text not null,
  username     text not null,
  display_name text not null,
  joined_at    timestamptz not null default now(),
  unique (discord_id)
);

create index if not exists idx_roulette_joined on roulette_participants (joined_at);