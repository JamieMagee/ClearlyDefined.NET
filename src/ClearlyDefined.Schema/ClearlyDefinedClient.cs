namespace ClearlyDefined.Schema;

using System.Reflection;
using RestSharp;

public sealed class ClearlyDefinedClient : IDisposable
{
    private readonly RestClient client;

    public ClearlyDefinedClient()
        : this("https://api.clearlydefined.io/")
    {
    }

    public ClearlyDefinedClient(string baseUri)
    {
        var version = typeof(ClearlyDefinedClient).GetTypeInfo().Assembly.GetName().Version;

        var clientOptions = new RestClientOptions(baseUri)
        {
            UserAgent = $"ClearlyDefined.NET/{version}",
        };

        this.client = new RestClient(clientOptions);
    }

    public void Dispose() => this.client.Dispose();

    public async Task QueryDefinitionsAsync(IEnumerable<string> components, CancellationToken cancellationToken = default)
    {
        var request = new RestRequest("definitions", Method.Post).AddBody(components);
        var response = await this.client.ExecuteAsync<IDictionary<string, string>>(request, cancellationToken).ConfigureAwait(false);

        if (!response.IsSuccessful)
        {
            throw new BadImageFormatException("a");
        }

        var data = response.ThrowIfError().Data;
    }
}
