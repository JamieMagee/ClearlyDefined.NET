namespace ClearlyDefined.Schema;

using System.Text.Json.Serialization;

/// <summary>
/// Represents the coordinates of a component in the ClearlyDefined ecosystem.
/// </summary>
public sealed record ComponentCoordinates
{
    [JsonPropertyName("type")]
    public ComponentType Type { get; init; }

    [JsonPropertyName("provider")]
    public ComponentProvider Provider { get; init; }

    [JsonPropertyName("namespace")]
    public string Namespace { get; init; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    [JsonPropertyName("revision")]
    public string? Revision { get; init; }

    /// <inheritdoc/>
    public override string ToString()
    {
        var path =
            $"{this.Type.ToApiString()}/{this.Provider.ToApiString()}/{this.Namespace}/{this.Name}";
        return this.Revision is not null ? $"{path}/{this.Revision}" : path;
    }
}
