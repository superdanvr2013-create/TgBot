using System.Text.Json.Serialization;

namespace KnowledgeEditor;

/// <summary>
/// CRUD для ai_providers (конфигурация ИИ-провайдеров). Работает через Vercel API
/// или напрямую (режимы как у остальных репозиториев).
/// </summary>
public sealed class ProvidersRepository
{
    private readonly DbApiClient? _api;

    public ProvidersRepository(AppConfig config)
    {
        if (config.DbMode != "direct")
        {
            _api = new DbApiClient(config.ApiUrl, config.ApiSecret);
        }
    }

    private sealed class ApiRow
    {
        [JsonPropertyName("id")] public long Id { get; set; }
        [JsonPropertyName("name")] public string Name { get; set; } = "";
        [JsonPropertyName("base_url")] public string BaseUrl { get; set; } = "";
        [JsonPropertyName("api_key")] public string ApiKey { get; set; } = "";
        [JsonPropertyName("model")] public string Model { get; set; } = "";
        [JsonPropertyName("description")] public string? Description { get; set; }
        [JsonPropertyName("active")] public bool Active { get; set; }
        [JsonPropertyName("sort_order")] public long SortOrder { get; set; }
        [JsonPropertyName("updated_at")] public DateTime UpdatedAt { get; set; }
    }

    private static ProviderRow ToRow(ApiRow r) => new()
    {
        Id = r.Id,
        Name = r.Name,
        BaseUrl = r.BaseUrl,
        ApiKey = r.ApiKey,
        Model = r.Model,
        Description = r.Description ?? "",
        Active = r.Active,
        SortOrder = r.SortOrder,
        UpdatedAt = r.UpdatedAt.ToUniversalTime().ToLocalTime(),
    };

    public async Task<IReadOnlyList<ProviderRow>> ListAsync()
    {
        if (_api is null) throw new NotSupportedException("direct-режим для провайдеров не реализован (используется api)");
        var rows = await _api.ListAsync<ApiRow>("providers");
        return rows.Select(ToRow).ToList();
    }

    public async Task<long> AddAsync(ProviderRow p)
        => await _api!.PostAsync(new { op = "add", table = "providers", name = p.Name, base_url = p.BaseUrl, api_key = p.ApiKey, model = p.Model, description = p.Description, active = p.Active, sort_order = (int)p.SortOrder });

    public async Task UpdateAsync(ProviderRow p)
        => await _api!.PostAsync(new { op = "update", table = "providers", id = p.Id, name = p.Name, base_url = p.BaseUrl, api_key = p.ApiKey, model = p.Model, description = p.Description, active = p.Active, sort_order = (int)p.SortOrder });

    public async Task ActivateAsync(long id)
        => await _api!.PostAsync(new { op = "activate", table = "providers", id });

    public async Task DeleteAsync(long id)
        => await _api!.PostAsync(new { op = "delete", table = "providers", id });
}

public sealed class ProviderRow
{
    public long Id { get; set; }
    public string Name { get; set; } = "";
    public string BaseUrl { get; set; } = "";
    public string ApiKey { get; set; } = "";
    public string Model { get; set; } = "";
    public string Description { get; set; } = "";
    public bool Active { get; set; }
    public long SortOrder { get; set; }
    public DateTime UpdatedAt { get; set; }
}
