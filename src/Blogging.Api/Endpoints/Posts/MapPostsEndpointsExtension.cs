namespace Blogging.Api.Endpoints.Posts;

/// <summary>
/// Maps HTTP endpoints for the Posts feature.
/// </summary>
public static class MapPostsEndpointsExtension
{
    /// <summary>
    /// Maps all Posts feature endpoints.
    /// </summary>
    /// <param name="endpoints">The endpoint route builder.</param>
    /// <returns>The same route builder for fluent composition.</returns>
    public static IEndpointRouteBuilder MapPostsEndpoints(
        this IEndpointRouteBuilder endpoints)
    {
        ArgumentNullException.ThrowIfNull(endpoints);

        endpoints.MapListPostsEndpoint();
        endpoints.MapCreatePostEndpoint();
        endpoints.MapSearchPostsEndpoint();
        endpoints.MapGetPostEndpoint();
        endpoints.MapCreateCommentEndpoint();

        return endpoints;
    }
}
