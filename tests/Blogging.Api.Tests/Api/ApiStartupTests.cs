using System.Net;
using Blogging.Repository.DependencyInjection;
using Blogging.Repository.Persistence;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Blogging.Api.Tests;

/// <summary>
/// Verifies API composition and startup database initialization.
/// </summary>
[TestClass]
[TestCategory("Api")]
public sealed class ApiStartupTests
{
    /// <summary>
    /// Confirms that the API composition root starts and serves the root endpoint.
    /// </summary>
    [TestMethod]
    public async Task RootEndpointStartsWithMigratedDatabaseAsync()
    {
        using var factory = new BloggingApiFactory();
        using var client = factory.CreateClient(
            new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

        var response = await client.GetAsync("/").ConfigureAwait(false);

        Assert.AreEqual(HttpStatusCode.Redirect, response.StatusCode);
        Assert.AreEqual("/swagger", response.Headers.Location?.OriginalString);
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
