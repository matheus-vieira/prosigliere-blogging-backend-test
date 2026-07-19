using Blogging.Api.Posts.Contracts;
using Blogging.Domain.Posts;

namespace Blogging.Api.Endpoints.Posts;

/// <summary>
/// Maps the post listing endpoint.
/// </summary>
public static class MapListPostsEndpointExtension
{
    /// <summary>
    /// Maps `GET /api/posts`.
    /// </summary>
    /// <param name="endpoints">The endpoint route builder.</param>
    /// <returns>The same route builder.</returns>
    public static IEndpointRouteBuilder MapListPostsEndpoint(
        this IEndpointRouteBuilder endpoints)
    {
        ArgumentNullException.ThrowIfNull(endpoints);

        endpoints.MapGet(
                "/api/posts",
                async (BlogPostService service, CancellationToken cancellationToken) =>
                {
                    var posts = await service
                        .ListAsync(cancellationToken)
                        .ConfigureAwait(false);

                    return Results.Ok(posts.Select(post => new PostListItemResponse(
                        post.Id,
                        post.Title,
                        post.CommentCount)));
                })
            .WithTags("Posts")
            .Produces<IReadOnlyList<PostListItemResponse>>(StatusCodes.Status200OK);

        return endpoints;
    }
}
