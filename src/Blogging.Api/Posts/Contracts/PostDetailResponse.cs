namespace Blogging.Api.Posts.Contracts;

/// <summary>
/// Represents a detailed post response.
/// </summary>
public sealed record PostDetailResponse(
    int Id,
    string Title,
    string Content,
    IReadOnlyList<CommentResponse> Comments);
