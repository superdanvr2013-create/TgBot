using Npgsql;

namespace KnowledgeEditor;

/// <summary>Persistence layer: insert/update/delete/list knowledge rows in Supabase (pgvector).</summary>
public sealed class KnowledgeRepository
{
    private readonly string _connectionString;

    public KnowledgeRepository(string connectionString)
    {
        _connectionString = Normalize(connectionString);
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
                Timeout = 30,
                CommandTimeout = 60,
            };
            if (!string.IsNullOrEmpty(db)) b.Database = db;
            return b.ConnectionString;
        }
        return s;
    }

    private NpgsqlConnection Open() => new(_connectionString);

    public async Task<long> UpsertAsync(string theme, string description, float[] embedding)
    {
        // Insert with embedding; on theme conflict, update description + embedding.
        await using var conn = Open();
        await conn.OpenAsync();

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
        // pgvector: cast text -> vector explicitly (deterministic on both pooler modes)
        cmd.Parameters.AddWithValue("embedding", "[" + string.Join(",", embedding.Select(x => x.ToString(System.Globalization.CultureInfo.InvariantCulture))) + "]");
        var id = await cmd.ExecuteScalarAsync();
        return (long)id!;
    }

    public async Task<long> UpdateAsync(long id, string theme, string description, float[] embedding)
    {
        // Update an existing row by its id (theme can be changed too).
        await using var conn = Open();
        await conn.OpenAsync();

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
        cmd.Parameters.AddWithValue("embedding", "[" + string.Join(",", embedding.Select(x => x.ToString(System.Globalization.CultureInfo.InvariantCulture))) + "]");
        var res = await cmd.ExecuteScalarAsync();
        if (res is null) throw new InvalidOperationException($"Запись с id={id} не найдена.");
        return (long)res;
    }

    public async Task<IReadOnlyList<KnowledgeRow>> ListAsync(int limit = 200)
    {
        await using var conn = Open();
        await conn.OpenAsync();
        const string sql = """
            SELECT id, theme, description, created_at, updated_at, embedding IS NOT NULL AS has_embedding
              FROM knowledge
             ORDER BY updated_at DESC
             LIMIT @limit;
            """;
        await using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("limit", limit);
        var rows = new List<KnowledgeRow>();
        await using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            rows.Add(new KnowledgeRow
            {
                Id = reader.GetInt64(0),
                Theme = reader.GetString(1),
                Description = reader.GetString(2),
                CreatedAt = reader.GetDateTime(3),
                UpdatedAt = reader.GetDateTime(4),
                HasEmbedding = reader.GetBoolean(5),
            });
        }
        return rows;
    }

    public async Task DeleteAsync(long id)
    {
        await using var conn = Open();
        await conn.OpenAsync();
        const string sql = "DELETE FROM knowledge WHERE id = @id;";
        await using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("id", id);
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task<bool> HealthAsync()
    {
        try
        {
            await using var conn = Open();
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand("SELECT 1;", conn);
            await cmd.ExecuteScalarAsync();
            return true;
        }
        catch
        {
            return false;
        }
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