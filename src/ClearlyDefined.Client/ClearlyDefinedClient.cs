namespace ClearlyDefined.Client;

using System.Globalization;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection;
using System.Text.Json;
using ClearlyDefined.Client.Models;

/// <summary>
/// HTTP client for the ClearlyDefined REST API.
/// </summary>
public sealed class ClearlyDefinedClient : IClearlyDefinedClient
{
    private static readonly Version? version = typeof(ClearlyDefinedClient)
        .GetTypeInfo()
        .Assembly.GetName()
        .Version;

    private readonly HttpClient httpClient;

    /// <summary>
    /// Initializes a new instance using the provided <see cref="HttpClient"/>.
    /// Intended for use with <c>IHttpClientFactory</c> / DI.
    /// </summary>
    public ClearlyDefinedClient(HttpClient httpClient)
    {
        this.httpClient = httpClient;
        this.httpClient.BaseAddress ??= new Uri("https://api.clearlydefined.io/");

        if (string.IsNullOrEmpty(this.httpClient.DefaultRequestHeaders.UserAgent.ToString()))
        {
            this.httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(
                $"ClearlyDefined.NET/{version}"
            );
        }
    }

    /// <summary>
    /// Initializes a new instance with default settings for standalone usage.
    /// </summary>
    public ClearlyDefinedClient()
        : this(new HttpClient()) { }

    // ── Definitions ──────────────────────────────────────────────────

    public async Task<Dictionary<string, Definition>> GetDefinitionsAsync(
        IEnumerable<string> components,
        CancellationToken cancellationToken = default
    )
    {
        using var response = await this
            .httpClient.PostAsJsonAsync("definitions", components, cancellationToken)
            .ConfigureAwait(false);

        _ = response.EnsureSuccessStatusCode();
        return (
            await response
                .Content.ReadFromJsonAsync<Dictionary<string, Definition>>(
                    (JsonSerializerOptions?)null,
                    cancellationToken
                )
                .ConfigureAwait(false)
        )!;
    }

    public async Task<Definition> GetDefinitionAsync(
        ComponentCoordinates coordinates,
        string? expand = null,
        CancellationToken cancellationToken = default
    )
    {
        var url = $"definitions/{coordinates}";
        if (expand is not null)
        {
            url += $"?expand={Uri.EscapeDataString(expand)}";
        }

        return (
            await this
                .httpClient.GetFromJsonAsync<Definition>(url, cancellationToken)
                .ConfigureAwait(false)
        )!;
    }

    public async Task<DefinitionSearchResult> SearchDefinitionsAsync(
        DefinitionSearchParameters parameters,
        CancellationToken cancellationToken = default
    )
    {
        var url = "definitions" + BuildSearchQuery(parameters);
        return (
            await this
                .httpClient.GetFromJsonAsync<DefinitionSearchResult>(url, cancellationToken)
                .ConfigureAwait(false)
        )!;
    }

    public async Task<Definition> PreviewDefinitionAsync(
        ComponentCoordinates coordinates,
        object curation,
        CancellationToken cancellationToken = default
    )
    {
        using var response = await this
            .httpClient.PostAsJsonAsync($"definitions/{coordinates}", curation, cancellationToken)
            .ConfigureAwait(false);

        _ = response.EnsureSuccessStatusCode();
        return (
            await response
                .Content.ReadFromJsonAsync<Definition>(
                    (JsonSerializerOptions?)null,
                    cancellationToken
                )
                .ConfigureAwait(false)
        )!;
    }

    public async Task<Definition> PreviewDefinitionWithPrAsync(
        ComponentCoordinates coordinates,
        int pr,
        CancellationToken cancellationToken = default
    ) =>
        (
            await this
                .httpClient.GetFromJsonAsync<Definition>(
                    $"definitions/{coordinates}/pr/{pr}",
                    cancellationToken
                )
                .ConfigureAwait(false)
        )!;

    // ── Curations ────────────────────────────────────────────────────

