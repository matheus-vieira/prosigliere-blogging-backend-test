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

## Current Baseline

The first branch intentionally contains only the API host and test project. The
Domain and Repository boundaries will be introduced with the corresponding
features, without prematurely creating empty projects or abstractions.

## Alternatives Considered

- Minimal API with all logic in `Program.cs`: rejected because it couples business
  behavior to HTTP and makes focused tests harder.
- Generic repository over EF Core: rejected because it hides useful query behavior
  and adds abstraction without a requirement.
- Microservices: rejected because the challenge is a single bounded application.
- Full Clean Architecture template: deferred because its ceremony exceeds the
  current scope.

## Consequences

- Business rules can be tested without an HTTP server.
- Persistence integration can be tested against SQLite without mocking EF behavior.
- New dependencies must respect the layer direction.
- Small files and feature-oriented folders remain preferred over large global
  technical folders.

## Validation

- Review project references for forbidden dependency direction.
- Keep domain tests independent from ASP.NET Core and EF Core.
- Keep repository tests on a real disposable SQLite database.
- Keep API tests focused on observable HTTP behavior.
