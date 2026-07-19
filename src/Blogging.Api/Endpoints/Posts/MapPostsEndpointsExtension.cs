using Blogging.Api.Posts.Contracts;
using Blogging.Domain.Posts;

namespace Blogging.Api.Endpoints.Posts;

/// <summary>
/// Maps HTTP endpoints for the Posts feature.
/// </summary>
public static class MapPostsEndpointsExtension
{
    /// <summary>
    /// Maps post listing and creation endpoints.
    /// </summary>
    /// <param name="endpoints">The endpoint route builder.</param>
    /// <returns>The same route builder for fluent composition.</returns>
    public static IEndpointRouteBuilder MapPostsEndpoints(
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

        endpoints.MapPost(
                "/api/posts",
                async (
                    CreatePostRequest request,
                    BlogPostService service,
                    CancellationToken cancellationToken) =>
                {
                    try
                    {
                        var post = await service.CreateAsync(
                                new CreateBlogPostCommand
                                {
                                    Title = request.Title ?? string.Empty,
                                    Content = request.Content ?? string.Empty
                                },
                                cancellationToken)
                            .ConfigureAwait(false);

                        var response = new PostListItemResponse(
                            post.Id,
                            post.Title,
                            post.CommentCount);

                        return Results.Created($"/api/posts/{post.Id}", response);
                    }
                    catch (ArgumentException exception)
                    {
                        return Results.BadRequest(new { error = exception.Message });
                    }
                })
            .WithTags("Posts")
            .Produces<PostListItemResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest);

        return endpoints;
    }
}