    public async Task<JsonElement> GetCurationsListAsync(
        string type,
        string provider,
        string ns,
        string name,
        CancellationToken cancellationToken = default
    ) =>
        await this.GetJsonAsync($"curations/{type}/{provider}/{ns}/{name}", cancellationToken)
            .ConfigureAwait(false);

    public async Task<JsonElement> GetCurationAsync(
        ComponentCoordinates coordinates,
        CancellationToken cancellationToken = default
    ) =>
        await this.GetJsonAsync($"curations/{coordinates}", cancellationToken)
            .ConfigureAwait(false);

    public async Task<JsonElement> GetCurationWithPrAsync(
        ComponentCoordinates coordinates,
        int pr,
        CancellationToken cancellationToken = default
    ) =>
        await this.GetJsonAsync($"curations/{coordinates}/pr/{pr}", cancellationToken)
            .ConfigureAwait(false);

    public async Task<Dictionary<string, JsonElement>> GetCurationsBatchAsync(
        IEnumerable<string> components,
        CancellationToken cancellationToken = default
    )
    {
        using var response = await this
            .httpClient.PostAsJsonAsync("curations", components, cancellationToken)
            .ConfigureAwait(false);

        _ = response.EnsureSuccessStatusCode();
        return (
            await response
                .Content.ReadFromJsonAsync<Dictionary<string, JsonElement>>(
                    (JsonSerializerOptions?)null,
                    cancellationToken
                )
                .ConfigureAwait(false)
        )!;
    }

    public async Task<JsonElement> PatchCurationAsync(
        CurationContribution contribution,
        CancellationToken cancellationToken = default
    )
    {
        using var request = new HttpRequestMessage(HttpMethod.Patch, "curations");
        request.Content = JsonContent.Create(contribution);

        using var response = await this
            .httpClient.SendAsync(request, cancellationToken)
            .ConfigureAwait(false);

        _ = response.EnsureSuccessStatusCode();
        return await response
            .Content.ReadFromJsonAsync<JsonElement>((JsonSerializerOptions?)null, cancellationToken)
            .ConfigureAwait(false);
    }

    // ── Harvest ──────────────────────────────────────────────────────

    public async Task HarvestAsync(
        IEnumerable<HarvestRequest> requests,
        CancellationToken cancellationToken = default
    )
    {
        using var response = await this
            .httpClient.PostAsJsonAsync("harvest", requests, cancellationToken)
            .ConfigureAwait(false);

        _ = response.EnsureSuccessStatusCode();
    }

    public async Task<JsonElement> GetHarvestAsync(
        ComponentCoordinates coordinates,
        HarvestForm? form = null,
        CancellationToken cancellationToken = default
    ) =>
        await this.GetJsonAsync($"harvest/{coordinates}{FormQuery(form)}", cancellationToken)
            .ConfigureAwait(false);

    public async Task<JsonElement> GetHarvestByToolAsync(
        ComponentCoordinates coordinates,
        string tool,
        HarvestForm? form = null,
        CancellationToken cancellationToken = default
    ) =>
        await this.GetJsonAsync($"harvest/{coordinates}/{tool}{FormQuery(form)}", cancellationToken)
            .ConfigureAwait(false);

    public async Task<JsonElement> GetHarvestByToolVersionAsync(
        ComponentCoordinates coordinates,
        string tool,
        string toolVersion,
        HarvestForm? form = null,
        CancellationToken cancellationToken = default
    ) =>
        await this.GetJsonAsync(
                $"harvest/{coordinates}/{tool}/{toolVersion}{FormQuery(form)}",
                cancellationToken
            )
            .ConfigureAwait(false);

    public async Task PutHarvestAsync(
        ComponentCoordinates coordinates,
        string tool,
        string toolVersion,
        object content,
        CancellationToken cancellationToken = default
    )
    {
        using var request = new HttpRequestMessage(
            HttpMethod.Put,
            $"harvest/{coordinates}/{tool}/{toolVersion}"
        );
        request.Content = JsonContent.Create(content);

        using var response = await this
            .httpClient.SendAsync(request, cancellationToken)
            .ConfigureAwait(false);

        _ = response.EnsureSuccessStatusCode();
    }

