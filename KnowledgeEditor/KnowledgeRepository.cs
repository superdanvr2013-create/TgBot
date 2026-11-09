using System.Text.Json.Serialization;
using Npgsql;

namespace KnowledgeEditor;

/// <summary>
/// Persistence layer для knowledge (pgvector). Два режима:
/// - "api": REST через Vercel /api/db (работает из РФ без VPN);
/// - "direct": Npgsql напрямую (требует доступности Supabase, например через VPN).
/// </summary>
public sealed class KnowledgeRepository
{
    private readonly string _connectionString;
    private readonly DbApiClient? _api;

    public KnowledgeRepository(AppConfig config)
    {
        if (config.DbMode == "direct")
        {
            _connectionString = Normalize(config.ConnectionString);
        }
        else
        {
            _api = new DbApiClient(config.ApiUrl, config.ApiSecret);
            _connectionString = "";
        }
    }

    /// <summary>
    /// Npgsql принимает строку только в формате "ключ=значение". Если передали
    /// URI вида postgresql://user:pass@host:port/db — конвертируем в key=value
    /// и добавляем обязательный для Supabase pooler SSL.
    /// </summary>
    private static string Normalize(string cs)
    {
        if (string.IsNullOrWhiteSpace(cs)) return cs;
        var s = cs.Trim();

        if (s.StartsWith("postgresql://", StringComparison.OrdinalIgnoreCase) ||
            s.StartsWith("postgres://", StringComparison.OrdinalIgnoreCase))
        {
            var uri = new Uri(s);
            var host = uri.Host;
            var port = uri.Port > 0 ? uri.Port : 5432;
            var db = uri.AbsolutePath.TrimStart('/');

            var user = "";
            var pass = "";
            if (!string.IsNullOrEmpty(uri.UserInfo))
            {
                var parts = uri.UserInfo.Split(':', 2);
                user = Uri.UnescapeDataString(parts[0]);
                if (parts.Length > 1) pass = Uri.UnescapeDataString(parts[1]);
            }

            var b = new NpgsqlConnectionStringBuilder
            {
                Host = host,
                Port = port,
                Username = user,
                Password = pass,
                SslMode = SslMode.Require,
                Timeout = 60,
                CommandTimeout = 60,
                CancellationTimeout = 30,
                MaxPoolSize = 4,
                ConnectionIdleLifetime = 60,
            };
            if (!string.IsNullOrEmpty(db)) b.Database = db;
            return b.ConnectionString;
        }
        return s;
    }

    // ── API-режим: DTO под JSON Vercel ───────────────────────────────────────

    private sealed class ApiRow
    {
        [JsonPropertyName("id")] public long Id { get; set; }
        [JsonPropertyName("theme")] public string Theme { get; set; } = "";
        [JsonPropertyName("description")] public string Description { get; set; } = "";
        [JsonPropertyName("created_at")] public DateTime CreatedAt { get; set; }
        [JsonPropertyName("updated_at")] public DateTime UpdatedAt { get; set; }
        [JsonPropertyName("has_embedding")] public bool HasEmbedding { get; set; }
    }

    private static KnowledgeRow ToRow(ApiRow r) => new()
    {
        Id = r.Id,
        Theme = r.Theme,
        Description = r.Description,
        CreatedAt = DateTime.Parse(r.CreatedAt.ToString("o")).ToUniversalTime().ToLocalTime(),
        UpdatedAt = DateTime.Parse(r.UpdatedAt.ToString("o")).ToUniversalTime().ToLocalTime(),
        HasEmbedding = r.HasEmbedding,
    };

    // ── Публичные операции ────────────────────────────────────────────────────

