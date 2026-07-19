using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Blogging.Repository.Persistence;

/// <summary>
/// Creates the context for EF Core design-time migration commands.
/// </summary>
public sealed class BlogDbContextFactory : IDesignTimeDbContextFactory<BlogDbContext>
{
    /// <param name="args">Design-time command arguments.</param>
    /// <returns>A configured EF Core database context.</returns>
    public BlogDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<BlogDbContext>()
            .UseSqlite("Data Source=blogging.db")
            .Options;

        return new BlogDbContext(options);
    }
}
