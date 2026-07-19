# Architectural Decision Records

This directory contains architectural decisions for the blogging backend test.
Each decision is kept close to the technology or layer it affects.

## Decision Areas

- [`dotnet/`](dotnet/): runtime, solution format, and build configuration.
- [`architecture/`](architecture/): application layer boundaries and dependency
  direction.
- [`nuget/`](nuget/): centralized package version management.
- [`testing/`](testing/): test framework, isolation, mocks, and coverage.
- [`ef/`](ef/): Entity Framework Core provider, model, and migration decisions.

## ADR Format

Every decision README must document:

- Context and problem.
- Decision and scope.
- Alternatives considered.
- Consequences and risks.
- Validation evidence.
- Review or revisit conditions.
