# Central Package Management Decision

## Context

The solution contains an API project and a test project that share ASP.NET Core,
EF Core, and testing dependencies. Floating versions in individual project files
make restores less reproducible and allow related packages to drift apart.

## Decision: NuGet CPM

Use NuGet Central Package Management through the repository-root
`Directory.Packages.props` file:

- Set `ManagePackageVersionsCentrally` to `true`.
- Define every package version with `<PackageVersion>` in the central file.
- Keep `<PackageReference>` entries in `.csproj` files without `Version` attributes.
- Use exact versions for reproducible restores.
- Enable `CentralPackageTransitivePinningEnabled` for reviewed security pins.

`Directory.Packages.props` owns dependency versions. `Directory.Build.props` owns
compiler, analyzer, nullable, style, and warning behavior. Keeping these concerns
separate is intentional.

## Security Pin

`SQLitePCLRaw.lib.e_sqlite3` is centrally pinned to `3.53.3` because the provider
graph previously resolved a vulnerable `2.1.11`. The pin is documented in the
[SQLite ADR](../ef/sqlite/README.md) and must be revalidated after provider updates.

## Alternatives Considered

- Versions in each `.csproj`: rejected because they drift across projects.
- Floating versions such as `10.*`: rejected for reproducibility and auditability.
- A third-party package manager: rejected because NuGet CPM is native to the .NET
  toolchain and meets the requirement.

## Consequences

- Adding a package requires updating the central file and the consuming project.
- Version upgrades are visible in one reviewable diff.
- Transitive pinning must be reviewed to avoid unexpected dependency promotion.
- Multiple package sources require package source mapping or a single source to
  avoid CPM warning `NU1507`.

## Validation

```bash
dotnet restore BloggingBackend.slnx
dotnet list BloggingBackend.slnx package --include-transitive
dotnet list BloggingBackend.slnx package --vulnerable --include-transitive
```

## Reference

- [NuGet Central Package Management](https://learn.microsoft.com/en-us/nuget/consume-packages/central-package-management)
