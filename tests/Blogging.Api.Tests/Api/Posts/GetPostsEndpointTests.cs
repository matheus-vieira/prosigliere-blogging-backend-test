using System.Net;
using System.Net.Http.Json;
using Blogging.Api.Posts.Contracts;
using Blogging.Domain.Entities;
using Blogging.Repository.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace Blogging.Api.Tests;

/// <summary>
/// Verifies the GET Posts endpoint behavior.
/// </summary>
[TestClass]
[TestCategory("Api")]
public sealed class GetPostsEndpointTests
{
    /// <summary>
    /// Confirms that an empty database returns an empty list.
    /// </summary>
    [TestMethod]
    public async Task GetPostsReturnsEmptyListAsync()
    {
        using var factory = new BloggingApiFactory();
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/api/posts").ConfigureAwait(false);
        var posts = await response.Content
            .ReadFromJsonAsync<List<PostListItemResponse>>()
            .ConfigureAwait(false);

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.IsNotNull(posts);
        Assert.IsEmpty(posts);
    }

    /// <summary>
    /// Confirms that list projection includes the persisted comment count.
    /// </summary>
    [TestMethod]
    public async Task GetPostsReturnsCommentCountAsync()
    {
        using var factory = new BloggingApiFactory();
        using var client = factory.CreateClient();
        var createResponse = await client.PostAsJsonAsync(
                "/api/posts",
                new { title = "A title", content = "Post content" })
            .ConfigureAwait(false);
        var post = await createResponse.Content
            .ReadFromJsonAsync<PostListItemResponse>()
            .ConfigureAwait(false);

        using (var scope = factory.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<BlogDbContext>();
            context.Comments.Add(new Comment(post!.Id, "A comment"));
            await context.SaveChangesAsync().ConfigureAwait(false);
        }

        var response = await client.GetFromJsonAsync<List<PostListItemResponse>>(
                "/api/posts")
            .ConfigureAwait(false);

        Assert.IsNotNull(response);
        Assert.AreEqual(1, response.Single().CommentCount);
    }
}
