using Blogging.Domain.Entities;

namespace Blogging.Domain.Posts;

using Blogging.Domain.Validation;

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
        return repository.ListAsync(
            PostSpecifications.FromFilter(new PostFilter()),
            cancellationToken);
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
        var normalizedCommand = command with
        {
            Title = PostValidationRules.NormalizeTitle(command.Title),
            Content = PostValidationRules.NormalizeContent(command.Content)
        };

        return repository.CreateAsync(normalizedCommand, cancellationToken);
    }

    /// <summary>
    /// Gets a post with its comments.
    /// </summary>
    /// <param name="postId">The post identifier.</param>
    /// <param name="cancellationToken">Cancels the query.</param>
    /// <returns>The post detail or null when it does not exist.</returns>
    public Task<BlogPostDetail?> GetByIdAsync(
        int postId,
        CancellationToken cancellationToken)
    {
        return repository.GetByIdAsync(
            PostSpecifications.FromFilter(new PostFilter { Id = postId }),
            cancellationToken);
    }

    /// <summary>
    /// Validates and creates a comment for a post.
    /// </summary>
    /// <param name="postId">The post identifier.</param>
    /// <param name="command">The requested comment values.</param>
    /// <param name="cancellationToken">Cancels the write.</param>
    /// <returns>The created comment or null when the post does not exist.</returns>
    public Task<CommentSummary?> CreateCommentAsync(
        int postId,
        CreateCommentCommand command,
        CancellationToken cancellationToken)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(postId);
        ArgumentNullException.ThrowIfNull(command);
        var normalizedCommand = command with
        {
            Content = PostValidationRules.NormalizeCommentContent(command.Content)
        };

        return repository.CreateCommentAsync(postId, normalizedCommand, cancellationToken);
    }
}
