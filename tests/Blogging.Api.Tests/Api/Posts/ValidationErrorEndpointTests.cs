using System.Net;
using System.Net.Http.Json;
using Blogging.Api.Contracts;
using Blogging.Api.Posts.Contracts;

namespace Blogging.Api.Tests;

/// <summary>
/// Verifies invalid identifiers and payload limits for Posts and Comments.
/// </summary>
[TestClass]
public sealed class ValidationErrorEndpointTests
{
    /// <summary>
    /// Confirms that an invalid post identifier returns a structured bad request.
    /// </summary>
    [TestMethod]
    public async Task GetPostRejectsInvalidIdentifierAsync()
    {
        using var factory = new BloggingApiFactory();
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/api/posts/not-an-id").ConfigureAwait(false);
        var error = await response.Content
            .ReadFromJsonAsync<ApiErrorResponse>()
            .ConfigureAwait(false);

        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.IsNotNull(error);
        Assert.IsFalse(string.IsNullOrWhiteSpace(error.Error));
    }

    /// <summary>
    /// Confirms that an invalid comment post identifier returns bad request.
    /// </summary>
    [TestMethod]
    public async Task PostCommentRejectsInvalidIdentifierAsync()
    {
        using var factory = new BloggingApiFactory();
        using var client = factory.CreateClient();

        var response = await client.PostAsJsonAsync(
                "/api/posts/not-an-id/comments",
                new { content = "A comment" })
            .ConfigureAwait(false);

        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
    }

    /// <summary>
    /// Confirms that oversized post content returns bad request.
    /// </summary>
    [TestMethod]
    public async Task PostPostsRejectsOversizedContentAsync()
    {
        using var factory = new BloggingApiFactory();
        using var client = factory.CreateClient();
        var content = new string('x', 10001);

        var response = await client.PostAsJsonAsync(
                "/api/posts",
                new { title = "A title", content })
            .ConfigureAwait(false);

        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
    }

    /// <summary>
    /// Confirms that malformed JSON returns the standard error extension.
    /// </summary>
    [TestMethod]
    public async Task PostPostsRejectsMalformedJsonAsync()
    {
        using var factory = new BloggingApiFactory();
        using var client = factory.CreateClient();
        using var content = new StringContent("{", System.Text.Encoding.UTF8, "application/json");

        var response = await client.PostAsync("/api/posts", content).ConfigureAwait(false);
        var body = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        StringAssert.Contains(body, "\"error\"");
    }
}
