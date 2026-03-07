namespace ClearlyDefined.Schema.Models;

using System.Text.Json;
using System.Text.Json.Serialization;

/// <summary>
/// Represents a notice file generation request. Loosely typed to match the API.
/// </summary>
public sealed record NoticeRequest
{
    [JsonPropertyName("content")]
    public JsonElement? Content { get; init; }
}
