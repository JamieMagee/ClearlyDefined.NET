namespace ClearlyDefined.Client.Tests;

using AwesomeAssertions;
using ClearlyDefined.Client;
using ClearlyDefined.Client.Models;

public sealed class ClearlyDefinedClientTests
{
    private static readonly string[] components = ["npm/npmjs/-/redie/0.3.0"];

    private static readonly ComponentCoordinates npmRedie = new()
    {
        Type = ComponentType.Npm,
        Provider = ComponentProvider.NpmJs,
        Namespace = "-",
        Name = "redie",
        Revision = "0.3.0",
    };

    private readonly ClearlyDefinedClient client = new();

    // -- Definitions --

    [Fact]
    public async Task GetDefinitionsAsync_ReturnsBatchResults()
    {
        var result = await this
            .client.GetDefinitionsAsync(components, TestContext.Current.CancellationToken)
            .ConfigureAwait(true);

        _ = result.Should().ContainKey("npm/npmjs/-/redie/0.3.0");
        _ = result["npm/npmjs/-/redie/0.3.0"].Licensed.Should().NotBeNull();
    }

    [Fact]
    public async Task GetDefinitionAsync_ReturnsSingleDefinition()
    {
        var result = await this
            .client.GetDefinitionAsync(
                npmRedie,
                cancellationToken: TestContext.Current.CancellationToken
            )
            .ConfigureAwait(true);

        _ = result.Coordinates.Should().NotBeNull();
        _ = result.Licensed?.Declared.Should().NotBeNullOrEmpty();
        _ = result.Described?.ReleaseDate.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task GetDefinitionAsync_WithExcludeFiles_OmitsFiles()
    {
        var result = await this
            .client.GetDefinitionAsync(
                npmRedie,
                expand: "-files",
                cancellationToken: TestContext.Current.CancellationToken
            )
            .ConfigureAwait(true);

        _ = result.Coordinates.Should().NotBeNull();
        _ = result.Files.Should().BeNull();
    }

    [Fact]
    public async Task SearchDefinitionsAsync_ReturnsResults()
    {
        var result = await this
            .client.SearchDefinitionsAsync(
                new DefinitionSearchParameters { Pattern = "redie" },
                TestContext.Current.CancellationToken
            )
            .ConfigureAwait(true);

        _ = result.Data.Should().NotBeNull();
    }

    // -- Curations --

    [Fact]
    public async Task GetCurationAsync_ReturnsData()
    {
        var result = await this
            .client.GetCurationAsync(npmRedie, TestContext.Current.CancellationToken)
            .ConfigureAwait(true);

        _ = result.ValueKind.Should().NotBe(System.Text.Json.JsonValueKind.Undefined);
    }

    [Fact]
    public async Task GetCurationsBatchAsync_ReturnsBatchData()
    {
        var result = await this
            .client.GetCurationsBatchAsync(components, TestContext.Current.CancellationToken)
            .ConfigureAwait(true);

        _ = result.Should().NotBeNull();
    }

    // -- Harvest --

    [Fact]
    public async Task GetHarvestAsync_ReturnsHarvestData()
    {
        var result = await this
            .client.GetHarvestAsync(
                npmRedie,
                cancellationToken: TestContext.Current.CancellationToken
            )
            .ConfigureAwait(true);

        _ = result.ValueKind.Should().NotBe(System.Text.Json.JsonValueKind.Undefined);
    }

    [Fact]
    public async Task GetHarvestAsync_WithSummaryForm_ReturnsData()
    {
        var result = await this
            .client.GetHarvestAsync(
                npmRedie,
                HarvestForm.Summary,
                TestContext.Current.CancellationToken
            )
            .ConfigureAwait(true);

        _ = result.ValueKind.Should().NotBe(System.Text.Json.JsonValueKind.Undefined);
    }

    // -- Notices --

    [Fact]
    public async Task GenerateNoticeAsync_ReturnsNoticeFile()
    {
        var result = await this
            .client.GenerateNoticeAsync(
                new { coordinates = components },
                TestContext.Current.CancellationToken
            )
            .ConfigureAwait(true);

        _ = result.Content.Should().NotBeNullOrEmpty();
    }
}
