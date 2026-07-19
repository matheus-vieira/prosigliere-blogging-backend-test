using Blogging.Domain.Posts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Blogging.Domain.DependencyInjection;

/// <summary>
/// Registers services owned by the Domain layer.
/// </summary>
public static class AddBloggingDomainExtension
{
    /// <summary>
    /// Adds Domain services and delegates infrastructure registration.
    /// </summary>
    /// <param name="services">The dependency injection service collection.</param>
    /// <param name="configuration">The application configuration.</param>
    /// <param name="registerRepository">The Repository registration callback.</param>
    /// <returns>The same service collection for fluent registration.</returns>
    public static IServiceCollection AddBloggingDomain(
        this IServiceCollection services,
        IConfiguration configuration,
        Action<IServiceCollection, IConfiguration> registerRepository)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentNullException.ThrowIfNull(registerRepository);

        services.AddScoped<BlogPostService>();
        services.AddScoped<PostSearchService>();
        registerRepository(services, configuration);
        return services;
    }
}
