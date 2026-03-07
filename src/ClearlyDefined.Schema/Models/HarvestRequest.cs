namespace ClearlyDefined.Schema.Models;

using System.Text.Json.Serialization;

/// <summary>
/// Represents a harvest request for the ClearlyDefined API.
/// </summary>
public sealed record HarvestRequest
{
    [JsonPropertyName("tool")]
    public string? Tool { get; init; }

    [JsonPropertyName("coordinates")]
    public string? Coordinates { get; init; }
}
