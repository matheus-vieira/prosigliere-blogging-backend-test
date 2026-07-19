# Layered Architecture Decision

## Context

The API has a small functional scope, but the code must remain testable and ready
for incremental features. The design needs clear ownership without introducing
microservices, generic repositories, or abstractions that the challenge does not
need.

## Decision: N-Layer Structure

Use a small N-layer architecture with three application layers:

```text
API -> Domain -> Repository
```

The implementation may use separate projects or clearly separated folders while
the baseline is small. The intended responsibilities are:

- **API**: HTTP routes, request/response DTOs, serialization, status codes, and
  dependency injection composition.
- **Domain**: entities, value rules, use-case contracts, validation rules, and
  business behavior. The domain must not depend on ASP.NET Core or EF Core.
- **Repository**: `DbContext`, EF configurations, migrations, queries, and SQLite
  integration. This layer implements persistence contracts without leaking EF
  entities through the HTTP boundary.

The dependency direction is inward: API may depend on Domain contracts, and the
Repository adapts persistence to Domain contracts. Domain remains independent of
both outer layers.

## Composition Extension Flow

The API composition root exposes one high-level entry point:

```csharp
builder.AddBloggingDomain();
```

The call is a façade for layer registration. It invokes Domain registration, and
Domain invokes a Repository registration callback. `Program.cs` must not call
`AddBloggingRepository`, configure `DbContext`, or manipulate migration details.
Application startup uses one `UseBloggingApiAsync` call, which delegates database
initialization to `UseBloggingAsync` and maps all feature endpoints internally.

The intended flow is:

```text
Program.cs
  -> API.AddBloggingDomain()
      -> Domain.AddBloggingDomain()
          -> Repository registration callback
  -> API.UseBloggingApiAsync()
      -> API.UseBloggingAsync()
          -> Repository.UseBloggingDatabaseAsync()
      -> API.MapBloggingEndpoints()
```

The callback avoids a project-reference cycle while keeping the composition flow
owned by Domain. Domain business objects remain independent from EF Core and the
callback is only a startup registration concern.

## Extension Organization

Extension methods are grouped by receiver and kept one method per file:

```text
API/DependencyInjection/
  WebApplicationBuilderExtensions/
  WebApplicationExtensions/
Domain/DependencyInjection/
  ServiceCollectionExtensions/
Repository/DependencyInjection/
  ServiceCollectionExtensions/
  ServiceProviderExtensions/
```

This structure makes the composition surface discoverable and avoids a large
catch-all extension class. The same rule applies to test-specific extensions when
they become necessary.

## Configuration Boundary

The composition boundary may read `IConfiguration` once, but Repository services
consume strongly typed `BlogDatabaseOptions` through `IOptions<T>`. This keeps
connection-string knowledge and startup flags out of business code.

Repository owns options binding and validation, `BlogDbContext` registration, the
SQLite provider configuration, and `UseBloggingDatabaseAsync`. The API owns only
the orchestration extensions and does not know table mappings or EF query behavior.

## Startup Migration Policy

The local challenge application may apply pending migrations during startup through
`Database.GetPendingMigrationsAsync` followed by `Database.MigrateAsync` when the
database is behind the model. This makes startup initialization idempotent without
an application option that duplicates database state. Production deployments must
review concurrency, locking, rollback, and permissions before using startup
migrations; an explicit deployment migration step may be safer for multi-instance
deployments.

## Alternatives Considered

- Minimal API with all logic in `Program.cs`: rejected because it couples business
  behavior to HTTP and makes focused tests harder.
- Generic repository over EF Core: rejected because it hides useful query behavior
  and adds abstraction without a requirement.
- Microservices: rejected because the challenge is a single bounded application.
- Full Clean Architecture template: deferred because its ceremony exceeds the
  current scope.
- Passing raw `IConfiguration` through Domain services: rejected because it leaks
  infrastructure concerns into business code.
- Calling Repository directly from `Program.cs`: rejected because it duplicates
  composition knowledge and bypasses the Domain façade.

## Consequences

- Business rules can be tested without an HTTP server.
- Persistence integration can be tested against SQLite without mocking EF behavior.
- New dependencies must respect the layer direction.
- A project-reference cycle between Domain and Repository is prohibited. Any
  concrete registration bridge must be isolated rather than solved with circular
  references or reflection.
- Small files and feature-oriented folders remain preferred over large global
  technical folders.

## Validation

- Review project references for forbidden dependency direction.
- Keep domain tests independent from ASP.NET Core and EF Core.
- Keep repository tests on a real disposable SQLite database.
- Keep API tests focused on observable HTTP behavior.
