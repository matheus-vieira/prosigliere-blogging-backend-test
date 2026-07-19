# Blogging Backend Test

This repository contains a .NET 10 REST API for a small blogging platform. The
challenge covers blog posts and their comments, including post creation,
listing, detail retrieval, and comment creation.

The original challenge statement is preserved in
[`challenge/README.md`](challenge/README.md).

## Current State

The repository contains the API, Domain, and Repository projects, shared build
configuration, package management, EF Core mappings, SQLite migrations, functional
Posts and Comments endpoints, and a layered automated test suite.

## Technology Stack

- .NET 10 and ASP.NET Core Minimal APIs.
- Entity Framework Core 10 with the Microsoft SQLite provider.
- API, Domain, and Repository layers composed through `AddBloggingDomain`.
- Automatic local schema creation and pending migration application through
  `UseBloggingApiAsync`.
- OpenAPI document and Swagger UI through Swashbuckle.
- NuGet Central Package Management through `Directory.Packages.props`.
- `Directory.Build.props` for analyzers, nullable reference types, code style, and
  warnings as errors.
- MSTest, Moq, Moq.AutoMock, Bogus, WebApplicationFactory, and Coverlet.
- `BloggingBackend.slnx`, the modern solution format supported by the .NET SDK.

`UseBloggingApiAsync` is the single API startup call and internally maps feature
endpoints through `MapBloggingEndpoints`.

## Prerequisites

- .NET SDK `10.0.110` or a compatible .NET 10 SDK.
- A shell with access to `https://api.nuget.org/v3/index.json`.

The required SDK is declared in `global.json`.

## Quick Start

From the repository root:

```bash
dotnet restore BloggingBackend.slnx
dotnet build BloggingBackend.slnx --warnaserror
dotnet test BloggingBackend.slnx
dotnet run --launch-profile http --project src/Blogging.Api/Blogging.Api.csproj
```

On startup, the configured SQLite database checks for pending EF Core migrations and
applies them when needed. The root path `/` redirects to `/swagger`.

Swagger UI is available at `/swagger` and the OpenAPI document is available at
`/swagger/v1/swagger.json`. The health endpoint is available at `/health` in every
environment.

Always open Swagger through the running API, for example
`http://localhost:5185/swagger`. Do not open `swagger/index.html` directly from the
file system, because a `file://` page cannot execute the OpenAPI request as an HTTP
or HTTPS CORS request.

The development database is stored in `src/Blogging.Api/blogging.db` and is ignored
by Git. Startup checks for pending migrations and applies them. Tests use isolated
temporary databases and never use the development database.

## Implemented Endpoints

- `GET /api/posts`: lists posts with `title` and `commentCount`.
- `POST /api/posts`: creates a post with `title` and `content`.
- `GET /api/posts/{id}`: returns a post with its content and comments.
- `POST /api/posts/{id}/comments`: creates a comment for an existing post.
- `GET /api/posts/search`: searches posts with optional filters, pagination, and
  multiple sort fields.

Invalid payloads and identifiers return `400` with an `error` field. Missing posts
return `404`, and unexpected failures return a generic `500` without implementation
details.

Detailed request and response examples are available in
[`docs/api/README.md`](docs/api/README.md).

## Quality Checks

Run the complete local validation before opening a pull request:

```bash
dotnet restore BloggingBackend.slnx
dotnet format BloggingBackend.slnx --no-restore --verify-no-changes
dotnet build BloggingBackend.slnx --no-restore --warnaserror
dotnet test BloggingBackend.slnx --no-build --no-restore
dotnet list BloggingBackend.slnx package --vulnerable --include-transitive
```

The implementation plan requires at least 85% line coverage and 85% branch
coverage for production code once functional code is introduced.

The current suite uses MSTest, Moq, Moq.AutoMock, Bogus, WebApplicationFactory,
isolated SQLite fixtures, and Coverlet MSBuild.

## Troubleshooting

- **Swagger reports `Failed to fetch`:** open `http://localhost:5185/swagger` while
  the API is running. Do not open `swagger/index.html` through `file://`.
- **The database is locked:** stop other local API processes and retry. SQLite
  migrations use EF Core's migration lock.
- **The database schema is stale:** remove the ignored local `blogging.db` only in
  development, then restart the API so pending migrations run again.
- **The SDK is not found:** install .NET SDK 10 and verify `dotnet --version`.

## Next Steps

- Add pagination and filtering for post listing.
- Add authentication and authorization.
- Add richer validation and typed error contracts.
- Add production migration coordination and deployment locks.
- Add observability metrics, tracing, and rate limiting.

## Repository Layout

```text
src/Blogging.Api/         API and HTTP composition root
src/Blogging.Domain/      Domain entities and service registration
src/Blogging.Repository/  EF Core context, mappings, migrations, and SQLite
tests/Blogging.Api.Tests/ MSTest unit and integration tests
docs/                   Project documentation and architectural decisions
challenge/              Original coding challenge
```

## Documentation

Start with [`docs/README.md`](docs/README.md). Architectural decisions follow the
hierarchy under
[`docs/archtectural-decision-records/`](docs/archtectural-decision-records/).
The directory name is kept as part of the project documentation convention.

The implementation task backlog is maintained in the local planning repository:
`/mnt/c/src/github/End2EndSystems/combinei-local/prosigliere/blogging-backend-test/tasks/README.md`.

## EF Core References

- [EF Core overview](https://learn.microsoft.com/en-us/ef/core/)
- [EF Core database providers](https://learn.microsoft.com/en-us/ef/core/providers/?tabs=dotnet-core-cli)
- [EF Core SQLite provider](https://learn.microsoft.com/en-us/ef/core/providers/sqlite/)
- [NuGet Central Package Management](https://learn.microsoft.com/en-us/nuget/consume-packages/central-package-management)

## License

See [`LICENSE`](LICENSE).
