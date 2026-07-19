using System.Net;
using System.Net.Http.Json;
using Blogging.Api.Posts.Contracts;

namespace Blogging.Api.Tests;

/// <summary>
/// Verifies the POST comment endpoint behavior.
/// </summary>
[TestClass]
[TestCategory("Api")]
public sealed class CreateCommentEndpointTests
{
    /// <summary>
    /// Confirms that a valid comment is associated with its post.
    /// </summary>
    [TestMethod]
    public async Task PostCommentCreatesAssociatedCommentAsync()
    {
        using var factory = new BloggingApiFactory();
        using var client = factory.CreateClient();
        var postResponse = await client.PostAsJsonAsync(
                "/api/posts",
                new { title = "A title", content = "Post content" })
            .ConfigureAwait(false);
        var post = await postResponse.Content
            .ReadFromJsonAsync<PostListItemResponse>()
            .ConfigureAwait(false);

        var response = await client.PostAsJsonAsync(
                $"/api/posts/{post!.Id}/comments",
                new { content = "A comment" })
            .ConfigureAwait(false);
        var comment = await response.Content
            .ReadFromJsonAsync<CommentResponse>()
            .ConfigureAwait(false);

        Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        Assert.IsNotNull(comment);
        Assert.AreEqual(post.Id, comment.PostId);
        Assert.AreEqual("A comment", comment.Content);
    }

    /// <summary>
    /// Confirms that comments cannot be created for a missing post.
    /// </summary>
    [TestMethod]
    public async Task PostCommentReturnsNotFoundForMissingPostAsync()
    {
        using var factory = new BloggingApiFactory();
        using var client = factory.CreateClient();

        var response = await client.PostAsJsonAsync(
                "/api/posts/999/comments",
                new { content = "A comment" })
            .ConfigureAwait(false);

        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
    }

    /// <summary>
    /// Confirms that empty comment content returns a bad request.
    /// </summary>
    [TestMethod]
    public async Task PostCommentRejectsEmptyContentAsync()
    {
        using var factory = new BloggingApiFactory();
        using var client = factory.CreateClient();
        var postResponse = await client.PostAsJsonAsync(
                "/api/posts",
                new { title = "A title", content = "Post content" })
            .ConfigureAwait(false);
        var post = await postResponse.Content
            .ReadFromJsonAsync<PostListItemResponse>()
            .ConfigureAwait(false);

        var response = await client.PostAsJsonAsync(
                $"/api/posts/{post!.Id}/comments",
                new { content = "" })
            .ConfigureAwait(false);

        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
