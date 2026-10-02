namespace ClearlyDefined.Client.Tests;

using System.Net;
using System.Text;
using System.Text.Json;
using AwesomeAssertions;
using ClearlyDefined.Client;
using ClearlyDefined.Client.Models;
using Moq;
using Moq.Protected;

public sealed class ClearlyDefinedClientTests : IDisposable
{
    private const string Component = "npm/npmjs/-/redie/0.3.0";
    private const string ComponentsJson = """["npm/npmjs/-/redie/0.3.0"]""";
    private const string DefinitionJson = """
        {
          "coordinates": {
            "type": "npm",
            "provider": "npmjs",
            "namespace": "-",
            "name": "redie",
            "revision": "0.3.0"
          },
          "licensed": {"declared": "MIT"},
          "described": {"releaseDate": "2016-08-18"},
          "files": [{"path": "index.js"}]
        }
        """;

    private static readonly string[] components = [Component];

    private static readonly ComponentCoordinates npmRedie = new()
    {
        Type = ComponentType.Npm,
        Provider = ComponentProvider.NpmJs,
        Namespace = "-",
        Name = "redie",
        Revision = "0.3.0",
    };

    private readonly Mock<HttpMessageHandler> handler = new(MockBehavior.Strict);
    private readonly HttpClient httpClient;
    private readonly ClearlyDefinedClient client;

    public ClearlyDefinedClientTests()
    {
        _ = this.handler.Protected().Setup("Dispose", ItExpr.IsAny<bool>()).CallBase();
        this.httpClient = new HttpClient(this.handler.Object);
        this.client = new ClearlyDefinedClient(this.httpClient);
    }

    public void Dispose() => this.httpClient.Dispose();

    // -- Definitions --

    [Fact]
    public async Task GetDefinitionsAsync_ReturnsBatchResults()
    {
        this.SetupResponse(
            HttpMethod.Post,
            "definitions",
            $"{{\"{Component}\":{DefinitionJson}}}",
            requestBody: ComponentsJson
        );

        var result = await this
            .client.GetDefinitionsAsync(components, TestContext.Current.CancellationToken)
            .ConfigureAwait(true);

        _ = result.Should().ContainKey(Component);
        _ = (result[Component].Licensed?.Declared).Should().Be("MIT");
        this.VerifyRequest();
    }

    [Fact]
    public async Task GetDefinitionAsync_ReturnsSingleDefinition()
    {
        this.SetupResponse(HttpMethod.Get, $"definitions/{Component}", DefinitionJson);

        var result = await this
            .client.GetDefinitionAsync(
                npmRedie,
                cancellationToken: TestContext.Current.CancellationToken
            )
            .ConfigureAwait(true);

        _ = result.Coordinates.Should().Be(npmRedie);
        _ = (result.Licensed?.Declared).Should().Be("MIT");
        _ = (result.Described?.ReleaseDate).Should().Be("2016-08-18");
        _ = result.Files.Should().ContainSingle();
        this.VerifyRequest();
    }

    [Fact]
    public async Task GetDefinitionAsync_WithExcludeFiles_OmitsFiles()
    {
        this.SetupResponse(
            HttpMethod.Get,
            $"definitions/{Component}?expand=-files",
            """
            {
              "coordinates": {
                "type": "npm",
                "provider": "npmjs",
                "namespace": "-",
                "name": "redie",
                "revision": "0.3.0"
              }
            }
            """
        );

        var result = await this
            .client.GetDefinitionAsync(
                npmRedie,
                expand: "-files",
                cancellationToken: TestContext.Current.CancellationToken
            )
            .ConfigureAwait(true);

        _ = result.Coordinates.Should().Be(npmRedie);
        _ = result.Files.Should().BeNull();
        this.VerifyRequest();
    }

    [Fact]
    public async Task SearchDefinitionsAsync_ReturnsResults()
    {
        this.SetupResponse(HttpMethod.Get, "definitions?pattern=redie", ComponentsJson);

        var result = await this
            .client.SearchDefinitionsAsync(
                new DefinitionSearchParameters { Pattern = "redie" },
                TestContext.Current.CancellationToken
            )
            .ConfigureAwait(true);

        _ = result.Should().Equal(components);
        this.VerifyRequest();
    }

    // -- Curations --

