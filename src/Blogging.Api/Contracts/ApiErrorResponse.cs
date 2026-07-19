namespace Blogging.Api.Contracts;

/// <summary>
/// Represents the stable error envelope returned by the API.
/// </summary>
public sealed record ApiErrorResponse(string Error);
