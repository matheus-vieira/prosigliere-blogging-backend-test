using Blogging.Api.Contracts;
using Blogging.Api.Posts.Contracts;
using Blogging.Domain.Posts;

namespace Blogging.Api.Endpoints.Posts;

/// <summary>
/// Maps the post creation endpoint.
/// </summary>
public static class MapCreatePostEndpointExtension
{
    /// <summary>
    /// Maps `POST /api/posts`.
    /// </summary>
    /// <param name="endpoints">The endpoint route builder.</param>
    /// <returns>The same route builder.</returns>
    public static IEndpointRouteBuilder MapCreatePostEndpoint(
        this IEndpointRouteBuilder endpoints)
    {
        ArgumentNullException.ThrowIfNull(endpoints);

        endpoints.MapPost(
                "/api/posts",
                async (
                    CreatePostRequest? request,
                    BlogPostService service,
                    CancellationToken cancellationToken) =>
                {
                    try
                    {
                        if (request is null)
                        {
                            return Results.BadRequest(new ApiErrorResponse("Request body is required."));
                        }

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
                        return Results.BadRequest(new ApiErrorResponse(exception.Message));
                    }
                })
            .WithTags("Posts")
            .Produces<PostListItemResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest);

        return endpoints;
    }
}
