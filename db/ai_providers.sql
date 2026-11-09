-- Провайдеры ИИ для ботов: мультиключевая конфигурация с приоритетами.
-- Бот использует строку с active=true (приоритетная), при сбое — следующую по sort_order.
create table if not exists ai_providers (
  id          bigint primary key generated always as identity,
  name        text not null,
  base_url    text not null,
  api_key     text not null,
  model       text not null,
  description text,
  active      boolean not null default false,
  sort_order  int not null default 0,
  updated_at  timestamptz not null default now()
);

-- Активной может быть только одна строка (галочка приоритета).
create unique index if not exists idx_ai_providers_single_active
  on ai_providers (active) where active;
