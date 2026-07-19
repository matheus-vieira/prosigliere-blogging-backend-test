using Blogging.Domain.Entities;
using Blogging.Domain.Posts;
using Blogging.Repository.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Blogging.Repository.Posts;

/// <summary>
/// Persists and queries blog posts through EF Core.
/// </summary>
public sealed class BlogPostRepository(BlogDbContext context) : IBlogPostRepository
{
    /// <inheritdoc />
    public async Task<IReadOnlyList<BlogPostSummary>> ListAsync(
        CancellationToken cancellationToken)
    {
        return await context.BlogPosts
            .AsNoTracking()
            .OrderBy(post => post.Id)
            .Select(post => new BlogPostSummary(
                post.Id,
                post.Title,
                post.Comments.Count))
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<BlogPostSummary> CreateAsync(
        CreateBlogPostCommand command,
        CancellationToken cancellationToken)
    {
        var post = new BlogPost(command.Title, command.Content);
        context.BlogPosts.Add(post);
        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return new BlogPostSummary(post.Id, post.Title, 0);
    }
}
