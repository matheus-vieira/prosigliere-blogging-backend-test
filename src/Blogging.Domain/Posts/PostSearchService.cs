namespace Blogging.Domain.Posts;

/// <summary>
/// Validates post search filters and composes persistence specifications.
/// </summary>
public sealed class PostSearchService(IBlogPostRepository repository)
{
    /// <summary>
    /// Searches posts using every supplied filter.
    /// </summary>
    /// <param name="query">The optional search filters.</param>
    /// <param name="cancellationToken">Cancels the query.</param>
    /// <returns>Matching post summaries.</returns>
    public Task<PagedResult<BlogPostSummary>> SearchAsync(
        PostFilter query,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        if (query.MinCommentCount is < 0 || query.MaxCommentCount is < 0)
        {
            throw new ArgumentException("Comment counts cannot be negative.", nameof(query));
        }

        if (query.MinCommentCount > query.MaxCommentCount)
        {
            throw new ArgumentException(
                "Minimum comment count cannot exceed maximum comment count.",
                nameof(query));
        }

        if (query.Page < 1 || query.PageSize is < 1 or > 100)
        {
            throw new ArgumentException("Page must be positive and page size must be between 1 and 100.", nameof(query));
        }

        return repository.SearchAsync(
            PostSpecifications.FromFilter(query),
            cancellationToken);
    }
}
