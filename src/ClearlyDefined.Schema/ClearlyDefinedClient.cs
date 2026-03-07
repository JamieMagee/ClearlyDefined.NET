namespace ClearlyDefined.Schema;

using System.Globalization;
using System.Reflection;
using System.Text.Json;
using ClearlyDefined.Schema.Models;
using RestSharp;

/// <summary>
/// HTTP client for the ClearlyDefined REST API.
/// </summary>
public sealed class ClearlyDefinedClient : IClearlyDefinedClient
{
    private readonly RestClient client;

    public ClearlyDefinedClient()
        : this("https://api.clearlydefined.io/") { }

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

    // ── Definitions ──────────────────────────────────────────────────

    public async Task<Dictionary<string, Definition>> GetDefinitionsAsync(
        IEnumerable<string> components,
        CancellationToken cancellationToken = default
    )
    {
        var request = new RestRequest("definitions", Method.Post).AddBody(components);
        var response = await this
            .client.ExecuteAsync<Dictionary<string, Definition>>(request, cancellationToken)
            .ConfigureAwait(false);

        EnsureSuccess(response);
        return response.Data!;
    }

    public async Task<Definition> GetDefinitionAsync(
        ComponentCoordinates coordinates,
        string? expand = null,
        CancellationToken cancellationToken = default
    )
    {
        var request = new RestRequest($"definitions/{coordinates}");
        if (expand is not null)
        {
            _ = request.AddQueryParameter("expand", expand);
        }

        var response = await this
            .client.ExecuteAsync<Definition>(request, cancellationToken)
            .ConfigureAwait(false);

        EnsureSuccess(response);
        return response.Data!;
    }

    public async Task<DefinitionSearchResult> SearchDefinitionsAsync(
        DefinitionSearchParameters parameters,
        CancellationToken cancellationToken = default
    )
    {
        var request = new RestRequest("definitions");
        AddSearchParameters(request, parameters);

        var response = await this
            .client.ExecuteAsync<DefinitionSearchResult>(request, cancellationToken)
            .ConfigureAwait(false);

        EnsureSuccess(response);
        return response.Data!;
    }

    public async Task<Definition> PreviewDefinitionAsync(
        ComponentCoordinates coordinates,
        object curation,
        CancellationToken cancellationToken = default
    )
    {
        var request = new RestRequest($"definitions/{coordinates}", Method.Post).AddBody(curation);
        var response = await this
            .client.ExecuteAsync<Definition>(request, cancellationToken)
            .ConfigureAwait(false);

        EnsureSuccess(response);
        return response.Data!;
    }

    public async Task<Definition> PreviewDefinitionWithPrAsync(
        ComponentCoordinates coordinates,
        int pr,
        CancellationToken cancellationToken = default
    )
    {
        var request = new RestRequest($"definitions/{coordinates}/pr/{pr}");
        var response = await this
            .client.ExecuteAsync<Definition>(request, cancellationToken)
            .ConfigureAwait(false);

        EnsureSuccess(response);
        return response.Data!;
    }

    // ── Curations ────────────────────────────────────────────────────

    public async Task<JsonElement> GetCurationsListAsync(
        string type,
        string provider,
        string ns,
        string name,
        CancellationToken cancellationToken = default
    )
    {
        var request = new RestRequest($"curations/{type}/{provider}/{ns}/{name}");
        return await this.ExecuteJsonAsync(request, cancellationToken).ConfigureAwait(false);
    }

    public async Task<JsonElement> GetCurationAsync(
        ComponentCoordinates coordinates,
        CancellationToken cancellationToken = default
    )
    {
        var request = new RestRequest($"curations/{coordinates}");
        return await this.ExecuteJsonAsync(request, cancellationToken).ConfigureAwait(false);
    }

    public async Task<JsonElement> GetCurationWithPrAsync(
        ComponentCoordinates coordinates,
        int pr,
        CancellationToken cancellationToken = default
    )
    {
        var request = new RestRequest($"curations/{coordinates}/pr/{pr}");
        return await this.ExecuteJsonAsync(request, cancellationToken).ConfigureAwait(false);
    }

    public async Task<Dictionary<string, JsonElement>> GetCurationsBatchAsync(
        IEnumerable<string> components,
        CancellationToken cancellationToken = default
    )
    {
        var request = new RestRequest("curations", Method.Post).AddBody(components);
        var response = await this
            .client.ExecuteAsync(request, cancellationToken)
            .ConfigureAwait(false);

        EnsureSuccess(response);
        return JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(response.Content!)!;
    }

    public async Task<JsonElement> PatchCurationAsync(
        CurationContribution contribution,
        CancellationToken cancellationToken = default
    )
    {
        var request = new RestRequest("curations", Method.Patch).AddBody(contribution);
        return await this.ExecuteJsonAsync(request, cancellationToken).ConfigureAwait(false);
    }

    // ── Harvest ──────────────────────────────────────────────────────

    public async Task HarvestAsync(
        IEnumerable<HarvestRequest> requests,
        CancellationToken cancellationToken = default
    )
    {
        var request = new RestRequest("harvest", Method.Post).AddBody(requests);
        var response = await this
            .client.ExecuteAsync(request, cancellationToken)
            .ConfigureAwait(false);

        EnsureSuccess(response);
    }

    public async Task<JsonElement> GetHarvestAsync(
        ComponentCoordinates coordinates,
        HarvestForm? form = null,
        CancellationToken cancellationToken = default
    )
    {
        var request = new RestRequest($"harvest/{coordinates}");
        AddFormParameter(request, form);

        return await this.ExecuteJsonAsync(request, cancellationToken).ConfigureAwait(false);
    }

