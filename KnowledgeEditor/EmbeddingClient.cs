using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace KnowledgeEditor;

/// <summary>Talks to the Mistral AI embeddings endpoint (mistral-embed, 1024 dims).</summary>
public sealed class EmbeddingClient
{
    private readonly HttpClient _http;
    private readonly string _apiKey;
    private readonly string _model;

    public EmbeddingClient(string baseUrl, string apiKey, string model)
    {
        _apiKey = apiKey;
        _model = model;
        _http = new HttpClient { BaseAddress = new Uri(baseUrl), Timeout = TimeSpan.FromSeconds(90) };
        _http.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
    }

    /// <summary>Vectorize a single text (throws on failure).</summary>
    public async Task<float[]> EmbedAsync(string text)
    {
        var req = new EmbedRequest { Input = text.Trim(), Model = _model };
        var res = await _http.PostAsJsonAsync("", req);
        if (!res.IsSuccessStatusCode)
        {
            var body = await res.Content.ReadAsStringAsync();
            throw new InvalidOperationException($"Embedding server {res.StatusCode}: {body[..Math.Min(body.Length, 300)]}");
        }
        var data = await res.Content.ReadFromJsonAsync<EmbedResponse>();
        var embedding = data?.Data?[0].Embedding;
        if (embedding is null || embedding.Count == 0)
            throw new InvalidOperationException("Embedding server returned an empty vector.");
        return embedding.ToArray();
    }

    private sealed class EmbedRequest
    {
        [JsonPropertyName("input")] public string Input { get; set; } = "";
        [JsonPropertyName("model")] public string Model { get; set; } = "";
    }

    private sealed class EmbedResponse
    {
        [JsonPropertyName("data")] public List<EmbedData>? Data { get; set; }
    }

    private sealed class EmbedData
    {
        [JsonPropertyName("embedding")] public List<float>? Embedding { get; set; }
    }
}