using Blogging.Repository.Options;
using Blogging.Repository.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Blogging.Repository.DependencyInjection;

/// <summary>
/// Registers Repository services and configures the SQLite provider.
/// </summary>
public static class AddBloggingRepositoryExtension
{
    /// <summary>
    /// Adds the blogging database and validates its strongly typed options.
    /// </summary>
    /// <param name="services">The dependency injection service collection.</param>
    /// <param name="configuration">The application configuration.</param>
    /// <returns>The same service collection for fluent registration.</returns>
    public static IServiceCollection AddBloggingRepository(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        services.AddOptions<BlogDatabaseOptions>()
            .Bind(configuration.GetSection(BlogDatabaseOptions.SectionName))
            .Validate(
                options => !string.IsNullOrWhiteSpace(options.ConnectionString),
                "The blog database connection string is required.")
            .ValidateOnStart();

        services.AddDbContext<BlogDbContext>((serviceProvider, options) =>
        {
            var databaseOptions = serviceProvider
                .GetRequiredService<IOptions<BlogDatabaseOptions>>()
                .Value;

            options.UseSqlite(databaseOptions.ConnectionString);
        });

        return services;
    }
}
