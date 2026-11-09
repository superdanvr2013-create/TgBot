using System.Text.Json.Serialization;

namespace KnowledgeEditor;

/// <summary>CRUD для individual (персональные обращения). Через Vercel API.</summary>
public sealed class IndividualRepository
{
    private readonly DbApiClient _api;

    public IndividualRepository(AppConfig config)
    {
        _api = new DbApiClient(config.ApiUrl, config.ApiSecret);
    }

    private sealed class ApiRow
    {
        [JsonPropertyName("id")] public long Id { get; set; }
        [JsonPropertyName("nickname")] public string Nickname { get; set; } = "";
        [JsonPropertyName("description")] public string Description { get; set; } = "";
        [JsonPropertyName("updated_at")] public DateTime UpdatedAt { get; set; }
    }

    public async Task<IReadOnlyList<IndividualRow>> ListAsync()
    {
        var rows = await _api.ListAsync<ApiRow>("individual");
        return rows.Select(r => new IndividualRow
        {
            Id = r.Id,
            Nickname = r.Nickname,
            Description = r.Description,
            UpdatedAt = r.UpdatedAt.ToUniversalTime().ToLocalTime(),
        }).ToList();
    }

    public async Task<long> AddAsync(string nickname, string description)
        => await _api.PostAsync(new { op = "add", table = "individual", nickname, description });

    public async Task UpdateAsync(long id, string nickname, string description)
        => await _api.PostAsync(new { op = "update", table = "individual", id, nickname, description });

    public async Task DeleteAsync(long id)
        => await _api.PostAsync(new { op = "delete", table = "individual", id });
}

public sealed class IndividualRow
{
    public long Id { get; set; }
    public string Nickname { get; set; } = "";
    public string Description { get; set; } = "";
    public DateTime UpdatedAt { get; set; }
}