    public async Task<long> UpsertAsync(string theme, string description, float[] embedding)
    {
        if (_api is not null)
        {
            return await _api.UpsertKnowledgeAsync(theme, description, ToVectorJson(embedding));
        }

        await using var conn = await OpenWithRetryAsync();
        const string sql = """
            INSERT INTO knowledge (theme, description, embedding)
            VALUES (@theme, @description, @embedding::vector)
            ON CONFLICT (theme) DO UPDATE
              SET description = EXCLUDED.description,
                  embedding    = EXCLUDED.embedding,
                  updated_at   = now()
            RETURNING id;
            """;
        await using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("theme", theme);
        cmd.Parameters.AddWithValue("description", description);
        cmd.Parameters.AddWithValue("embedding", ToVectorJson(embedding));
        var id = await cmd.ExecuteScalarAsync();
        return (long)id!;
    }

    public async Task<long> UpdateAsync(long id, string theme, string description, float[] embedding)
    {
        if (_api is not null)
        {
            return await _api.UpdateKnowledgeAsync(id, theme, description, ToVectorJson(embedding));
        }

        await using var conn = await OpenWithRetryAsync();
        const string sql = """
            UPDATE knowledge
               SET theme       = @theme,
                   description = @description,
                   embedding   = @embedding::vector,
                   updated_at  = now()
             WHERE id = @id
            RETURNING id;
            """;
        await using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("id", id);
        cmd.Parameters.AddWithValue("theme", theme);
        cmd.Parameters.AddWithValue("description", description);
        cmd.Parameters.AddWithValue("embedding", ToVectorJson(embedding));
        var res = await cmd.ExecuteScalarAsync();
        if (res is null) throw new InvalidOperationException($"Запись с id={id} не найдена.");
        return (long)res;
    }

    public async Task<IReadOnlyList<KnowledgeRow>> ListAsync(int limit = 200)
    {
        if (_api is not null)
        {
            var rows = await _api.ListAsync<ApiRow>("knowledge");
            return rows.Select(ToRow).ToList();
        }

        await using var conn = await OpenWithRetryAsync();
        const string sql = """
            SELECT id, theme, description, created_at, updated_at, embedding IS NOT NULL AS has_embedding
              FROM knowledge
             ORDER BY updated_at DESC
             LIMIT @limit;
            """;
        await using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("limit", limit);
        var rowsOut = new List<KnowledgeRow>();
        await using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            rowsOut.Add(new KnowledgeRow
            {
                Id = reader.GetInt64(0),
                Theme = reader.GetString(1),
                Description = reader.GetString(2),
                CreatedAt = reader.GetDateTime(3),
                UpdatedAt = reader.GetDateTime(4),
                HasEmbedding = reader.GetBoolean(5),
            });
        }
        return rowsOut;
    }

    public async Task DeleteAsync(long id)
    {
        if (_api is not null)
        {
            await _api.DeleteKnowledgeAsync(id);
            return;
        }

        await using var conn = await OpenWithRetryAsync();
        await using var cmd = new NpgsqlCommand("DELETE FROM knowledge WHERE id = @id;", conn);
        cmd.Parameters.AddWithValue("id", id);
        await cmd.ExecuteNonQueryAsync();
    }

    /// <summary>Вектор в текстовом JSON: [0.12,0.34,...] — формат pgvector.</summary>
    private static string ToVectorJson(float[] embedding)
        => "[" + string.Join(",", embedding.Select(x => x.ToString(System.Globalization.CultureInfo.InvariantCulture))) + "]";

    // ── Direct-режим: соединения с retry ──────────────────────────────────────

    private NpgsqlConnection Open() => new(_connectionString);

    private async Task<NpgsqlConnection> OpenWithRetryAsync(int attempts = 3)
    {
        Exception? last = null;
        for (var i = 1; i <= attempts; i++)
        {
            var conn = new NpgsqlConnection(_connectionString);
            try
            {
                await conn.OpenAsync();
                return conn;
            }
            catch (Exception ex)
            {
                last = ex;
                conn.Dispose();
                if (i < attempts) await Task.Delay(i * 2000);
            }
        }
        throw last!;
    }
}

public sealed class KnowledgeRow
{
    public long Id { get; set; }
    public string Theme { get; set; } = "";
    public string Description { get; set; } = "";
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool HasEmbedding { get; set; }
}
