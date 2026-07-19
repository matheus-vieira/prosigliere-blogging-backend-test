using System.Net;
using System.Net.Http.Json;
using Blogging.Api.Posts.Contracts;

namespace Blogging.Api.Tests;

/// <summary>
/// Verifies the POST Posts endpoint behavior.
/// </summary>
[TestClass]
public sealed class CreatePostEndpointTests
{
    /// <summary>
    /// Confirms that a valid payload creates and persists a post.
    /// </summary>
    [TestMethod]
    public async Task PostPostsCreatesPostAsync()
    {
        using var factory = new BloggingApiFactory();
        using var client = factory.CreateClient();

        var response = await client.PostAsJsonAsync(
                "/api/posts",
                new { title = "A title", content = "Post content" })
            .ConfigureAwait(false);
        var post = await response.Content
            .ReadFromJsonAsync<PostListItemResponse>()
            .ConfigureAwait(false);

        Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        Assert.IsNotNull(post);
        Assert.AreEqual("A title", post.Title);
        Assert.AreEqual(0, post.CommentCount);
    }

    /// <summary>
    /// Confirms that missing required values return a bad request without a write.
    /// </summary>
    [TestMethod]
    public async Task PostPostsRejectsEmptyPayloadAsync()
    {
        using var factory = new BloggingApiFactory();
        using var client = factory.CreateClient();

        var response = await client.PostAsJsonAsync(
                "/api/posts",
                new { title = (string?)null, content = (string?)null })
            .ConfigureAwait(false);
        var posts = await client.GetFromJsonAsync<List<PostListItemResponse>>(
                "/api/posts")
            .ConfigureAwait(false);

        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.IsNotNull(posts);
        Assert.IsEmpty(posts);
    }
}
