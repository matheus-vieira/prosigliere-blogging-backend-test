using Microsoft.EntityFrameworkCore;

namespace Blogging.Repository.Persistence;

/// <summary>
/// EF Core session for the blogging persistence model.
/// </summary>
public sealed partial class BlogDbContext(DbContextOptions<BlogDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BlogDbContext).Assembly);
    }
}