    [Fact]
    public async Task GetCurationAsync_Returns404ForUncuratedComponent()
    {
        this.SetupResponse(
            HttpMethod.Get,
            $"curations/{Component}",
            "{}",
            statusCode: HttpStatusCode.NotFound
        );

        var act = () =>
            this.client.GetCurationAsync(npmRedie, TestContext.Current.CancellationToken);

        var exception = await act.Should().ThrowAsync<HttpRequestException>().ConfigureAwait(true);

        _ = exception.Which.StatusCode.Should().Be(HttpStatusCode.NotFound);
        this.VerifyRequest();
    }

    [Fact]
    public async Task GetCurationsBatchAsync_ReturnsBatchData()
    {
        this.SetupResponse(
            HttpMethod.Post,
            "curations",
            """
            {
              "npm/npmjs/-/redie/0.3.0": {
                "licensed": {"declared": "MIT"}
              }
            }
            """,
            requestBody: ComponentsJson
        );

        var result = await this
            .client.GetCurationsBatchAsync(components, TestContext.Current.CancellationToken)
            .ConfigureAwait(true);

        _ = result.Should().ContainKey(Component);
        _ = result[Component]
            .GetProperty("licensed")
            .GetProperty("declared")
            .GetString()
            .Should()
            .Be("MIT");
        this.VerifyRequest();
    }

    // -- Harvest --

    [Fact]
    public async Task GetHarvestAsync_ReturnsHarvestData()
    {
        this.SetupResponse(
            HttpMethod.Get,
            $"harvest/{Component}",
            """{"scancode":{"32.1.0":{}}}"""
        );

        var result = await this
            .client.GetHarvestAsync(
                npmRedie,
                cancellationToken: TestContext.Current.CancellationToken
            )
            .ConfigureAwait(true);

        _ = result
            .GetProperty("scancode")
            .GetProperty("32.1.0")
            .ValueKind.Should()
            .Be(JsonValueKind.Object);
        this.VerifyRequest();
    }

    [Fact]
    public async Task GetHarvestAsync_WithSummaryForm_ReturnsData()
    {
        this.SetupResponse(
            HttpMethod.Get,
            $"harvest/{Component}?form=summary",
            """{"scancode":{"32.1.0":{}}}"""
        );

        var result = await this
            .client.GetHarvestAsync(
                npmRedie,
                HarvestForm.Summary,
                TestContext.Current.CancellationToken
            )
            .ConfigureAwait(true);

        _ = result
            .GetProperty("scancode")
            .GetProperty("32.1.0")
            .ValueKind.Should()
            .Be(JsonValueKind.Object);
        this.VerifyRequest();
    }

    // -- Notices --

    [Fact]
    public async Task GenerateNoticeAsync_ReturnsNoticeFile()
    {
        this.SetupResponse(
            HttpMethod.Post,
            "notices",
            """{"content":"Notice text","summary":{"total":1}}""",
            requestBody: """{"coordinates":["npm/npmjs/-/redie/0.3.0"]}"""
        );

        var result = await this
            .client.GenerateNoticeAsync(
                new { coordinates = components },
                TestContext.Current.CancellationToken
            )
            .ConfigureAwait(true);

        _ = result.Content.Should().Be("Notice text");
        _ = (result.Summary?.Total).Should().Be(1);
        this.VerifyRequest();
    }

    private void SetupResponse(
        HttpMethod method,
        string path,
        string responseBody,
        string? requestBody = null,
        HttpStatusCode statusCode = HttpStatusCode.OK
    )
    {
        var uri = new Uri($"https://api.clearlydefined.io/{path}");

        _ = this.handler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(request =>
                    request.Method == method && request.RequestUri == uri
                ),
                ItExpr.IsAny<CancellationToken>()
            )
            .Returns(async (HttpRequestMessage request, CancellationToken cancellationToken) =>
            {
                if (requestBody is not null)
                {
                    Assert.NotNull(request.Content);
                    var body = await request
                        .Content.ReadAsStringAsync(cancellationToken)
                        .ConfigureAwait(true);
                    using var expected = JsonDocument.Parse(requestBody);
                    using var actual = JsonDocument.Parse(body);

                    _ = JsonElement
                        .DeepEquals(actual.RootElement, expected.RootElement)
                        .Should()
                        .BeTrue("the request body must be {0}, but was {1}", requestBody, body);
                }

                return new HttpResponseMessage(statusCode)
                {
                    Content = new StringContent(responseBody, Encoding.UTF8, "application/json"),
                };
            });
    }

    private void VerifyRequest() =>
        this.handler
            .Protected()
            .Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            );
}
