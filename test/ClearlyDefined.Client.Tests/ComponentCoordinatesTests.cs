namespace ClearlyDefined.Client.Tests;

using AwesomeAssertions;
using ClearlyDefined.Schema;

public sealed class ComponentCoordinatesTests
{
    [Fact]
    public void ToString_WithRevision_ReturnsFullCoordinates()
    {
        var coordinates = new ComponentCoordinates
        {
            Type = ComponentType.Npm,
            Provider = ComponentProvider.NpmJs,
            Namespace = "-",
            Name = "redie",
            Revision = "0.3.0",
        };

        _ = coordinates.ToString().Should().Be("npm/npmjs/-/redie/0.3.0");
    }

    [Fact]
    public void ToString_WithoutRevision_OmitsTrailingSegment()
    {
        var coordinates = new ComponentCoordinates
        {
            Type = ComponentType.Npm,
            Provider = ComponentProvider.NpmJs,
            Namespace = "-",
            Name = "redie",
        };

        _ = coordinates.ToString().Should().Be("npm/npmjs/-/redie");
    }

    [Fact]
    public void ToString_GitCoordinates_ReturnsCorrectPath()
    {
        var coordinates = new ComponentCoordinates
        {
            Type = ComponentType.Git,
            Provider = ComponentProvider.GitHub,
            Namespace = "microsoft",
            Name = "redie",
            Revision = "abc123",
        };

        _ = coordinates.ToString().Should().Be("git/github/microsoft/redie/abc123");
    }
}
