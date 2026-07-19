namespace Blogging.Domain.Posts;

/// <summary>
/// Contains optional filters shared by post GET use cases.
/// </summary>
public sealed record PostFilter
{
    public int? Id { get; init; }

    public string? Title { get; init; }

    public string? Content { get; init; }

    public bool? HasComments { get; init; }

    public int? MinCommentCount { get; init; }

    public int? MaxCommentCount { get; init; }

    public int Page { get; init; } = 1;

    public int PageSize { get; init; } = 20;

    public IReadOnlyList<PostSort> Sorts { get; init; } = [];
}

/// <summary>
/// Defines one requested post sort field.
/// </summary>
public sealed record PostSort(string Field, bool Descending);
