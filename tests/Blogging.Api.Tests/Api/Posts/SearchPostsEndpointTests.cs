using System.Net;
using System.Net.Http.Json;
using Blogging.Api.Posts.Contracts;

namespace Blogging.Api.Tests;

/// <summary>
/// Verifies post search filters, ordering, and pagination through HTTP GET.
/// </summary>
[TestClass]
[TestCategory("Api")]
public sealed class SearchPostsEndpointTests
{
    [TestMethod]
    public async Task GetSearchPostsFiltersAndPaginatesAsync()
    {
        using var factory = new BloggingApiFactory();
        using var client = factory.CreateClient();
        await CreatePostAsync(client, "API Alpha", "Backend content").ConfigureAwait(false);
        await CreatePostAsync(client, "Other", "Unrelated content").ConfigureAwait(false);

        var result = await client.GetFromJsonAsync<PagedPostSearchResponse>(
                "/api/posts/search?title=api&page=1&pageSize=1&sortBy=-title")
            .ConfigureAwait(false);

        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.TotalCount);
        Assert.AreEqual("API Alpha", result.Items[0].Title);
    }

    [TestMethod]
    public async Task GetSearchPostsFiltersByCommentCountAsync()
    {
        using var factory = new BloggingApiFactory();
        using var client = factory.CreateClient();
        var post = await CreatePostAsync(client, "Commented", "Content").ConfigureAwait(false);
        await client.PostAsJsonAsync(
                $"/api/posts/{post.Id}/comments",
                new { content = "Comment" })
            .ConfigureAwait(false);

        var result = await client.GetFromJsonAsync<PagedPostSearchResponse>(
                "/api/posts/search?hasComments=true&minCommentCount=1")
            .ConfigureAwait(false);

        Assert.IsNotNull(result);
        Assert.AreEqual("Commented", result.Items.Single().Title);
    }

    [TestMethod]
    public async Task GetSearchPostsCombinesOptionalFiltersAsync()
    {
        using var factory = new BloggingApiFactory();
        using var client = factory.CreateClient();
        await CreatePostAsync(client, "Backend Alpha", "Backend content")
            .ConfigureAwait(false);

        var result = await client.GetFromJsonAsync<PagedPostSearchResponse>(
                "/api/posts/search?content=backend&hasComments=false&minCommentCount=0&maxCommentCount=5&page=1&pageSize=10&sortBy=title&sortBy=-commentCount")
            .ConfigureAwait(false);

        Assert.IsNotNull(result);
        Assert.AreEqual("Backend Alpha", result.Items.Single().Title);
    }

    [TestMethod]
    public async Task GetSearchPostsRejectsInvalidRangeAsync()
    {
        using var factory = new BloggingApiFactory();
        using var client = factory.CreateClient();

        var response = await client.GetAsync(
                "/api/posts/search?minCommentCount=3&maxCommentCount=1")
            .ConfigureAwait(false);

        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [TestMethod]
    public async Task GetSearchPostsRejectsUnsupportedSortFieldAsync()
    {
        using var factory = new BloggingApiFactory();
        using var client = factory.CreateClient();

        var response = await client.GetAsync(
                "/api/posts/search?sortBy=unknown")
            .ConfigureAwait(false);

        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
    }

    private static async Task<PostListItemResponse> CreatePostAsync(
        HttpClient client,
        string title,
        string content)
    {
        var response = await client.PostAsJsonAsync(
                "/api/posts",
                new { title, content })
            .ConfigureAwait(false);
        return (await response.Content
            .ReadFromJsonAsync<PostListItemResponse>()
            .ConfigureAwait(false))!;
    }
}
