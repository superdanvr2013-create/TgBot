-- Персональные обращения бота к участникам (вариант 2: выбирает ИИ).
create table if not exists individual (
  id          bigint primary key generated always as identity,
  nickname    text not null unique,
  description text not null,
  updated_at  timestamptz not null default now()
);
