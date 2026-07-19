using Blogging.Domain.Entities;

namespace Blogging.Api.Tests;

/// <summary>
/// Verifies Domain entity invariants that do not require infrastructure.
/// </summary>
[TestClass]
public sealed class DomainEntityTests
{
    /// <summary>
    /// Confirms that a blog post stores its required values and starts without comments.
    /// </summary>
    [TestMethod]
    public void BlogPostStoresValuesAndStartsWithoutComments()
    {
        var post = new BlogPost("A title", "Post content");

        Assert.AreEqual("A title", post.Title);
        Assert.AreEqual("Post content", post.Content);
        Assert.IsEmpty(post.Comments);
    }

    /// <summary>
    /// Confirms that a comment stores its post relationship and content.
    /// </summary>
    [TestMethod]
    public void CommentStoresPostRelationshipAndContent()
    {
        var comment = new Comment(42, "A comment");

        Assert.AreEqual(42, comment.PostId);
        Assert.AreEqual("A comment", comment.Content);
    }
}
