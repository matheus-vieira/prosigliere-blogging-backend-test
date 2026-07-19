# .NET 10 and Solution Decisions

## Context

The challenge asks for a production-oriented backend with a short implementation
window. The project needs a supported runtime, reproducible tooling, and a solution
format that works consistently in the .NET CLI and modern IDEs.

## Decision: .NET 10

Target `net10.0` for the API and test projects. Use the stable .NET 10 SDK declared
in `global.json` and keep the target framework aligned across projects.

This provides a single supported runtime, ASP.NET Core hosting model, dependency
injection, configuration, logging, and test tooling without introducing a second
platform abstraction.

## Decision: `global.json`

Pin the development SDK to `10.0.110` with patch-level roll-forward. This prevents
local machines and CI runners from silently selecting incompatible SDK behavior.
Patch roll-forward remains allowed for security and servicing updates within the
same SDK feature band.

## Decision: `BloggingBackend.slnx`

Use the modern `.slnx` solution format instead of the legacy `.sln` format. The
solution groups the API and test projects under `src` and `tests`, and is operated
through the .NET CLI:

```bash
dotnet sln BloggingBackend.slnx list
dotnet build BloggingBackend.slnx
```

The format is intentionally simple and avoids solution metadata unrelated to the
project structure.

## Alternatives Considered

- .NET 8: supported, but not the requested .NET 10 target.
- Multi-targeting: unnecessary for this focused challenge and increases build/test
  complexity.
- Legacy `.sln`: supported, but less aligned with the requested modern Microsoft
  solution format.

## Consequences

- Contributors need a .NET 10 SDK.
- CI must use a compatible .NET 10 image or runner.
- The SDK pin must be reviewed when the repository adopts a new feature band.

## Validation

```bash
dotnet --version
dotnet sln BloggingBackend.slnx list
dotnet build BloggingBackend.slnx --warnaserror
```

## References

- [.NET downloads](https://dotnet.microsoft.com/download/dotnet/10.0)
- [.NET SDK `global.json` overview](https://learn.microsoft.com/en-us/dotnet/core/tools/global-json)
- [Solution file format](https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-sln)
