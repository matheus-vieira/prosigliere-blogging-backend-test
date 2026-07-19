namespace Blogging.Domain.Posts;

/// <summary>
/// Represents a paged query result.
/// </summary>
public sealed record PagedResult<T>(
    IReadOnlyList<T> Items,
    int Page,
    int PageSize,
    int TotalCount);
