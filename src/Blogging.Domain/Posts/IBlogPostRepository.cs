namespace Blogging.Domain.Posts;

/// <summary>
/// Defines persistence operations required by the posts use case.
/// </summary>
public interface IBlogPostRepository
{
    /// <summary>
    /// Lists post summaries with their comment counts.
    /// </summary>
    /// <param name="cancellationToken">Cancels the query.</param>
    /// <returns>The persisted post summaries.</returns>
    Task<IReadOnlyList<BlogPostSummary>> ListAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Persists a post and returns its summary.
    /// </summary>
    /// <param name="command">The post values to persist.</param>
    /// <param name="cancellationToken">Cancels the write.</param>
    /// <returns>The created post summary.</returns>
    Task<BlogPostSummary> CreateAsync(
        CreateBlogPostCommand command,
        CancellationToken cancellationToken);
}
