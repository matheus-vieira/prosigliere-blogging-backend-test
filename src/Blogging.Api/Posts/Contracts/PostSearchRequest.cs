namespace Blogging.Api.Posts.Contracts;

/// <summary>
/// Represents optional query filters for post search.
/// </summary>
public sealed class PostSearchRequest
{
    public string? Title { get; set; }

    public string? Content { get; set; }

    public bool? HasComments { get; set; }

    public int? MinCommentCount { get; set; }

    public int? MaxCommentCount { get; set; }

    public int? Page { get; set; }

    public int? PageSize { get; set; }

    public string[]? SortBy { get; set; }
}
