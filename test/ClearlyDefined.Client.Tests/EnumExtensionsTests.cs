namespace ClearlyDefined.Client.Tests;

using AwesomeAssertions;
using ClearlyDefined.Schema;

public sealed class EnumExtensionsTests
{
    [Theory]
    [InlineData(ComponentType.Npm, "npm")]
    [InlineData(ComponentType.NuGet, "nuget")]
    [InlineData(ComponentType.SourceArchive, "sourcearchive")]
    [InlineData(ComponentType.CondaSrc, "condasrc")]
    public void ComponentType_ToApiString_ReturnsExpected(ComponentType type, string expected) =>
        type.ToApiString().Should().Be(expected);

    [Theory]
    [InlineData("npm", ComponentType.Npm)]
    [InlineData("nuget", ComponentType.NuGet)]
    [InlineData("sourcearchive", ComponentType.SourceArchive)]
    [InlineData("condasrc", ComponentType.CondaSrc)]
    public void ToComponentType_ParsesCorrectly(string value, ComponentType expected) =>
        EnumExtensions.ToComponentType(value).Should().Be(expected);

    [Theory]
    [InlineData(ComponentProvider.NpmJs, "npmjs")]
    [InlineData(ComponentProvider.AnacondaMain, "anaconda-main")]
    [InlineData(ComponentProvider.CondaForge, "conda-forge")]
    [InlineData(ComponentProvider.MavenCentral, "mavencentral")]
    public void ComponentProvider_ToApiString_ReturnsExpected(
        ComponentProvider provider,
        string expected
    ) => provider.ToApiString().Should().Be(expected);

    [Theory]
    [InlineData("npmjs", ComponentProvider.NpmJs)]
    [InlineData("anaconda-main", ComponentProvider.AnacondaMain)]
    [InlineData("conda-forge", ComponentProvider.CondaForge)]
    [InlineData("mavencentral", ComponentProvider.MavenCentral)]
    public void ToComponentProvider_ParsesCorrectly(string value, ComponentProvider expected) =>
        EnumExtensions.ToComponentProvider(value).Should().Be(expected);

    [Theory]
    [InlineData(HarvestForm.Summary, "summary")]
    [InlineData(HarvestForm.Streamed, "streamed")]
    [InlineData(HarvestForm.Raw, "raw")]
    [InlineData(HarvestForm.List, "list")]
    public void HarvestForm_ToApiString_ReturnsExpected(HarvestForm form, string expected) =>
        form.ToApiString().Should().Be(expected);

    [Theory]
    [InlineData("summary", HarvestForm.Summary)]
    [InlineData("streamed", HarvestForm.Streamed)]
    [InlineData("raw", HarvestForm.Raw)]
    [InlineData("list", HarvestForm.List)]
    public void ToHarvestForm_ParsesCorrectly(string value, HarvestForm expected) =>
        EnumExtensions.ToHarvestForm(value).Should().Be(expected);
}
