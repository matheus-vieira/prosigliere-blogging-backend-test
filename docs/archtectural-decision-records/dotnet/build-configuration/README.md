# Build Configuration Decision

## Context

The API and test projects must use the same compiler, analyzer, nullable, and code
style rules. Duplicating those settings in every `.csproj` creates drift and makes
local validation differ from CI.

## Decision: `Directory.Build.props`

Keep repository-wide build behavior in `Directory.Build.props`. The file enables:

- Nullable reference types.
- Implicit global usings.
- .NET analyzers at the latest recommended analysis level.
- Code style enforcement during builds.
- Warnings treated as errors.

Project files should contain only project-specific settings and package references.
They must not repeat these common properties without a documented exception.

## Decision: `.editorconfig`

Keep source formatting and analyzer severities in the repository root `.editorconfig`.
The same file is consumed by IDEs, `dotnet format`, and code review tooling.
Technical code and comments use en-US according to the project language policy.

## Alternatives Considered

- Per-project properties: rejected because they allow configuration drift.
- CI-only validation: rejected because developers should reproduce failures locally.
- A third-party analyzer suite: deferred until a concrete requirement exists.

## Consequences

- A new project automatically inherits the baseline quality rules.
- Existing warnings must be resolved or explicitly justified before merging.
- Generated files and intentional exceptions must be excluded narrowly, never by
  weakening the repository-wide policy.

## Validation

```bash
dotnet format BloggingBackend.slnx --no-restore --verify-no-changes
```

## Related Decisions

- [Central Package Management](../../nuget/README.md)
- [.NET 10 and solution format](../README.md)
