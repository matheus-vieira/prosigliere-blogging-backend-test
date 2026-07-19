using Blogging.Api.ErrorHandling;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;

namespace Blogging.Api.Tests;

/// <summary>
/// Verifies API health, OpenAPI, Swagger UI, and safe exception responses.
/// </summary>
[TestClass]
public sealed class ApiDiagnosticsTests
{
    /// <summary>
    /// Confirms that the health endpoint reports a healthy application.
    /// </summary>
    [TestMethod]
    public async Task HealthEndpointReturnsSuccessAsync()
    {
        using var factory = new BloggingApiFactory();
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/health").ConfigureAwait(false);

        Assert.IsTrue(response.IsSuccessStatusCode);
    }

    /// <summary>
    /// Confirms that the OpenAPI document is available in Development.
    /// </summary>
    [TestMethod]
    public async Task SwaggerDocumentIsAvailableInDevelopmentAsync()
    {
        using var factory = new BloggingApiFactory();
        using var client = factory.CreateClient();

        var response = await client
            .GetAsync("/swagger/v1/swagger.json")
            .ConfigureAwait(false);
        var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

        Assert.IsTrue(response.IsSuccessStatusCode);
        StringAssert.Contains(content, "\"openapi\"");
    }

    /// <summary>
    /// Confirms that unexpected exceptions produce a safe error response.
    /// </summary>
    [TestMethod]
    public async Task ExceptionHandlerWritesSafeErrorResponseAsync()
    {
        var context = new DefaultHttpContext
        {
            Response = { Body = new MemoryStream() }
        };
        var handler = new GlobalExceptionHandler(
            NullLogger<GlobalExceptionHandler>.Instance);

        var handled = await handler.TryHandleAsync(
            context,
            new InvalidOperationException("private failure"),
            CancellationToken.None).ConfigureAwait(false);

        context.Response.Body.Position = 0;
        using var reader = new StreamReader(context.Response.Body);
        var content = await reader.ReadToEndAsync().ConfigureAwait(false);

        Assert.IsTrue(handled);
        Assert.AreEqual(StatusCodes.Status500InternalServerError, context.Response.StatusCode);
        StringAssert.Contains(content, "An unexpected error occurred.");
        Assert.IsFalse(content.Contains("private failure", StringComparison.Ordinal));
    }

}
