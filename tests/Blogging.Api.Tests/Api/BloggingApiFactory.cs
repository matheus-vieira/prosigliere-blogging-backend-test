using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Blogging.Api.Tests;

internal sealed class BloggingApiFactory : WebApplicationFactory<Program>
{
    private readonly string databasePath = Path.Combine(
        Path.GetTempPath(),
        $"blogging-api-{Guid.NewGuid():N}.db");

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");
        builder.UseSetting(
            "BlogDatabase:ConnectionString",
            $"Data Source={databasePath}");
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (disposing && File.Exists(databasePath))
        {
            File.Delete(databasePath);
        }
    }
}
