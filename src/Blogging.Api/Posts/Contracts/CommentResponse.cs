namespace Blogging.Api.Posts.Contracts;

/// <summary>
/// Represents a comment returned by the API.
/// </summary>
public sealed record CommentResponse(
    int Id,
    int PostId,
    string Content);
