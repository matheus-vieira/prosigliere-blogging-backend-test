# Testing Architecture Decision

## Context

The challenge requires confidence in HTTP behavior, persistence relationships, and
business validation. Tests must be deterministic, isolated, and able to enforce the
85% production coverage requirement without hiding integration defects behind
Mocks.

## Decision: MSTest

Use the Microsoft MSTest framework with `MSTest.TestFramework`,
`MSTest.TestAdapter`, and `Microsoft.NET.Test.Sdk`. MSTest is the selected test
framework because it is maintained and documented by Microsoft, integrates with
the .NET SDK and Microsoft Testing Platform, and supports unit, integration,
data-driven, lifecycle, and parallel test scenarios.

All package versions are centrally managed in `Directory.Packages.props`.

## Test Packages

| Package | Role | Why it is used |
| --- | --- | --- |
| `MSTest.TestFramework` | Test framework | Provides `[TestClass]`, `[TestMethod]`, assertions, lifecycle hooks, test data, and MSTest execution semantics. |
| `MSTest.TestAdapter` | Test adapter | Connects MSTest tests to `dotnet test`, the .NET CLI, and CI test discovery. |
| `Microsoft.NET.Test.Sdk` | Test platform SDK | Provides the Microsoft test host and the standard build/runtime integration used by the adapter. |
| `Microsoft.AspNetCore.Mvc.Testing` | HTTP integration testing | Provides `WebApplicationFactory` to execute the real ASP.NET Core pipeline in-process without an external server. |
| `Moq` | Mocking | Creates strict, focused test doubles for external contracts and replaceable collaborators. |
| `Moq.AutoMock` | Automatic mocking | Provides `AutoMocker` to construct classes with multiple injected dependencies while keeping unit tests readable. |
| `Bogus` | Test data generation | Generates realistic, non-sensitive test data and reduces repetitive fixture setup. Deterministic seeds keep failures reproducible. |
| `coverlet.msbuild` | Code coverage | Instruments production assemblies and enforces the required line and branch coverage thresholds during `dotnet test`. |
| `Microsoft.EntityFrameworkCore.Sqlite` | Relational integration tests | Exercises real EF Core relational behavior, constraints, queries, and SQLite mappings instead of hiding them behind mocks. |

The test project references the API project. EF Core and SQLite references are
transitively available to integration tests, while persistence behavior is tested
against a disposable SQLite database rather than a mocked `DbContext`.

## Test Doubles and Data

- Use Moq for external contracts and replaceable collaborators.
- Use Moq.AutoMock for classes with multiple injectable collaborators.
- Use Bogus with deterministic seeds for generated test data; never use real or
  sensitive data.
- Do not mock `DbContext` when validating EF mapping, constraints, queries, or
  persistence behavior.
- Use assembly/class fixtures only for immutable setup. Each test owns its mutable
  data and does not depend on test order.

## Test Responsibilities

- **Unit tests** use MSTest with Moq, Moq.AutoMock, and Bogus to validate domain
  rules and application behavior in isolation.
- **Integration tests** use MSTest, `WebApplicationFactory`, EF Core, and SQLite to
  validate dependency injection, HTTP routing, serialization, mappings,
  constraints, and persistence behavior.
- **Coverage checks** use Coverlet MSBuild and measure production code only. A high
  percentage does not replace explicit success, edge, and error scenarios.

## Coverage Gate

Use Coverlet MSBuild to enforce both thresholds:

```bash
dotnet test BloggingBackend.slnx \
  /p:CollectCoverage=true \
  /p:CoverletOutputFormat=cobertura \
  /p:Threshold=85 \
  /p:ThresholdType=line,branch \
  /p:ThresholdStat=total
```

The threshold applies to production code. Tests, generated migrations, and
explicitly generated files may be excluded narrowly and must be documented in the
pull request.

## Alternatives Considered

- xUnit: capable, but MSTest is the selected Microsoft-supported framework for this
  project and matches the repository test package policy.
- EF Core InMemory: rejected for persistence tests because it does not reproduce
  relational provider behavior.
- Shared mutable database fixtures: rejected because they create order dependence.
- Percentage-only quality checks: rejected because critical behavior still needs
  explicit success, edge, and error scenarios.

## Consequences

- Unit and integration tests have distinct responsibilities.
- CI must publish coverage evidence and fail below either threshold.
- Test execution may be parallelized only when fixtures are isolated.

## References

- [MSTest documentation](https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-csharp-with-mstest)
- [ASP.NET Core integration testing](https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests)
- [EF Core testing](https://learn.microsoft.com/en-us/ef/core/testing/)
