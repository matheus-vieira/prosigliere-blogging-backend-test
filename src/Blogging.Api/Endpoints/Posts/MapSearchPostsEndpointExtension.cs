using Blogging.Api.Contracts;
using Blogging.Api.Posts.Contracts;
using Blogging.Domain.Posts;
using Microsoft.AspNetCore.Http;

namespace Blogging.Api.Endpoints.Posts;

/// <summary>
/// Maps the post search endpoint.
/// </summary>
public static class MapSearchPostsEndpointExtension
{
    /// <summary>
    /// Maps `GET /api/posts/search` with optional filters.
    /// </summary>
    /// <param name="endpoints">The endpoint route builder.</param>
    /// <returns>The same route builder.</returns>
    public static IEndpointRouteBuilder MapSearchPostsEndpoint(
        this IEndpointRouteBuilder endpoints)
    {
        ArgumentNullException.ThrowIfNull(endpoints);

        endpoints.MapGet(
                "/api/posts/search",
                async (
                    [AsParameters] PostSearchRequest request,
                    PostSearchService service,
                    CancellationToken cancellationToken) =>
                {
                    try
                    {
                        var results = await service.SearchAsync(
                                new PostFilter
                                {
                                    Title = request.Title,
                                    Content = request.Content,
                                    HasComments = request.HasComments,
                                    MinCommentCount = request.MinCommentCount,
                                    MaxCommentCount = request.MaxCommentCount,
                                    Page = request.Page ?? 1,
                                    PageSize = request.PageSize ?? 20,
                                    Sorts = (request.SortBy ?? [])
                                        .Select(value => new PostSort(
                                            value.TrimStart('-'),
                                            value.StartsWith('-')))
                                        .ToArray()
                                },
                                cancellationToken)
                            .ConfigureAwait(false);

                        return Results.Ok(new PagedPostSearchResponse(
                            results.Items.Select(post => new PostListItemResponse(
                                post.Id,
                                post.Title,
                                post.CommentCount)).ToArray(),
                            results.Page,
                            results.PageSize,
                            results.TotalCount));
                    }
                    catch (ArgumentException exception)
                    {
                        return Results.BadRequest(new ApiErrorResponse(exception.Message));
                    }
                })
            .WithTags("Posts")
            .WithSummary("Search posts")
            .Produces<IReadOnlyList<PostListItemResponse>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);

        return endpoints;
    }
}
