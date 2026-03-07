namespace ClearlyDefined.Client.Models;

using System.Text.Json;
using System.Text.Json.Serialization;

/// <summary>
/// Represents a loosely-typed curation from the ClearlyDefined API.
/// </summary>
public sealed record Curation
{
    [JsonPropertyName("data")]
    public JsonElement? Data { get; init; }
}
