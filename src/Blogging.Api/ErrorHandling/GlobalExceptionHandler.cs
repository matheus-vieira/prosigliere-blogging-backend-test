using Microsoft.AspNetCore.Diagnostics;

namespace Blogging.Api.ErrorHandling;

/// <summary>
/// Converts unexpected exceptions into safe API responses.
/// </summary>
public sealed partial class GlobalExceptionHandler(
    ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    /// <summary>
    /// Logs an unexpected exception and writes a generic error response.
    /// </summary>
    /// <param name="httpContext">The current HTTP context.</param>
    /// <param name="exception">The unhandled exception.</param>
    /// <param name="cancellationToken">Cancels response writing.</param>
    /// <returns>Always returns true after handling the exception.</returns>
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        Log.UnhandledRequestException(logger, exception);

        if (!httpContext.Response.HasStarted)
        {
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await httpContext.Response.WriteAsJsonAsync(
                new { error = "An unexpected error occurred." },
                cancellationToken).ConfigureAwait(false);
        }

        return true;
    }

    private static partial class Log
    {
        [LoggerMessage(
            EventId = 1000,
            Level = LogLevel.Error,
            Message = "Unhandled request exception.")]
        public static partial void UnhandledRequestException(
            ILogger logger,
            Exception exception);
    }
}
