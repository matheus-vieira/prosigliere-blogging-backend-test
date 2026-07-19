using Blogging.Repository.DependencyInjection;

namespace Blogging.Api.DependencyInjection;

/// <summary>
/// Composes application lifecycle operations from the API layer.
/// </summary>
public static class UseBloggingAsyncExtension
{
    /// <summary>
    /// Starts the blogging application and prepares its infrastructure.
    /// </summary>
    /// <param name="application">The web application.</param>
    /// <param name="cancellationToken">Cancels the migration operation.</param>
    /// <returns>A task representing the database initialization operation.</returns>
    public static async Task UseBloggingAsync(
        this WebApplication application,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(application);

        await application.Services
            .UseBloggingDatabaseAsync(cancellationToken)
            .ConfigureAwait(false);
    }
}
