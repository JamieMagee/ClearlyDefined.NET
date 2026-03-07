namespace ClearlyDefined.Client.Models;

using System.Text.Json;
using System.Text.Json.Serialization;

/// <summary>
/// Represents a ClearlyDefined definition response.
/// </summary>
public sealed record Definition
{
    [JsonPropertyName("coordinates")]
    public ComponentCoordinates? Coordinates { get; init; }

    [JsonPropertyName("described")]
    public DefinitionDescribed? Described { get; init; }

    [JsonPropertyName("licensed")]
    public DefinitionLicensed? Licensed { get; init; }

    [JsonPropertyName("scores")]
    public DefinitionScores? Scores { get; init; }

    [JsonPropertyName("files")]
    public IReadOnlyList<JsonElement>? Files { get; init; }
}

/// <summary>
/// Described metadata for a definition.
/// </summary>
public sealed record DefinitionDescribed
{
    [JsonPropertyName("sourceLocation")]
    public ComponentCoordinates? SourceLocation { get; init; }

    [JsonPropertyName("releaseDate")]
    public string? ReleaseDate { get; init; }
}

/// <summary>
/// Licensed metadata for a definition.
/// </summary>
public sealed record DefinitionLicensed
{
    [JsonPropertyName("declared")]
    public string? Declared { get; init; }

    [JsonPropertyName("facets")]
    public DefinitionFacets? Facets { get; init; }
}

/// <summary>
/// Facets within licensed metadata.
/// </summary>
public sealed record DefinitionFacets
{
    [JsonPropertyName("core")]
    public DefinitionCore? Core { get; init; }
}

/// <summary>
/// Core facet information.
/// </summary>
public sealed record DefinitionCore
{
    [JsonPropertyName("attribution")]
    public DefinitionAttribution? Attribution { get; init; }

    [JsonPropertyName("discovered")]
    public JsonElement? Discovered { get; init; }
}

/// <summary>
/// Attribution information within a core facet.
/// </summary>
public sealed record DefinitionAttribution
{
    [JsonPropertyName("parties")]
    public IReadOnlyList<string>? Parties { get; init; }
}

/// <summary>
/// Scores for a definition.
/// </summary>
public sealed record DefinitionScores
{
    [JsonPropertyName("effective")]
    public int? Effective { get; init; }

    [JsonPropertyName("tool")]
    public int? Tool { get; init; }

    [JsonPropertyName("licensed")]
    public int? Licensed { get; init; }

    [JsonPropertyName("described")]
    public int? Described { get; init; }
}
