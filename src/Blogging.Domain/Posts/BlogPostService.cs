namespace Blogging.Domain.Posts;

/// <summary>
/// Executes the application use cases for blog posts.
/// </summary>
public sealed class BlogPostService(IBlogPostRepository repository)
{
    /// <summary>
    /// Lists posts with their comment counts.
    /// </summary>
    /// <param name="cancellationToken">Cancels the query.</param>
    /// <returns>The post summaries.</returns>
    public Task<IReadOnlyList<BlogPostSummary>> ListAsync(
        CancellationToken cancellationToken)
    {
        return repository.ListAsync(cancellationToken);
    }

    /// <summary>
    /// Validates and creates a blog post.
    /// </summary>
    /// <param name="command">The requested post values.</param>
    /// <param name="cancellationToken">Cancels the write.</param>
    /// <returns>The created post summary.</returns>
    public Task<BlogPostSummary> CreateAsync(
        CreateBlogPostCommand command,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);
        ArgumentException.ThrowIfNullOrWhiteSpace(command.Title);
        ArgumentException.ThrowIfNullOrWhiteSpace(command.Content);

        return repository.CreateAsync(command, cancellationToken);
    }
}
