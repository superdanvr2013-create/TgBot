-- Словарь оскорблений и ругательств для модерации.
-- Слова можно добавлять/править/удалять вручную (в т.ч. из C#-приложения).
create table if not exists moderation_words (
  id        bigint primary key generated always as identity,
  word      text not null unique,
  note      text,
  added_at  timestamptz not null default now()
);

create index if not exists idx_moderation_words_word on moderation_words (word);
