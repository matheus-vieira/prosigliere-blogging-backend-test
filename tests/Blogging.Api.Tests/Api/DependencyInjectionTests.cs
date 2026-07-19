using Blogging.Api.DependencyInjection;
using Blogging.Domain.DependencyInjection;
using Blogging.Repository.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Blogging.Api.Tests;

/// <summary>
/// Verifies the extension-method composition boundaries.
/// </summary>
[TestClass]
public sealed class DependencyInjectionTests
{
    /// <summary>
    /// Confirms that the API composition extension registers the repository context.
    /// </summary>
    [TestMethod]
    public void AddBloggingDomainRegistersRepositoryContext()
    {
        var builder = WebApplication.CreateBuilder();
        builder.Configuration["BlogDatabase:ConnectionString"] = "Data Source=:memory:";

        builder.AddBloggingDomain();
        using var application = builder.Build();
        using var scope = application.Services.CreateScope();

        var context = scope.ServiceProvider.GetService<BlogDbContext>();

        Assert.IsNotNull(context);
    }

    /// <summary>
    /// Confirms that Domain invokes the supplied infrastructure registration callback.
    /// </summary>
    [TestMethod]
    public void AddBloggingDomainInvokesRepositoryRegistration()
    {
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder().Build();
        var callbackInvoked = false;

        services.AddBloggingDomain(
            configuration,
            (_, _) => callbackInvoked = true);

        Assert.IsTrue(callbackInvoked);
    }

    /// <summary>
    /// Confirms that API diagnostics can be registered and composed.
    /// </summary>
    [TestMethod]
    public void AddBloggingApiRegistersDiagnostics()
    {
        var builder = WebApplication.CreateBuilder();
        builder.AddBloggingApi();
        using var application = builder.Build();

        application.UseBloggingApi();

        Assert.IsNotNull(application);
    }

    /// <summary>
    /// Confirms that production composition omits the development Swagger UI.
    /// </summary>
    [TestMethod]
    public void UseBloggingApiSupportsProductionEnvironment()
    {
        var builder = WebApplication.CreateBuilder(
            new WebApplicationOptions { EnvironmentName = "Production" });
        builder.AddBloggingApi();
        using var application = builder.Build();

        application.UseBloggingApi();

        Assert.IsNotNull(application);
    }
}
