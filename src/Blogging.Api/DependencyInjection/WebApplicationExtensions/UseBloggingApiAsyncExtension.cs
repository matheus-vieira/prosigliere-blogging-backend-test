using Blogging.Api.Endpoints;

namespace Blogging.Api.DependencyInjection;

/// <summary>
/// Configures the API startup, middleware, diagnostics, and endpoints.
/// </summary>
public static class UseBloggingApiAsyncExtension
{
    /// <summary>
    /// Initializes persistence and enables API middleware, health checks, and Swagger UI.
    /// </summary>
    /// <param name="application">The web application.</param>
    /// <param name="cancellationToken">Cancels startup initialization.</param>
    /// <returns>A task containing the same application for fluent composition.</returns>
    public static async Task<WebApplication> UseBloggingApiAsync(
        this WebApplication application,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(application);

        await application
            .UseBloggingAsync(cancellationToken)
            .ConfigureAwait(false);

        application.UseExceptionHandler();

        application.UseSwagger();
        application.UseSwaggerUI();

        application.MapHealthChecks("/health");
        application.MapBloggingEndpoints();
        return application;
    }
}
