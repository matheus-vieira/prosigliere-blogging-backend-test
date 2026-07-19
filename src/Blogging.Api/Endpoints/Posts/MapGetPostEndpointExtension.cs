using Blogging.Api.Posts.Contracts;
using Blogging.Domain.Posts;

namespace Blogging.Api.Endpoints.Posts;

/// <summary>
/// Maps the post detail endpoint.
/// </summary>
public static class MapGetPostEndpointExtension
{
    /// <summary>
    /// Maps `GET /api/posts/{id}`.
    /// </summary>
    /// <param name="endpoints">The endpoint route builder.</param>
    /// <returns>The same route builder.</returns>
    public static IEndpointRouteBuilder MapGetPostEndpoint(
        this IEndpointRouteBuilder endpoints)
    {
        ArgumentNullException.ThrowIfNull(endpoints);

        endpoints.MapGet(
                "/api/posts/{id:int}",
                async (int id, BlogPostService service, CancellationToken cancellationToken) =>
                {
                    var post = await service
                        .GetByIdAsync(id, cancellationToken)
                        .ConfigureAwait(false);

                    if (post is null)
                    {
                        return Results.NotFound(new { error = "Post not found." });
                    }

                    var response = new PostDetailResponse(
                        post.Id,
                        post.Title,
                        post.Content,
                        post.Comments
                            .Select(comment => new CommentResponse(
                                comment.Id,
                                comment.PostId,
                                comment.Content))
                            .ToArray());

                    return Results.Ok(response);
                })
            .WithTags("Posts")
            .Produces<PostDetailResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        return endpoints;
    }
}
