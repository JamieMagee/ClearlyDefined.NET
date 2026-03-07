namespace ClearlyDefined.Client.Models;

using System.Text.Json.Serialization;

/// <summary>
/// Result of a definition search.
/// </summary>
public sealed record DefinitionSearchResult
{
    [JsonPropertyName("data")]
    public IReadOnlyList<string>? Data { get; init; }

    [JsonPropertyName("continuationToken")]
    public string? ContinuationToken { get; init; }
}
