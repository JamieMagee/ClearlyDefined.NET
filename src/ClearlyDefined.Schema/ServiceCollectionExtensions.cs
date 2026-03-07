namespace ClearlyDefined.Schema;

using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods for registering <see cref="IClearlyDefinedClient"/> with dependency injection.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers <see cref="ClearlyDefinedClient"/> as a typed <see cref="HttpClient"/>
    /// and binds it to <see cref="IClearlyDefinedClient"/>.
    /// </summary>
    public static IHttpClientBuilder AddClearlyDefinedClient(
        this IServiceCollection services,
        Action<HttpClient>? configure = null
    ) =>
        services.AddHttpClient<IClearlyDefinedClient, ClearlyDefinedClient>(httpClient =>
            configure?.Invoke(httpClient)
        );
}
