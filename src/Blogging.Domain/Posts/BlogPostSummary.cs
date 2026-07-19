namespace Blogging.Domain.Posts;

/// <summary>
/// Represents the summary returned when posts are listed.
/// </summary>
public sealed record BlogPostSummary(
    int Id,
    string Title,
    int CommentCount);