    public async Task<JsonElement> GetHarvestByToolAsync(
        ComponentCoordinates coordinates,
        string tool,
        HarvestForm? form = null,
        CancellationToken cancellationToken = default
    )
    {
        var request = new RestRequest($"harvest/{coordinates}/{tool}");
        AddFormParameter(request, form);

        return await this.ExecuteJsonAsync(request, cancellationToken).ConfigureAwait(false);
    }

    public async Task<JsonElement> GetHarvestByToolVersionAsync(
        ComponentCoordinates coordinates,
        string tool,
        string toolVersion,
        HarvestForm? form = null,
        CancellationToken cancellationToken = default
    )
    {
        var request = new RestRequest($"harvest/{coordinates}/{tool}/{toolVersion}");
        AddFormParameter(request, form);

        return await this.ExecuteJsonAsync(request, cancellationToken).ConfigureAwait(false);
    }

    public async Task PutHarvestAsync(
        ComponentCoordinates coordinates,
        string tool,
        string toolVersion,
        object content,
        CancellationToken cancellationToken = default
    )
    {
        var request = new RestRequest(
            $"harvest/{coordinates}/{tool}/{toolVersion}",
            Method.Put
        ).AddBody(content);

        var response = await this
            .client.ExecuteAsync(request, cancellationToken)
            .ConfigureAwait(false);

        EnsureSuccess(response);
    }

    // ── Attachments ──────────────────────────────────────────────────

    public async Task<string> GetAttachmentAsync(
        string id,
        CancellationToken cancellationToken = default
    )
    {
        var request = new RestRequest($"attachments/{id}");
        var response = await this
            .client.ExecuteAsync(request, cancellationToken)
            .ConfigureAwait(false);

        EnsureSuccess(response);
        return response.Content!;
    }

    // ── Notices ──────────────────────────────────────────────────────

    public async Task<NoticeFile> GenerateNoticeAsync(
        object request,
        CancellationToken cancellationToken = default
    )
    {
        var restRequest = new RestRequest("notices", Method.Post).AddBody(request);
        var response = await this
            .client.ExecuteAsync<NoticeFile>(restRequest, cancellationToken)
            .ConfigureAwait(false);

        EnsureSuccess(response);
        return response.Data!;
    }

    // ── Helpers ──────────────────────────────────────────────────────

    private static void AddFormParameter(RestRequest request, HarvestForm? form)
    {
        if (form is not null)
        {
            _ = request.AddQueryParameter("form", form.Value.ToApiString());
        }
    }

    private static void AddSearchParameters(
        RestRequest request,
        DefinitionSearchParameters parameters
    )
    {
        if (parameters.Pattern is not null)
        {
            _ = request.AddQueryParameter("pattern", parameters.Pattern);
        }

        if (parameters.Type is not null)
        {
            _ = request.AddQueryParameter("type", parameters.Type);
        }

        if (parameters.Provider is not null)
        {
            _ = request.AddQueryParameter("provider", parameters.Provider);
        }

        if (parameters.Name is not null)
        {
            _ = request.AddQueryParameter("name", parameters.Name);
        }

        if (parameters.Namespace is not null)
        {
            _ = request.AddQueryParameter("namespace", parameters.Namespace);
        }

        if (parameters.License is not null)
        {
            _ = request.AddQueryParameter("license", parameters.License);
        }

        if (parameters.ReleasedAfter is not null)
        {
            _ = request.AddQueryParameter("releasedAfter", parameters.ReleasedAfter);
        }

        if (parameters.ReleasedBefore is not null)
        {
            _ = request.AddQueryParameter("releasedBefore", parameters.ReleasedBefore);
        }

        if (parameters.MinLicensedScore is not null)
        {
            _ = request.AddQueryParameter(
                "minLicensedScore",
                parameters.MinLicensedScore.Value.ToString(CultureInfo.InvariantCulture)
            );
        }

        if (parameters.MaxLicensedScore is not null)
        {
            _ = request.AddQueryParameter(
                "maxLicensedScore",
                parameters.MaxLicensedScore.Value.ToString(CultureInfo.InvariantCulture)
            );
        }

        if (parameters.MinDescribedScore is not null)
        {
            _ = request.AddQueryParameter(
                "minDescribedScore",
                parameters.MinDescribedScore.Value.ToString(CultureInfo.InvariantCulture)
            );
        }

        if (parameters.MaxDescribedScore is not null)
        {
            _ = request.AddQueryParameter(
                "maxDescribedScore",
                parameters.MaxDescribedScore.Value.ToString(CultureInfo.InvariantCulture)
            );
        }

        if (parameters.Sort is not null)
        {
            _ = request.AddQueryParameter("sort", parameters.Sort);
        }

        if (parameters.SortDesc is not null)
        {
            _ = request.AddQueryParameter(
                "sortDesc",
                parameters.SortDesc.Value.ToString(CultureInfo.InvariantCulture)
            );
        }

        if (parameters.ContinuationToken is not null)
        {
            _ = request.AddQueryParameter("continuationToken", parameters.ContinuationToken);
        }
    }

    private async Task<JsonElement> ExecuteJsonAsync(
        RestRequest request,
        CancellationToken cancellationToken
    )
    {
        var response = await this
            .client.ExecuteAsync(request, cancellationToken)
            .ConfigureAwait(false);

        EnsureSuccess(response);
        return JsonSerializer.Deserialize<JsonElement>(response.Content!);
    }

    private static void EnsureSuccess(RestResponse response)
    {
        if (!response.IsSuccessful)
        {
            throw new HttpRequestException(
                $"Request failed with status code {response.StatusCode}: {response.Content}",
                null,
                response.StatusCode
            );
        }
    }
}
