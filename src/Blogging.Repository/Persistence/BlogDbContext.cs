using Blogging.Domain.Entities;
using Blogging.Repository.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Blogging.Repository.Persistence;

/// <summary>
/// EF Core session for the blogging persistence model.
/// </summary>
public sealed class BlogDbContext(DbContextOptions<BlogDbContext> options) : DbContext(options)
{
    /// <summary>
    /// Gets the blog post entity set.
    /// </summary>
    public DbSet<BlogPost> BlogPosts => Set<BlogPost>();

    /// <summary>
    /// Gets the comment entity set.
    /// </summary>
    public DbSet<Comment> Comments => Set<Comment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        IEntityTypeConfigurator[] configurators =
        [
            new BlogPostEntityConfigurator(),
            new CommentEntityConfigurator()
        ];

        foreach (var configurator in configurators)
        {
            configurator.Configure(modelBuilder);
        }
    }
}
