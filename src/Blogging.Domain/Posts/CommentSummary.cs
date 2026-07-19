namespace Blogging.Domain.Posts;

/// <summary>
/// Represents a comment returned by a post detail use case.
/// </summary>
public sealed record CommentSummary(
    int Id,
    int PostId,
    string Content);
