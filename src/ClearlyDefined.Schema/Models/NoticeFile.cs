namespace ClearlyDefined.Schema.Models;

using System.Text.Json.Serialization;

/// <summary>
/// Represents a generated notice file response.
/// </summary>
public sealed record NoticeFile
{
    [JsonPropertyName("content")]
    public string? Content { get; init; }

    [JsonPropertyName("summary")]
    public NoticeFileSummary? Summary { get; init; }
}

/// <summary>
/// Summary statistics for a generated notice file.
/// </summary>
public sealed record NoticeFileSummary
{
    [JsonPropertyName("total")]
    public int? Total { get; init; }

    [JsonPropertyName("warnings")]
    public NoticeFileWarnings? Warnings { get; init; }
}

/// <summary>
/// Warning details for a notice file.
/// </summary>
public sealed record NoticeFileWarnings
{
    [JsonPropertyName("noDefinition")]
    public IReadOnlyList<string>? NoDefinition { get; init; }

    [JsonPropertyName("noLicense")]
    public IReadOnlyList<string>? NoLicense { get; init; }

    [JsonPropertyName("noCopyright")]
    public IReadOnlyList<string>? NoCopyright { get; init; }
}
