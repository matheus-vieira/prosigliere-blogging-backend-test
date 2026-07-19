# Entity Framework Core Decisions

This section documents decisions related to Entity Framework Core 10, its database
providers, model configuration, migrations, and persistence boundaries.

## Decisions

- [`sqlite/`](sqlite/): SQLite provider selection and native package security pin.

## EF Core Baseline

- Use the Microsoft-maintained `Microsoft.EntityFrameworkCore.Sqlite` provider.
- Keep EF Core packages on the same major and patch version.
- Reference `Microsoft.EntityFrameworkCore.Relational` directly so relational
  patch updates are explicit and centrally managed.
- Keep package versions in the repository-level `Directory.Packages.props`.
- Keep build analyzers and compiler settings in `Directory.Build.props`.
- Apply entity mappings through the project `IEntityTypeConfigurator` abstraction
  from `OnModelCreating`, keeping EF configuration discoverable and testable.
- Validate provider changes with restore, build, integration tests, and package
  vulnerability scanning.

## Official References

- [EF Core overview](https://learn.microsoft.com/en-us/ef/core/)
- [Database providers](https://learn.microsoft.com/en-us/ef/core/providers/?tabs=dotnet-core-cli)
