# Dan Games Knowledge Base — RAG architecture

## Components
1. **Postgres + pgvector** (Supabase, project "DanGamesProjects")
   - table `knowledge(id, theme, description, embedding vector(1024), created_at, updated_at)`
   - HNSW index (cosine) for similarity search
2. **Embedding model** — free cloud: `mistral-embed` (Mistral AI, 1024 dims, multilingual incl. Russian)
   - endpoint https://api.mistral.ai/v1/embeddings (OpenAI-compatible)
3. **Discord bot** (`TgBot/discord/bot.mjs`) — on relevant message:
   - embed user query (Mistral) → query Supabase for top-k knowledge → inject as context → Groq answer
4. **C# WinForms app** (`TgBot/KnowledgeEditor/`) — insert/edit knowledge rows
   - theme + description → embed (Mistral) → upsert into Supabase

## Environment variables
- `SUPABASE_DATABASE_URL` — postgresql:// connection string
- `MISTRAL_API_KEY` — Mistral AI API key (free tier)
- `GROQ_API_KEY`, `DISCORD_TOKEN` — as before