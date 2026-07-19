namespace Blogging.Domain.Posts;

/// <summary>
/// Contains the values required to create a comment.
/// </summary>
public sealed record CreateCommentCommand
{
    /// <summary>
    /// Gets or initializes the comment content.
    /// </summary>
    public required string Content { get; init; }
}
