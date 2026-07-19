# Blogging Backend Test

This repository contains a .NET 10 REST API for a small blogging platform. The
challenge covers blog posts and their comments, including post creation,
listing, detail retrieval, and comment creation.

The original challenge statement is preserved in
[`challenge/README.md`](challenge/README.md).

## Current State

The repository currently contains the API baseline and the first persistence slice.
It includes the API, Domain, and Repository projects, shared build configuration,
package management, EF Core mappings, SQLite migrations, and persistence tests.
The functional HTTP endpoints will be implemented in subsequent branches.

## Technology Stack

- .NET 10 and ASP.NET Core Minimal APIs.
- Entity Framework Core 10 with the Microsoft SQLite provider.
- API, Domain, and Repository layers composed through `AddBloggingDomain`.
- Automatic local schema creation and pending migration application through
  `UseBloggingAsync` and `UseBloggingDatabaseAsync`.
- NuGet Central Package Management through `Directory.Packages.props`.
- `Directory.Build.props` for analyzers, nullable reference types, code style, and
  warnings as errors.
- MSTest, Moq, Moq.AutoMock, Bogus, WebApplicationFactory, and Coverlet.
- `BloggingBackend.slnx`, the modern solution format supported by the .NET SDK.

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
dotnet run --project src/Blogging.Api/Blogging.Api.csproj
```

The API currently exposes the template root endpoint. On startup, the configured
SQLite database checks for pending EF Core migrations and applies them when needed.

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
