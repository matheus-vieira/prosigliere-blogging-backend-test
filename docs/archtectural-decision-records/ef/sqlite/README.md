# SQLite Provider Decision

## Context

The challenge requires a small, reproducible backend without an external database
service. The application needs relational constraints, migrations, and isolated
integration tests.

## Decision

Use the Microsoft-maintained `Microsoft.EntityFrameworkCore.Sqlite` provider for
EF Core 10. Use a local SQLite file for development and a disposable SQLite
database for integration tests. Do not use the EF Core InMemory provider for
persistence tests because it does not reproduce relational SQLite behavior.

The provider dependencies are centrally managed in `Directory.Packages.props`.
The EF Core relational package is also referenced directly, as recommended by the
official provider documentation, to make relational patch updates explicit.

## Application Startup

The Repository layer exposes `UseBloggingDatabaseAsync`, which applies pending
migrations through `Database.GetPendingMigrationsAsync` followed by
`Database.MigrateAsync` when needed. The API invokes this
through `UseBloggingAsync`; `Program.cs` does not access `DbContext` or migration
APIs directly.

The connection string and startup flag are bound into `BlogDatabaseOptions`.
Repository services consume `IOptions<BlogDatabaseOptions>` instead of receiving
raw `IConfiguration`.

## Native SQLite Security Pin

The provider graph previously resolved `SQLitePCLRaw.lib.e_sqlite3` `2.1.11`, which
was reported with advisory `GHSA-2m69-gcr7-jv3q`. The project therefore pins
`SQLitePCLRaw.lib.e_sqlite3` to `3.53.3` in `Directory.Packages.props`.

This pin is a security control, not a version preference. It must be reviewed when
EF Core or the SQLite provider changes its dependency graph.

## Consequences

- Development requires no database server.
- Integration tests exercise a relational provider and foreign keys.
- The native SQLite package must be monitored for compatibility and security.
- Local startup can create or update the schema without a separate command.
- Multi-instance production deployments must review migration locking and may use
  an explicit deployment migration step instead.
- Production deployment is not implied by this decision; the challenge scope is
  local and test-oriented.

## Validation

Run these commands after changing EF Core or SQLite packages:

```bash
dotnet restore BloggingBackend.slnx
dotnet build BloggingBackend.slnx --no-restore --warnaserror
dotnet test BloggingBackend.slnx --no-build --no-restore
dotnet list BloggingBackend.slnx package --vulnerable --include-transitive
```

The current validation reports no vulnerable packages.

## References

- [EF Core overview](https://learn.microsoft.com/en-us/ef/core/)
- [EF Core database providers](https://learn.microsoft.com/en-us/ef/core/providers/?tabs=dotnet-core-cli)
- [EF Core SQLite provider](https://learn.microsoft.com/en-us/ef/core/providers/sqlite/)
- [NuGet Central Package Management](https://learn.microsoft.com/en-us/nuget/consume-packages/central-package-management)
- [SQLite security advisory](https://github.com/advisories/GHSA-2m69-gcr7-jv3q)
