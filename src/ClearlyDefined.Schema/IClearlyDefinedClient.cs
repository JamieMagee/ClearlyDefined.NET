namespace ClearlyDefined.Schema;

using System.Text.Json;
using ClearlyDefined.Schema.Models;

/// <summary>
/// Client interface for the ClearlyDefined API.
/// </summary>
public interface IClearlyDefinedClient : IDisposable
{
    // Definitions
    public Task<Dictionary<string, Definition>> GetDefinitionsAsync(
        IEnumerable<string> components,
        CancellationToken cancellationToken = default
    );

    public Task<Definition> GetDefinitionAsync(
        ComponentCoordinates coordinates,
        string? expand = null,
        CancellationToken cancellationToken = default
    );

    public Task<DefinitionSearchResult> SearchDefinitionsAsync(
        DefinitionSearchParameters parameters,
        CancellationToken cancellationToken = default
    );

    public Task<Definition> PreviewDefinitionAsync(
        ComponentCoordinates coordinates,
        object curation,
        CancellationToken cancellationToken = default
    );

    public Task<Definition> PreviewDefinitionWithPrAsync(
        ComponentCoordinates coordinates,
        int pr,
        CancellationToken cancellationToken = default
    );

    // Curations
    public Task<JsonElement> GetCurationsListAsync(
        string type,
        string provider,
        string ns,
        string name,
        CancellationToken cancellationToken = default
    );

    public Task<JsonElement> GetCurationAsync(
        ComponentCoordinates coordinates,
        CancellationToken cancellationToken = default
    );

    public Task<JsonElement> GetCurationWithPrAsync(
        ComponentCoordinates coordinates,
        int pr,
        CancellationToken cancellationToken = default
    );

    public Task<Dictionary<string, JsonElement>> GetCurationsBatchAsync(
        IEnumerable<string> components,
        CancellationToken cancellationToken = default
    );

    public Task<JsonElement> PatchCurationAsync(
        CurationContribution contribution,
        CancellationToken cancellationToken = default
    );

    // Harvest
    public Task HarvestAsync(
        IEnumerable<HarvestRequest> requests,
        CancellationToken cancellationToken = default
    );

    public Task<JsonElement> GetHarvestAsync(
        ComponentCoordinates coordinates,
        HarvestForm? form = null,
        CancellationToken cancellationToken = default
    );

    public Task<JsonElement> GetHarvestByToolAsync(
        ComponentCoordinates coordinates,
        string tool,
        HarvestForm? form = null,
        CancellationToken cancellationToken = default
    );

    public Task<JsonElement> GetHarvestByToolVersionAsync(
        ComponentCoordinates coordinates,
        string tool,
        string toolVersion,
        HarvestForm? form = null,
        CancellationToken cancellationToken = default
    );

    public Task PutHarvestAsync(
        ComponentCoordinates coordinates,
        string tool,
        string toolVersion,
        object content,
        CancellationToken cancellationToken = default
    );

    // Attachments
    public Task<string> GetAttachmentAsync(
        string id,
        CancellationToken cancellationToken = default
    );

    // Notices
    public Task<NoticeFile> GenerateNoticeAsync(
        object request,
        CancellationToken cancellationToken = default
    );
}
