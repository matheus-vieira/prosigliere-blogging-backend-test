using System.Net;
using System.Net.Http.Json;
using Blogging.Api.Posts.Contracts;

namespace Blogging.Api.Tests;

/// <summary>
/// Verifies the GET post detail endpoint behavior.
/// </summary>
[TestClass]
public sealed class GetPostEndpointTests
{
    /// <summary>
    /// Confirms that an existing post returns its content and comments.
    /// </summary>
    [TestMethod]
    public async Task GetPostReturnsDetailAsync()
    {
        using var factory = new BloggingApiFactory();
        using var client = factory.CreateClient();
        var createResponse = await client.PostAsJsonAsync(
                "/api/posts",
                new { title = "A title", content = "Post content" })
            .ConfigureAwait(false);
        var created = await createResponse.Content
            .ReadFromJsonAsync<PostListItemResponse>()
            .ConfigureAwait(false);

        var response = await client.GetAsync($"/api/posts/{created!.Id}")
            .ConfigureAwait(false);
        var detail = await response.Content
            .ReadFromJsonAsync<PostDetailResponse>()
            .ConfigureAwait(false);

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.IsNotNull(detail);
        Assert.AreEqual("A title", detail.Title);
        Assert.AreEqual("Post content", detail.Content);
        Assert.IsEmpty(detail.Comments);
    }

    /// <summary>
    /// Confirms that a missing post returns not found.
    /// </summary>
    [TestMethod]
    public async Task GetPostReturnsNotFoundForMissingPostAsync()
    {
        using var factory = new BloggingApiFactory();
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/api/posts/999").ConfigureAwait(false);

        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
    }
}
