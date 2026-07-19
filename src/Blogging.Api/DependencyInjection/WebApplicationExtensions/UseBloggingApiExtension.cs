namespace Blogging.Api.DependencyInjection;

/// <summary>
/// Configures the API middleware and diagnostic endpoints.
/// </summary>
public static class UseBloggingApiExtension
{
    /// <summary>
    /// Enables exception handling, health checks, and development Swagger UI.
    /// </summary>
    /// <param name="application">The web application.</param>
    /// <returns>The same application for fluent composition.</returns>
    public static WebApplication UseBloggingApi(this WebApplication application)
    {
        ArgumentNullException.ThrowIfNull(application);

        application.UseExceptionHandler();

        if (application.Environment.IsDevelopment())
        {
            application.UseSwagger();
            application.UseSwaggerUI();
        }

        application.MapHealthChecks("/health");
        return application;
    }
}