    // ── Attachments ──────────────────────────────────────────────────

    public async Task<string> GetAttachmentAsync(
        string id,
        CancellationToken cancellationToken = default
    )
    {
        using var response = await this
            .httpClient.GetAsync($"attachments/{id}", cancellationToken)
            .ConfigureAwait(false);

        _ = response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
    }

    // ── Notices ──────────────────────────────────────────────────────

    public async Task<NoticeFile> GenerateNoticeAsync(
        object request,
        CancellationToken cancellationToken = default
    )
    {
        using var response = await this
            .httpClient.PostAsJsonAsync("notices", request, cancellationToken)
            .ConfigureAwait(false);

        _ = response.EnsureSuccessStatusCode();
        return (
            await response
                .Content.ReadFromJsonAsync<NoticeFile>(
                    (JsonSerializerOptions?)null,
                    cancellationToken
                )
                .ConfigureAwait(false)
        )!;
    }

    // ── Helpers ──────────────────────────────────────────────────────

    private static string FormQuery(HarvestForm? form) =>
        form is not null ? $"?form={form.Value.ToApiString()}" : string.Empty;

    private async Task<JsonElement> GetJsonAsync(string url, CancellationToken cancellationToken) =>
        await this
            .httpClient.GetFromJsonAsync<JsonElement>(url, cancellationToken)
            .ConfigureAwait(false);

    private static string BuildSearchQuery(DefinitionSearchParameters p)
    {
        var parts = new List<string>();

        if (p.Pattern is not null)
        {
            parts.Add($"pattern={Uri.EscapeDataString(p.Pattern)}");
        }

        if (p.Type is not null)
        {
            parts.Add($"type={Uri.EscapeDataString(p.Type)}");
        }

        if (p.Provider is not null)
        {
            parts.Add($"provider={Uri.EscapeDataString(p.Provider)}");
        }

        if (p.Name is not null)
        {
            parts.Add($"name={Uri.EscapeDataString(p.Name)}");
        }

        if (p.Namespace is not null)
        {
            parts.Add($"namespace={Uri.EscapeDataString(p.Namespace)}");
        }

        if (p.License is not null)
        {
            parts.Add($"license={Uri.EscapeDataString(p.License)}");
        }

        if (p.ReleasedAfter is not null)
        {
            parts.Add($"releasedAfter={Uri.EscapeDataString(p.ReleasedAfter)}");
        }

        if (p.ReleasedBefore is not null)
        {
            parts.Add($"releasedBefore={Uri.EscapeDataString(p.ReleasedBefore)}");
        }

        if (p.MinLicensedScore is not null)
        {
            parts.Add(
                $"minLicensedScore={p.MinLicensedScore.Value.ToString(CultureInfo.InvariantCulture)}"
            );
        }

        if (p.MaxLicensedScore is not null)
        {
            parts.Add(
                $"maxLicensedScore={p.MaxLicensedScore.Value.ToString(CultureInfo.InvariantCulture)}"
            );
        }

        if (p.MinDescribedScore is not null)
        {
            parts.Add(
                $"minDescribedScore={p.MinDescribedScore.Value.ToString(CultureInfo.InvariantCulture)}"
            );
        }

        if (p.MaxDescribedScore is not null)
        {
            parts.Add(
                $"maxDescribedScore={p.MaxDescribedScore.Value.ToString(CultureInfo.InvariantCulture)}"
            );
        }

        if (p.Sort is not null)
        {
            parts.Add($"sort={Uri.EscapeDataString(p.Sort)}");
        }

        if (p.SortDesc is not null)
        {
            parts.Add($"sortDesc={p.SortDesc.Value.ToString(CultureInfo.InvariantCulture)}");
        }

        if (p.ContinuationToken is not null)
        {
            parts.Add($"continuationToken={Uri.EscapeDataString(p.ContinuationToken)}");
        }

        return parts.Count > 0 ? "?" + string.Join("&", parts) : string.Empty;
    }
}
