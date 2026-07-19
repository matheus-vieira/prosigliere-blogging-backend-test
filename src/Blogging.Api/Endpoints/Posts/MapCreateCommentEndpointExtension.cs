using System.Globalization;
using Blogging.Api.Contracts;
using Blogging.Api.Posts.Contracts;
using Blogging.Domain.Posts;

namespace Blogging.Api.Endpoints.Posts;

/// <summary>
/// Maps the comment creation endpoint.
/// </summary>
public static class MapCreateCommentEndpointExtension
{
    /// <summary>
    /// Maps `POST /api/posts/{id}/comments`.
    /// </summary>
    /// <param name="endpoints">The endpoint route builder.</param>
    /// <returns>The same route builder.</returns>
    public static IEndpointRouteBuilder MapCreateCommentEndpoint(
        this IEndpointRouteBuilder endpoints)
    {
        ArgumentNullException.ThrowIfNull(endpoints);

        endpoints.MapPost(
                "/api/posts/{id}/comments",
                async (
                    string id,
                    CreateCommentRequest? request,
                    BlogPostService service,
                    CancellationToken cancellationToken) =>
                {
                    try
                    {
                        if (!int.TryParse(id, NumberStyles.None, CultureInfo.InvariantCulture, out var postId)
                            || postId <= 0)
                        {
                            return Results.BadRequest(new ApiErrorResponse("Post id must be a positive integer."));
                        }

                        if (request is null)
                        {
                            return Results.BadRequest(new ApiErrorResponse("Request body is required."));
                        }

                        var comment = await service.CreateCommentAsync(
                                postId,
                                new CreateCommentCommand
                                {
                                    Content = request.Content ?? string.Empty
                                },
                                cancellationToken)
                            .ConfigureAwait(false);

                        if (comment is null)
                        {
                            return Results.NotFound(new ApiErrorResponse("Post not found."));
                        }

                        var response = new CommentResponse(
                            comment.Id,
                            comment.PostId,
                            comment.Content);

                        return Results.Created($"/api/posts/{postId}", response);
                    }
                    catch (ArgumentException exception)
                    {
                        return Results.BadRequest(new ApiErrorResponse(exception.Message));
                    }
                })
            .WithTags("Comments")
            .Produces<CommentResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);

        return endpoints;
    }
}
