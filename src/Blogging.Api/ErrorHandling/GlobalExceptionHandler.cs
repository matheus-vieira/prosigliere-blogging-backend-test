using Blogging.Api.Contracts;
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
        Log.UnhandledRequestException(
            logger,
            exception.GetType().Name,
            httpContext.TraceIdentifier);

        if (!httpContext.Response.HasStarted)
        {
            var isInvalidRequest = exception is BadHttpRequestException;
            httpContext.Response.StatusCode = isInvalidRequest
                ? StatusCodes.Status400BadRequest
                : StatusCodes.Status500InternalServerError;
            var message = isInvalidRequest
                ? "Invalid request."
                : "An unexpected error occurred.";
            await httpContext.Response.WriteAsJsonAsync(
                new ApiErrorResponse(message),
                cancellationToken).ConfigureAwait(false);
        }

        return true;
    }

    private static partial class Log
    {
        [LoggerMessage(
            EventId = 1000,
            Level = LogLevel.Error,
            Message = "Unhandled request exception. Type: {exceptionType}; TraceId: {traceId}.")]
        public static partial void UnhandledRequestException(
            ILogger logger,
            string exceptionType,
            string traceId);
    }
}
