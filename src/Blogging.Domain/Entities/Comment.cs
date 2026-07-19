namespace Blogging.Domain.Entities;

/// <summary>
/// Represents a comment associated with a blog post.
/// </summary>
public sealed class Comment
{
    private Comment()
    {
    }

    /// <summary>
    /// Creates a comment associated with a blog post.
    /// </summary>
    /// <param name="postId">The identifier of the associated post.</param>
    /// <param name="content">The comment content.</param>
    public Comment(int postId, string content)
    {
        PostId = postId;
        Content = content;
    }

    public int Id { get; private set; }

    public int PostId { get; private set; }

    public string Content { get; private set; } = string.Empty;

    public BlogPost Post { get; private set; } = null!;
}
