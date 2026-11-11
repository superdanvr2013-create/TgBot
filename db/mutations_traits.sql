-- Справочники мутаций и трейтов (редактируемые через KnowledgeEditor).
create table if not exists mutations (
  id         bigint primary key generated always as identity,
  name       text not null unique,
  multi      numeric not null default 1,
  icon       text,          -- ссылка https на иконку
  description text,         -- описание
  updated_at timestamptz not null default now()
);

create table if not exists traits (
  id         bigint primary key generated always as identity,
  name       text not null unique,
  multiplier numeric not null default 1,
  icon       text,
  description text,
  updated_at timestamptz not null default now()
);

create index if not exists idx_mutations_name on mutations (name);
create index if not exists idx_traits_name on traits (name);
