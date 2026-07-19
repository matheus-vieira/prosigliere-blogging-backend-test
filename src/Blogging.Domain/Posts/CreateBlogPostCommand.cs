namespace Blogging.Domain.Posts;

/// <summary>
/// Contains the values required to create a blog post.
/// </summary>
public sealed record CreateBlogPostCommand
{
    /// <summary>
    /// Gets or initializes the post title.
    /// </summary>
    public required string Title { get; init; }

    /// <summary>
    /// Gets or initializes the post content.
    /// </summary>
    public required string Content { get; init; }
}
