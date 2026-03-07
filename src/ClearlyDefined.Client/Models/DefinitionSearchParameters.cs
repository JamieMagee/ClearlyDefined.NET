namespace ClearlyDefined.Client.Models;

using System.Text.Json.Serialization;

/// <summary>
/// Query parameters for the GET /definitions search endpoint.
/// </summary>
public sealed record DefinitionSearchParameters
{
    [JsonPropertyName("pattern")]
    public string? Pattern { get; init; }

    [JsonPropertyName("type")]
    public string? Type { get; init; }

    [JsonPropertyName("provider")]
    public string? Provider { get; init; }

    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("namespace")]
    public string? Namespace { get; init; }

    [JsonPropertyName("license")]
    public string? License { get; init; }

    [JsonPropertyName("releasedAfter")]
    public string? ReleasedAfter { get; init; }

    [JsonPropertyName("releasedBefore")]
    public string? ReleasedBefore { get; init; }

    [JsonPropertyName("minLicensedScore")]
    public int? MinLicensedScore { get; init; }

    [JsonPropertyName("maxLicensedScore")]
    public int? MaxLicensedScore { get; init; }

    [JsonPropertyName("minDescribedScore")]
    public int? MinDescribedScore { get; init; }

    [JsonPropertyName("maxDescribedScore")]
    public int? MaxDescribedScore { get; init; }

    [JsonPropertyName("sort")]
    public string? Sort { get; init; }

    [JsonPropertyName("sortDesc")]
    public bool? SortDesc { get; init; }

    [JsonPropertyName("continuationToken")]
    public string? ContinuationToken { get; init; }
}
