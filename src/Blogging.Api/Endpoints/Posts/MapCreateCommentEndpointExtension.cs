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
                "/api/posts/{id:int}/comments",
                async (
                    int id,
                    CreateCommentRequest request,
                    BlogPostService service,
                    CancellationToken cancellationToken) =>
                {
                    try
                    {
                        var comment = await service.CreateCommentAsync(
                                id,
                                new CreateCommentCommand
                                {
                                    Content = request.Content ?? string.Empty
                                },
                                cancellationToken)
                            .ConfigureAwait(false);

                        if (comment is null)
                        {
                            return Results.NotFound(new { error = "Post not found." });
                        }

                        var response = new CommentResponse(
                            comment.Id,
                            comment.PostId,
                            comment.Content);

                        return Results.Created($"/api/posts/{id}", response);
                    }
                    catch (ArgumentException exception)
                    {
                        return Results.BadRequest(new { error = exception.Message });
                    }
                })
            .WithTags("Comments")
            .Produces<CommentResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);

        return endpoints;
    }
}
