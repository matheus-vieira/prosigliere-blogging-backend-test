namespace Blogging.Api.Posts.Contracts;

/// <summary>
/// Represents the HTTP payload used to create a post.
/// </summary>
public sealed class CreatePostRequest
{
    /// <summary>
    /// Gets or sets the post title.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets the post content.
    /// </summary>
    public string? Content { get; set; }
}
