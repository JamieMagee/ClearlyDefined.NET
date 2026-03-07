namespace ClearlyDefined.Client;

using System.Text.Json.Serialization;

/// <summary>
/// The type of a component in the ClearlyDefined ecosystem.
/// </summary>
[JsonConverter(typeof(ComponentTypeJsonConverter))]
public enum ComponentType
{
    Composer,
    Conda,
    CondaSrc,
    Crate,
    Deb,
    DebSrc,
    Gem,
    Git,
    Go,
    Maven,
    Npm,
    NuGet,
    Pod,
    PyPi,
    SourceArchive,
}
