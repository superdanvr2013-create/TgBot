using Npgsql;

namespace KnowledgeEditor;

/// <summary>CRUD для таблицы moderation_words (словарь оскорблений).</summary>
public sealed class WordsRepository
{
    private readonly string _connectionString;

    public WordsRepository(string connectionString)
    {
        _connectionString = Normalize(connectionString);
    }

    private static string Normalize(string cs)
    {
        if (string.IsNullOrWhiteSpace(cs)) return cs;
        var s = cs.Trim();
        if (s.StartsWith("postgresql://", StringComparison.OrdinalIgnoreCase) ||
            s.StartsWith("postgres://", StringComparison.OrdinalIgnoreCase))
        {
            var uri = new Uri(s);
            var b = new NpgsqlConnectionStringBuilder
            {
                Host = uri.Host,
                Port = uri.Port > 0 ? uri.Port : 5432,
                Username = uri.UserInfo.Contains(':') ? uri.UserInfo.Split(':')[0] : uri.UserInfo,
                Password = uri.UserInfo.Contains(':') ? uri.UserInfo.Split(':', 2)[1] : "",
                SslMode = SslMode.Require,
                Timeout = 60,
                CommandTimeout = 60,
                CancellationTimeout = 30,
                MaxPoolSize = 4,
                ConnectionIdleLifetime = 60,
            };
            var db = uri.AbsolutePath.TrimStart('/');
            if (!string.IsNullOrEmpty(db)) b.Database = db;
            return b.ConnectionString;
        }
        return s;
    }

    private NpgsqlConnection Open() => new(_connectionString);

    /// <summary>Открывает соединение с повторами: транзитные обрывы сети к Supabase не редкость.</summary>
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

    public async Task<IReadOnlyList<WordRow>> ListAsync(string? filter = null)
    {
        await using var conn = await OpenWithRetryAsync();
        const string baseSql = """
            SELECT id, word, note, added_at
              FROM moderation_words
             WHERE (@filter::text IS NULL OR word ILIKE '%' || @filter::text || '%')
             ORDER BY word
            """;
        await using var cmd = new NpgsqlCommand(baseSql, conn);
        cmd.Parameters.AddWithValue("filter", (object?)filter ?? DBNull.Value);
        var rows = new List<WordRow>();
        await using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            rows.Add(new WordRow
            {
                Id = reader.GetInt64(0),
                Word = reader.GetString(1),
                Note = reader.IsDBNull(2) ? "" : reader.GetString(2),
                AddedAt = reader.GetDateTime(3),
            });
        }
        return rows;
    }

    public async Task<long> AddAsync(string word, string note)
    {
        await using var conn = await OpenWithRetryAsync();
        const string sql = """
            INSERT INTO moderation_words (word, note)
            VALUES (@word, @note)
            ON CONFLICT (word) DO UPDATE SET note = EXCLUDED.note
            RETURNING id;
            """;
        await using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("word", word);
        cmd.Parameters.AddWithValue("note", string.IsNullOrWhiteSpace(note) ? DBNull.Value : note);
        var id = await cmd.ExecuteScalarAsync();
        return (long)id!;
    }

    public async Task UpdateAsync(long id, string word, string note)
    {
        await using var conn = await OpenWithRetryAsync();
        const string sql = """
            UPDATE moderation_words
               SET word = @word, note = @note
             WHERE id = @id;
            """;
        await using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("id", id);
        cmd.Parameters.AddWithValue("word", word);
        cmd.Parameters.AddWithValue("note", string.IsNullOrWhiteSpace(note) ? DBNull.Value : note);
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task DeleteAsync(long id)
    {
        await using var conn = await OpenWithRetryAsync();
        await using var cmd = new NpgsqlCommand("DELETE FROM moderation_words WHERE id = @id;", conn);
        cmd.Parameters.AddWithValue("id", id);
        await cmd.ExecuteNonQueryAsync();
    }
}

public sealed class WordRow
{
    public long Id { get; set; }
    public string Word { get; set; } = "";
    public string Note { get; set; } = "";
    public DateTime AddedAt { get; set; }
}
