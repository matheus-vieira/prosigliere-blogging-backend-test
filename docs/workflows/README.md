# GitHub Workflows

This project uses separate GitHub Actions workflows for quality gates. Keeping
these responsibilities separate makes each check visible, independently
reviewable, and suitable for branch protection.

## Workflow Overview

### .NET Quality Gates

File: `.github/workflows/ci.yml`

This workflow runs for pull requests when they are opened, reopened, or
synchronized, and for pushes to `main`. It has five ordered jobs:

1. `restore` restores `BloggingBackend.slnx`.
2. `format` verifies formatting with `dotnet format`.
3. `build` compiles with `--warnaserror`.
4. `test` restores and builds on its own runner, then runs the tests.
5. `package-audit` checks direct and transitive packages for vulnerabilities.

Every job uses `ubuntu-latest`, checks out the repository, selects the SDK from
the required `10.0.110` version, and has only the permissions needed by the
workflow. The workflow does not start the API, create the development database,
deploy, or publish artifacts.

## Why Commands Repeat

The jobs run on isolated GitHub-hosted runners. `needs` controls job ordering,
but it does not transfer `bin/`, `obj/`, NuGet caches, or other generated files
between runners. Each job therefore restores its own dependencies, and the
`test` job builds locally before using `--no-build`.

This repetition is intentional. It makes every gate deterministic and safe to
run on a clean runner. Sharing artifacts, relying on a cache, or combining jobs
could reduce execution time, but would add coupling and should only be adopted
after a measured performance need.

## Follow-up Documentation

The Conventional Commits workflow is implemented separately in PR #12. After
this PR is approved and merged, synchronize that branch with `main` and extend
this README with the title and commit-message validation details. This order
keeps the documentation aligned with the files already present in each merged
revision.

Merge into `main` remains a human-controlled action. Do not bypass a failed
check or configure branch protection before the workflows have been reviewed.
