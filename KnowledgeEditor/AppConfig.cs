using System.Text.Json;

namespace KnowledgeEditor;

/// <summary>Application configuration, loaded from <c>config.json</c> next to the executable.</summary>
public sealed class AppConfig
{
    /// <summary>Режим доступа к БД: "api" (через Vercel REST — для работы из РФ без VPN) или "direct" (Npgsql напрямую).</summary>
    public string DbMode { get; set; } = "api";

    /// <summary>База Vercel REST API (db.mjs).</summary>
    public string ApiUrl { get; set; } = "https://dan-games-discord-poller.vercel.app/api/db";
    /// <summary>Тот же секрет, что POLL_SECRET в Vercel / DISCORD_POLL_SECRET в GitHub.</summary>
    public string ApiSecret { get; set; } = "";

    /// <summary>Прямое подключение (только если DbMode = direct и БД доступна, например через VPN).</summary>
    public string ConnectionString { get; set; } = "";
    public string EmbedUrl { get; set; } = "https://api.mistral.ai/v1/embeddings";
    public string EmbedModel { get; set; } = "mistral-embed";
    public string EmbedApiKey { get; set; } = "";

    public static AppConfig Load()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "config.json");
        if (File.Exists(path))
        {
            var json = File.ReadAllText(path);
            var cfg = JsonSerializer.Deserialize<AppConfig>(json);
            if (cfg is not null) return cfg;
        }

        // Fall back to environment variables.
        return new AppConfig
        {
            ApiUrl = Environment.GetEnvironmentVariable("DB_API_URL") ?? "https://dan-games-discord-poller.vercel.app/api/db",
            ApiSecret = Environment.GetEnvironmentVariable("DB_API_SECRET") ?? "",
            ConnectionString = Environment.GetEnvironmentVariable("SUPABASE_DATABASE_URL") ?? "",
            EmbedUrl = Environment.GetEnvironmentVariable("EMBED_URL") ?? "https://api.mistral.ai/v1/embeddings",
            EmbedModel = Environment.GetEnvironmentVariable("EMBED_MODEL") ?? "mistral-embed",
            EmbedApiKey = Environment.GetEnvironmentVariable("MISTRAL_API_KEY") ?? "",
        };
    }

    public void Save()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "config.json");
        File.WriteAllText(path, JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true }));
    }

    public bool IsConfigured =>
        (DbMode == "direct" && !string.IsNullOrWhiteSpace(ConnectionString))
        || (DbMode != "direct" && !string.IsNullOrWhiteSpace(ApiSecret));
}
