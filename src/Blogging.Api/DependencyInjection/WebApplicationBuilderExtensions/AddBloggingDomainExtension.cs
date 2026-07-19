using Blogging.Domain.DependencyInjection;
using Blogging.Repository.DependencyInjection;

namespace Blogging.Api.DependencyInjection;

/// <summary>
/// Composes application services from the API composition root.
/// </summary>
public static class AddBloggingDomainExtension
{
    /// <summary>
    /// Adds Domain services and delegates Repository registration.
    /// </summary>
    /// <param name="builder">The web application builder.</param>
    /// <returns>The same builder for fluent composition.</returns>
    public static WebApplicationBuilder AddBloggingDomain(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Services.AddBloggingDomain(
            builder.Configuration,
            static (services, configuration) =>
                services.AddBloggingRepository(configuration));

        return builder;
    }
}
