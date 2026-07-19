namespace Blogging.Domain.Posts;

/// <summary>
/// Represents a post with its associated comments.
/// </summary>
public sealed record BlogPostDetail(
    int Id,
    string Title,
    string Content,
    IReadOnlyList<CommentSummary> Comments);
