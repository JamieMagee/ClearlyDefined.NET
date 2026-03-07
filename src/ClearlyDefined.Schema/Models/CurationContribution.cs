namespace ClearlyDefined.Schema.Models;

using System.Text.Json;
using System.Text.Json.Serialization;

/// <summary>
/// Represents a curation contribution (PR) in the ClearlyDefined API.
/// </summary>
public sealed record CurationContribution
{
    [JsonPropertyName("info")]
    public ContributionInfo? Info { get; init; }

    [JsonPropertyName("patches")]
    public IReadOnlyList<JsonElement>? Patches { get; init; }
}

/// <summary>
/// Metadata about a curation contribution.
/// </summary>
public sealed record ContributionInfo
{
    [JsonPropertyName("summary")]
    public string? Summary { get; init; }

    [JsonPropertyName("details")]
    public string? Details { get; init; }

    [JsonPropertyName("resolution")]
    public string? Resolution { get; init; }

    [JsonPropertyName("type")]
    public string? Type { get; init; }

    [JsonPropertyName("removedDefinitions")]
    public IReadOnlyList<string>? RemovedDefinitions { get; init; }
}
