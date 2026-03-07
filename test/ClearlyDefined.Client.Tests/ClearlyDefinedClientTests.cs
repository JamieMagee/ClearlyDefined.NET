namespace ClearlyDefined.Client.Tests;

using ClearlyDefined.Schema;

public sealed class ClearlyDefinedClientTests
{
    [Fact]
    public async Task ShouldWorkAsync()
    {
        var client = new ClearlyDefinedClient();
        _ = await client
            .GetDefinitionsAsync(["npm/npmjs/-/redie/0.3.0"], TestContext.Current.CancellationToken)
            .ConfigureAwait(true);
    }
}
