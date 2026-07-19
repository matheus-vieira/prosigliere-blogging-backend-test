using Blogging.Repository.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Blogging.Repository.DependencyInjection;

/// <summary>
/// Provides database lifecycle operations for the application composition root.
/// </summary>
public static class UseBloggingDatabaseAsyncExtension
{
    /// <summary>
    /// Applies pending migrations when the database schema is behind the model.
    /// </summary>
    /// <param name="services">The application's service provider.</param>
    /// <param name="cancellationToken">Cancels the migration operation.</param>
    /// <returns>A task representing the migration operation.</returns>
    public static async Task UseBloggingDatabaseAsync(
        this IServiceProvider services,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(services);

        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<BlogDbContext>();
        var pendingMigrations = await context.Database
            .GetPendingMigrationsAsync(cancellationToken)
            .ConfigureAwait(false);

        if (!pendingMigrations.Any())
        {
            return;
        }

        await context.Database.MigrateAsync(cancellationToken).ConfigureAwait(false);
    }
}
