using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace KnowledgeEditor;

/// <summary>
/// REST-клиент к Vercel /api/db: CRUD для knowledge и moderation_words.
/// Используется, когда прямое подключение к Supabase из РФ недоступно.
/// </summary>
public sealed class DbApiClient
{
    private readonly HttpClient _http;
    private readonly string _baseUrl;

    public DbApiClient(string baseUrl, string secret)
    {
        _baseUrl = baseUrl.TrimEnd('/');
        _http = new HttpClient(new SocketsHttpHandler
        {
            // новые соединения на каждый ретрай: оборванное TLS-соединение не кэшируется
            PooledConnectionLifetime = TimeSpan.FromMinutes(2),
            ConnectTimeout = TimeSpan.FromSeconds(20),
        })
        { Timeout = TimeSpan.FromSeconds(60) };
        _http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", secret);
    }

    // node-postgres возвращает bigint как строки ("79") — разрешаем чтение чисел из строк
    private static readonly JsonSerializerOptions JsonOpts = new()
    {
        NumberHandling = JsonNumberHandling.AllowReadingFromString,
    };

    private static async Task<T> ReadAsync<T>(HttpResponseMessage resp) where T : class
    {
        var body = await resp.Content.ReadAsStringAsync();
        if (!resp.IsSuccessStatusCode)
        {
            try
            {
                var doc = JsonDocument.Parse(body);
                if (doc.RootElement.TryGetProperty("error", out var errEl))
                    throw new Exception($"HTTP {(int)resp.StatusCode}: {errEl.GetString()}");
            }
            catch (JsonException) { }
            throw new Exception($"HTTP {(int)resp.StatusCode}: {body}");
        }
        var result = JsonSerializer.Deserialize<T>(body, JsonOpts);
        return result ?? throw new Exception("Пустой ответ API");
    }

    // ── GET ────────────────────────────────────────────────────────────────

    public async Task<List<T>> ListAsync<T>(string table, string? filter = null) where T : class
    {
        var url = $"{_baseUrl}?table={Uri.EscapeDataString(table)}";
        if (!string.IsNullOrWhiteSpace(filter)) url += $"&filter={Uri.EscapeDataString(filter)}";
        return await SendWithRetryAsync(async () =>
        {
            using var resp = await _http.GetAsync(url);
            var result = await ReadAsync<RowsResponse<T>>(resp);
            return result.Rows ?? new();
        });
    }

    // ── POST ───────────────────────────────────────────────────────────────

    public async Task<long> PostAsync(object command)
    {
        return await SendWithRetryAsync(async () =>
        {
            using var resp = await _http.PostAsJsonAsync(_baseUrl, command);
            var result = await ReadAsync<WriteResponse>(resp);
            return result.Id;
        });
    }

    /// <summary>
    /// Транзитные TLS-обрывы (DPI) к Vercel случаются регулярно: повторяем запрос
    /// до 4 раз с паузами. Идемпотентные GET безопасны; POST-операции БД
    /// тоже повторяем — upsert/add идемпотентны по слову/теме, delete безопасен.
    /// </summary>
    private static async Task<T> SendWithRetryAsync<T>(Func<Task<T>> send, int attempts = 4)
    {
        Exception? last = null;
        for (var i = 1; i <= attempts; i++)
        {
            try
            {
                return await send();
            }
            catch (Exception ex) when (ex is HttpRequestException or IOException or TaskCanceledException)
            {
                last = ex;
                if (i < attempts) await Task.Delay(i * 1500);
            }
        }
        throw last!;
    }

    public async Task<long> AddWordAsync(string word, string note)
        => await PostAsync(new { op = "add", table = "words", word, note });

    public async Task<long> UpdateWordAsync(long id, string word, string note)
        => await PostAsync(new { op = "update", table = "words", id, word, note });

    public async Task DeleteWordAsync(long id)
        => await PostAsync(new { op = "delete", table = "words", id });

    public async Task<long> UpsertKnowledgeAsync(string theme, string description, string embeddingJson)
        => await PostAsync(new { op = "upsert", table = "knowledge", theme, description, embedding = embeddingJson });

    public async Task<long> UpdateKnowledgeAsync(long id, string theme, string description, string embeddingJson)
        => await PostAsync(new { op = "update", table = "knowledge", id, theme, description, embedding = embeddingJson });

    public async Task DeleteKnowledgeAsync(long id)
        => await PostAsync(new { op = "delete", table = "knowledge", id });

    private sealed record RowsResponse<T>([property: JsonPropertyName("rows")] List<T>? Rows);
    private sealed record WriteResponse([property: JsonPropertyName("id")] long Id);
}
