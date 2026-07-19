# API Bootstrap Decision

## OpenAPI and Swagger

Use `Microsoft.AspNetCore.OpenApi` and `Swashbuckle.AspNetCore` for API contract
discovery. The application exposes the generated OpenAPI document at
`/swagger/v1/swagger.json`. Swagger UI is enabled only in Development at
`/swagger`, avoiding an unnecessary interactive documentation surface in
production.

The API registration is isolated in `AddBloggingApi`, while middleware and
diagnostic endpoints are configured by `UseBloggingApi`. Each extension method is
kept in its own receiver-specific directory and file.

## Health Checks

Register ASP.NET Core health checks and expose `/health` in every environment. The
endpoint is intentionally small and does not expose configuration, dependency
details, connection strings, or stack traces.

## Exception Handling

Register `GlobalExceptionHandler` through the ASP.NET Core exception handler
pipeline. Unexpected exceptions are logged with source-generated structured
logging and return only:

```json
{
  "error": "An unexpected error occurred."
}
```

The original exception, SQL, stack trace, secrets, and user payload are not sent to
the client.

## Alternatives Considered

- Exposing Swagger UI in all environments: rejected to reduce production surface.
- Manually documenting routes without OpenAPI: rejected because it drifts from the
  executable API contract.
- Returning exception details: rejected because it leaks implementation and
  potentially sensitive data.

## Validation

```bash
```

Integration tests verify `/health`, `/swagger/v1/swagger.json`, and safe exception
responses.
