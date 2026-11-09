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
        _http = new HttpClient { Timeout = TimeSpan.FromSeconds(60) };
        _http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", secret);
    }

    private static async Task<T> ReadAsync<T>(HttpResponseMessage resp) where T : class
    {
        var body = await resp.Content.ReadAsStringAsync();
        if (!resp.IsSuccessStatusCode)
        {
            // error: {"error":"..."} или {"ok":false,"error":"..."}
            try
            {
                var doc = System.Text.Json.JsonDocument.Parse(body);
                if (doc.RootElement.TryGetProperty("error", out var errEl))
                    throw new Exception($"HTTP {(int)resp.StatusCode}: {errEl.GetString()}");
            }
            catch (JsonException) { }
            throw new Exception($"HTTP {(int)resp.StatusCode}: {body}");
        }
        var result = System.Text.Json.JsonSerializer.Deserialize<T>(body);
        return result ?? throw new Exception("Пустой ответ API");
    }

    // ── GET ────────────────────────────────────────────────────────────────

    public async Task<List<T>> ListAsync<T>(string table, string? filter = null) where T : class
    {
        var url = $"{_baseUrl}?table={Uri.EscapeDataString(table)}";
        if (!string.IsNullOrWhiteSpace(filter)) url += $"&filter={Uri.EscapeDataString(filter)}";
        using var resp = await _http.GetAsync(url);
        var result = await ReadAsync<RowsResponse<T>>(resp);
        return result.Rows ?? new();
    }

    // ── POST ───────────────────────────────────────────────────────────────

    public async Task<long> PostAsync(object command)
    {
        using var resp = await _http.PostAsJsonAsync(_baseUrl, command);
        var result = await ReadAsync<WriteResponse>(resp);
        return result.Id;
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
