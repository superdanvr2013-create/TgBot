-- Knowledge base for Dan Games discord bot (RAG)
-- Run against the DanGamesProjects Supabase Postgres database.
-- Embeddings: mistral-embed (1024 dims) via Mistral AI.

-- 1. pgvector extension
create extension if not exists vector;

-- 2. Knowledge table: theme (unique) + description + embedding + metadata
create table if not exists knowledge (
  id bigint generated always as identity primary key,
  theme text not null unique,             -- unique so the C# app can upsert ON CONFLICT (theme)
  description text not null,
  embedding vector(1024),                 -- mistral-embed = 1024 dims
  created_at timestamptz not null default now(),
  updated_at timestamptz not null default now()
);

-- 3. HNSW index for cosine similarity search (matches the <=> operator used in rag.mjs)
create index if not exists knowledge_embedding_idx on knowledge
  using hnsw (embedding vector_cosine_ops);

-- 4. trigger to bump updated_at
create or replace function set_updated_at() returns trigger as $$
begin
  new.updated_at = now();
  return new;
end;
$$ language plpgsql;

drop trigger if exists knowledge_updated_at on knowledge;
create trigger knowledge_updated_at
  before update on knowledge
  for each row execute function set_updated_at();