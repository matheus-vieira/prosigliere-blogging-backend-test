using Blogging.Api.Endpoints.Posts;

namespace Blogging.Api.Endpoints;

/// <summary>
/// Aggregates all API endpoint mappings.
/// </summary>
public static class MapBloggingEndpointsExtension
{
    /// <summary>
    /// Maps every blogging feature endpoint.
    /// </summary>
    /// <param name="endpoints">The endpoint route builder.</param>
    /// <returns>The same route builder for fluent composition.</returns>
    public static IEndpointRouteBuilder MapBloggingEndpoints(
        this IEndpointRouteBuilder endpoints)
    {
        ArgumentNullException.ThrowIfNull(endpoints);

        endpoints.MapGet("/", () => Results.Redirect("/swagger"));
        endpoints.MapPostsEndpoints();
        return endpoints;
    }
}
