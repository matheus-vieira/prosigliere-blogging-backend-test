using Blogging.Api.ErrorHandling;
using Microsoft.OpenApi;

namespace Blogging.Api.DependencyInjection;

/// <summary>
/// Registers cross-cutting API services.
/// </summary>
public static class AddBloggingApiExtension
{
    /// <summary>
    /// Adds health checks, exception handling, and OpenAPI generation.
    /// </summary>
    /// <param name="builder">The web application builder.</param>
    /// <returns>The same builder for fluent composition.</returns>
    public static WebApplicationBuilder AddBloggingApi(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Services.AddHealthChecks();
        builder.Services.AddProblemDetails();
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Blogging Backend API",
                Version = "v1",
                Description = "REST API for blog posts and comments."
            });
        });

        return builder;
    }
}
