namespace Blogging.Api.Posts.Contracts;

/// <summary>
/// Represents a paged post search response.
/// </summary>
public sealed record PagedPostSearchResponse(
    IReadOnlyList<PostListItemResponse> Items,
    int Page,
    int PageSize,
    int TotalCount);
