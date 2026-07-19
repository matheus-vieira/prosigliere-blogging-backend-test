using Blogging.Domain.Entities;
using Blogging.Domain.Posts;
using Blogging.Domain.Specifications;
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
        ISpecification<BlogPost> specification,
        CancellationToken cancellationToken)
    {
        return await context.BlogPosts
            .AsNoTracking()
            .Where(specification.Criteria)
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

    /// <inheritdoc />
    public async Task<BlogPostDetail?> GetByIdAsync(
        ISpecification<BlogPost> specification,
        CancellationToken cancellationToken)
    {
        var post = await context.BlogPosts
            .AsNoTracking()
            .Include(item => item.Comments)
            .Where(specification.Criteria)
            .SingleOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);

        return post is null
            ? null
            : new BlogPostDetail(
                post.Id,
                post.Title,
                post.Content,
                post.Comments
                    .OrderBy(comment => comment.Id)
                    .Select(comment => new CommentSummary(
                        comment.Id,
                        comment.PostId,
                        comment.Content))
                    .ToArray());
    }

    /// <inheritdoc />
    public async Task<CommentSummary?> CreateCommentAsync(
        int postId,
        CreateCommentCommand command,
        CancellationToken cancellationToken)
    {
        var postExists = await context.BlogPosts
            .AsNoTracking()
            .AnyAsync(post => post.Id == postId, cancellationToken)
            .ConfigureAwait(false);

        if (!postExists)
        {
            return null;
        }

        var comment = new Comment(postId, command.Content);
        context.Comments.Add(comment);
        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return new CommentSummary(comment.Id, comment.PostId, comment.Content);
    }

    /// <inheritdoc />
    public async Task<PagedResult<BlogPostSummary>> SearchAsync(
        ISpecification<BlogPost> specification,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(specification);

        var query = context.BlogPosts
            .AsNoTracking()
            .Where(specification.Criteria);
        var totalCount = await query.CountAsync(cancellationToken).ConfigureAwait(false);
        IOrderedQueryable<BlogPost>? orderedQuery = null;

        foreach (var ordering in specification.Orderings)
        {
            orderedQuery = orderedQuery is null
                ? ordering.Descending
                    ? query.OrderByDescending(ordering.KeySelector)
                    : query.OrderBy(ordering.KeySelector)
                : ordering.Descending
                    ? orderedQuery.ThenByDescending(ordering.KeySelector)
                    : orderedQuery.ThenBy(ordering.KeySelector);
        }

        var pagedQuery = (orderedQuery ?? query.OrderBy(post => post.Id))
            .Skip((specification.PageNumber - 1) * specification.PageSize)
            .Take(specification.PageSize);
        var items = await pagedQuery
            .Select(post => new BlogPostSummary(
                post.Id,
                post.Title,
                post.Comments.Count))
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        return new PagedResult<BlogPostSummary>(
            items,
            specification.PageNumber,
            specification.PageSize,
            totalCount);
    }
}
