namespace Blogging.Api.Posts.Contracts;

/// <summary>
/// Represents a post item returned by the list endpoint.
/// </summary>
public sealed record PostListItemResponse(
    int Id,
    string Title,
    int CommentCount);
