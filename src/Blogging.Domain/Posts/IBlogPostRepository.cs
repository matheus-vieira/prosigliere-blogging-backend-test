using Blogging.Domain.Entities;
using Blogging.Domain.Specifications;

namespace Blogging.Domain.Posts;

/// <summary>
/// Defines persistence operations required by the posts use case.
/// </summary>
public interface IBlogPostRepository
{
    /// <summary>
    /// Lists post summaries with their comment counts.
    /// </summary>
    /// <param name="specification">The query specification.</param>
    /// <param name="cancellationToken">Cancels the query.</param>
    /// <returns>The persisted post summaries.</returns>
    Task<IReadOnlyList<BlogPostSummary>> ListAsync(
        ISpecification<BlogPost> specification,
        CancellationToken cancellationToken);

    /// <summary>
    /// Persists a post and returns its summary.
    /// </summary>
    /// <param name="command">The post values to persist.</param>
    /// <param name="cancellationToken">Cancels the write.</param>
    /// <returns>The created post summary.</returns>
    Task<BlogPostSummary> CreateAsync(
        CreateBlogPostCommand command,
        CancellationToken cancellationToken);

    /// <summary>
    /// Gets a post with its comments.
    /// </summary>
    /// <param name="specification">The query specification.</param>
    /// <param name="cancellationToken">Cancels the query.</param>
    /// <returns>The detail or null when the post does not exist.</returns>
    Task<BlogPostDetail?> GetByIdAsync(
        ISpecification<BlogPost> specification,
        CancellationToken cancellationToken);

    /// <summary>
    /// Persists a comment for an existing post.
    /// </summary>
    /// <param name="postId">The post identifier.</param>
    /// <param name="command">The comment values to persist.</param>
    /// <param name="cancellationToken">Cancels the write.</param>
    /// <returns>The created comment or null when the post does not exist.</returns>
    Task<CommentSummary?> CreateCommentAsync(
        int postId,
        CreateCommentCommand command,
        CancellationToken cancellationToken);

    /// <summary>
    /// Searches posts using a composable specification.
    /// </summary>
    /// <param name="specification">The query specification.</param>
    /// <param name="cancellationToken">Cancels the query.</param>
    /// <returns>Matching post summaries.</returns>
    Task<PagedResult<BlogPostSummary>> SearchAsync(
        ISpecification<BlogPost> specification,
        CancellationToken cancellationToken);
}
