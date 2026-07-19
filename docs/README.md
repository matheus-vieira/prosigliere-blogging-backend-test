# Project Documentation

This directory contains the project documentation. The root README is intentionally
focused on the project purpose and execution instructions; implementation details
and architectural decisions belong here.

## Documentation Rules

- Documentation files use en-US.
- Each documentation directory contains one `README.md` as its entry point.
- New topics are represented by a directory named after the topic, with its own
  `README.md`.
- Architectural decisions use the format: context, decision, alternatives,
  consequences, and validation.
- Security, dependency, and provider decisions must include their source links and
  the validation command used.

## Structure

- [`archtectural-decision-records/`](archtectural-decision-records/): decisions
  that affect architecture, dependencies, and infrastructure.

## Related Documentation

- [EF Core decisions](archtectural-decision-records/ef/)
- [SQLite decisions](archtectural-decision-records/ef/sqlite/)
- [API bootstrap decisions](archtectural-decision-records/api/)
