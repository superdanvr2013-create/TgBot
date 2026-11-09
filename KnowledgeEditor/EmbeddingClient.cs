using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace KnowledgeEditor;

/// <summary>
/// Эмбеддинги через Vercel-прокси (/api/embed): РФ → Vercel → Mistral.
/// Прокси сам режет текст на чанки (лимит Mistral 8192 токенов) и усредняет вектор,
/// поэтому текст любого размера уходит одним запросом. Прямое подключение из РФ
/// к Mistral рвётся DPI при payload > ~12 КБ — транзит решает это.
/// </summary>
public sealed class EmbeddingClient
{
    private readonly HttpClient _http;
    private readonly string _baseUrl;

    public EmbeddingClient(string baseUrl, string apiKey, string model)
    {
        // baseUrl может быть: полный Vercel /api/embed ИЛИ старый прямой Mistral URL.
        // Проксируем через /api/embed только если ключ — это poll-secret и URL ведёт к Mistral.
        if (baseUrl.Contains("mistral.ai", StringComparison.OrdinalIgnoreCase))
        {
            _baseUrl = "https://dan-games-discord-poller.vercel.app/api/embed";
            _http = new HttpClient { Timeout = TimeSpan.FromSeconds(180) };
            _http.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
        }
        else
        {
            // явный URL прокси или другой endpoint — используем как есть
            _baseUrl = baseUrl.TrimEnd('/');
            _http = new HttpClient { Timeout = TimeSpan.FromSeconds(180) };
            _http.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
        }
    }

    /// <summary>Vectorize текст любого размера через прокси (throws on failure).</summary>
    public async Task<float[]> EmbedAsync(string text)
    {
        var req = new EmbedRequest { Input = text.Trim() };
        using var res = await PostWithRetryAsync(req);
        if (!res.IsSuccessStatusCode)
        {
            var body = await res.Content.ReadAsStringAsync();
            throw new InvalidOperationException($"Embedding server {res.StatusCode}: {body[..Math.Min(body.Length, 300)]}");
        }
        var data = await res.Content.ReadFromJsonAsync<EmbedResponse>();
        var embedding = data?.Embedding;
        if (embedding is null || embedding.Count == 0)
            throw new InvalidOperationException("Embedding server returned an empty vector.");
        return embedding.ToArray();
    }

    private async Task<HttpResponseMessage> PostWithRetryAsync(EmbedRequest req, int attempts = 3)
    {
        Exception? last = null;
        for (var i = 1; i <= attempts; i++)
        {
            try
            {
                return await _http.PostAsJsonAsync(_baseUrl, req);
            }
            catch (Exception ex) when (ex is HttpRequestException or IOException or TaskCanceledException)
            {
                last = ex;
                if (i < attempts) await Task.Delay(i * 2000);
            }
        }
        throw last!;
    }

    private sealed class EmbedRequest
    {
        [JsonPropertyName("input")] public string Input { get; set; } = "";
    }

    private sealed class EmbedResponse
    {
        // Прокси отдаёт плоский массив: {"ok":true,"embedding":[0.1,0.2,...]}
        [JsonPropertyName("embedding")] public List<float>? Embedding { get; set; }
    }
}