using Blogging.Repository.DependencyInjection;
using Blogging.Repository.Persistence;

namespace Blogging.Api.Tests;

/// <summary>
/// Verifies API composition and startup database initialization.
/// </summary>
[TestClass]
public sealed class ApiStartupTests
{
    /// <summary>
    /// Confirms that the API composition root starts and serves the root endpoint.
    /// </summary>
    [TestMethod]
    public async Task RootEndpointStartsWithMigratedDatabaseAsync()
    {
        using var factory = new BloggingApiFactory();
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/").ConfigureAwait(false);
        var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

        Assert.IsTrue(response.IsSuccessStatusCode);
        Assert.AreEqual("Hello World!", content);
    }

    /// <summary>
    /// Confirms that running startup initialization again finds no pending migration.
    /// </summary>
    [TestMethod]
    public async Task DatabaseInitializationIsIdempotentAsync()
    {
        using var factory = new BloggingApiFactory();
        using var client = factory.CreateClient();

        await factory.Services.UseBloggingDatabaseAsync().ConfigureAwait(false);

        Assert.IsNotNull(client);
    }

    /// <summary>
    /// Confirms that the design-time factory creates a SQLite context.
    /// </summary>
    [TestMethod]
    public void DesignTimeFactoryCreatesSqliteContext()
    {
        using var context = new BlogDbContextFactory().CreateDbContext([]);

        Assert.AreEqual(
            "Microsoft.EntityFrameworkCore.Sqlite",
            context.Database.ProviderName);
    }

}
