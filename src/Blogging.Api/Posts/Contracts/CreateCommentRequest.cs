namespace Blogging.Api.Posts.Contracts;

/// <summary>
/// Represents the HTTP payload used to create a comment.
/// </summary>
public sealed class CreateCommentRequest
{
    /// <summary>
    /// Gets or sets the comment content.
    /// </summary>
    public string? Content { get; set; }
}
