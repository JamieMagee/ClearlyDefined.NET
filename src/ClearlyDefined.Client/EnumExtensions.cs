namespace ClearlyDefined.Client;

using System;

/// <summary>
/// Extension methods for converting enums to and from their API string representations.
/// </summary>
public static class EnumExtensions
{
    /// <summary>
    /// Converts a <see cref="ComponentType"/> to its API string representation.
    /// </summary>
    public static string ToApiString(this ComponentType value) =>
        value switch
        {
            ComponentType.Composer => "composer",
            ComponentType.Conda => "conda",
            ComponentType.CondaSrc => "condasrc",
            ComponentType.Crate => "crate",
            ComponentType.Deb => "deb",
            ComponentType.DebSrc => "debsrc",
            ComponentType.Gem => "gem",
            ComponentType.Git => "git",
            ComponentType.Go => "go",
            ComponentType.Maven => "maven",
            ComponentType.Npm => "npm",
            ComponentType.NuGet => "nuget",
            ComponentType.Pod => "pod",
            ComponentType.PyPi => "pypi",
            ComponentType.SourceArchive => "sourcearchive",
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null),
        };

    /// <summary>
    /// Parses an API string to a <see cref="ComponentType"/>.
    /// </summary>
    public static ComponentType ToComponentType(string value) =>
        value switch
        {
            "composer" => ComponentType.Composer,
            "conda" => ComponentType.Conda,
            "condasrc" => ComponentType.CondaSrc,
            "crate" => ComponentType.Crate,
            "deb" => ComponentType.Deb,
            "debsrc" => ComponentType.DebSrc,
            "gem" => ComponentType.Gem,
            "git" => ComponentType.Git,
            "go" => ComponentType.Go,
            "maven" => ComponentType.Maven,
            "npm" => ComponentType.Npm,
            "nuget" => ComponentType.NuGet,
            "pod" => ComponentType.Pod,
            "pypi" => ComponentType.PyPi,
            "sourcearchive" => ComponentType.SourceArchive,
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null),
        };

    /// <summary>
    /// Converts a <see cref="ComponentProvider"/> to its API string representation.
    /// </summary>
    public static string ToApiString(this ComponentProvider value) =>
        value switch
        {
            ComponentProvider.AnacondaMain => "anaconda-main",
            ComponentProvider.AnacondaR => "anaconda-r",
            ComponentProvider.CocoaPods => "cocoapods",
            ComponentProvider.CondaForge => "conda-forge",
            ComponentProvider.CratesIo => "cratesio",
            ComponentProvider.Debian => "debian",
            ComponentProvider.GitHub => "github",
            ComponentProvider.GitLab => "gitlab",
            ComponentProvider.MavenCentral => "mavencentral",
            ComponentProvider.MavenGoogle => "mavengoogle",
            ComponentProvider.GradlePlugin => "gradleplugin",
            ComponentProvider.NpmJs => "npmjs",
            ComponentProvider.NuGet => "nuget",
            ComponentProvider.Packagist => "packagist",
            ComponentProvider.PyPi => "pypi",
            ComponentProvider.RubyGems => "rubygems",
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null),
        };

    /// <summary>
    /// Parses an API string to a <see cref="ComponentProvider"/>.
    /// </summary>
    public static ComponentProvider ToComponentProvider(string value) =>
        value switch
        {
            "anaconda-main" => ComponentProvider.AnacondaMain,
            "anaconda-r" => ComponentProvider.AnacondaR,
            "cocoapods" => ComponentProvider.CocoaPods,
            "conda-forge" => ComponentProvider.CondaForge,
            "cratesio" => ComponentProvider.CratesIo,
            "debian" => ComponentProvider.Debian,
            "github" => ComponentProvider.GitHub,
            "gitlab" => ComponentProvider.GitLab,
            "mavencentral" => ComponentProvider.MavenCentral,
            "mavengoogle" => ComponentProvider.MavenGoogle,
            "gradleplugin" => ComponentProvider.GradlePlugin,
            "npmjs" => ComponentProvider.NpmJs,
            "nuget" => ComponentProvider.NuGet,
            "packagist" => ComponentProvider.Packagist,
            "pypi" => ComponentProvider.PyPi,
            "rubygems" => ComponentProvider.RubyGems,
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null),
        };

    /// <summary>
    /// Converts a <see cref="HarvestForm"/> to its API string representation.
    /// </summary>
    public static string ToApiString(this HarvestForm value) =>
        value switch
        {
            HarvestForm.Summary => "summary",
            HarvestForm.Streamed => "streamed",
            HarvestForm.Raw => "raw",
            HarvestForm.List => "list",
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null),
        };

    /// <summary>
    /// Parses an API string to a <see cref="HarvestForm"/>.
    /// </summary>
    public static HarvestForm ToHarvestForm(string value) =>
        value switch
        {
            "summary" => HarvestForm.Summary,
            "streamed" => HarvestForm.Streamed,
            "raw" => HarvestForm.Raw,
            "list" => HarvestForm.List,
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null),
        };
}
