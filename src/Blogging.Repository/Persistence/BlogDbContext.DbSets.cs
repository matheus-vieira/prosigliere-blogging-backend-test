using Blogging.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Blogging.Repository.Persistence;

public sealed partial class BlogDbContext
{
    /// <summary>
    /// Gets the blog post entity set.
    /// </summary>
    public DbSet<BlogPost> BlogPosts => Set<BlogPost>();

    /// <summary>
    /// Gets the comment entity set.
    /// </summary>
    public DbSet<Comment> Comments => Set<Comment>();
}
