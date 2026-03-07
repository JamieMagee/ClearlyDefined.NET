namespace ClearlyDefined.Client;

using System.Text.Json.Serialization;

/// <summary>
/// The provider of a component in the ClearlyDefined ecosystem.
/// </summary>
[JsonConverter(typeof(ComponentProviderJsonConverter))]
public enum ComponentProvider
{
    AnacondaMain,
    AnacondaR,
    CocoaPods,
    CondaForge,
    CratesIo,
    Debian,
    GitHub,
    GitLab,
    MavenCentral,
    MavenGoogle,
    GradlePlugin,
    NpmJs,
    NuGet,
    Packagist,
    PyPi,
    RubyGems,
}
