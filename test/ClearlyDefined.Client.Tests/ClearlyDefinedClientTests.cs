namespace ClearlyDefined.Client.Tests;

using ClearlyDefined.Schema;

public sealed class ClearlyDefinedClientTests : IDisposable
{
    private readonly ClearlyDefinedClient client = new();

    public void Dispose() => this.client.Dispose();

    [Fact]
    public async Task ShouldWorkAsync() =>
        await this
            .client.QueryDefinitionsAsync(
                ["npm/npmjs/-/redie/0.3.0"],
                TestContext.Current.CancellationToken
            )
            .ConfigureAwait(true);
}
