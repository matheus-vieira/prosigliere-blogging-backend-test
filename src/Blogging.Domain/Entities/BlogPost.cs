namespace Blogging.Domain.Entities;

/// <summary>
/// Represents a blog post and its comments.
/// </summary>
public sealed class BlogPost
{
    private BlogPost()
    {
    }

    /// <summary>
    /// Creates a blog post with its required content.
    /// </summary>
    /// <param name="title">The post title.</param>
    /// <param name="content">The post content.</param>
    public BlogPost(string title, string content)
    {
        Title = title;
        Content = content;
    }

    public int Id { get; private set; }

    public string Title { get; private set; } = string.Empty;

    public string Content { get; private set; } = string.Empty;

    public ICollection<Comment> Comments { get; } = new List<Comment>();
}
