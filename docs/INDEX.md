# Documentation index

## Repository summary

**NuciCLI.Arguments** — Minimal .NET 10.0 command-line argument parser.

- **Purpose:** Parse `--name value` arguments into typed values
- **Scope:** Intentionally minimal — no flags, positionals, subcommands, help generation
- **Target:** net10.0
- **License:** GPL-3.0-or-later
- **Package:** NuGet.org (`NuciCLI.Arguments`)
- **CI/CD:** GitHub Actions (build, test, NuGet publish, GitHub Release)

## Root document map

| Document | Purpose |
|----------|---------|
| [README.md](../README.md) | Project overview, install, basic usage, links |
| [SECURITY.md](../SECURITY.md) | Vulnerability reporting, security policy |
| [CONTRIBUTING.md](../CONTRIBUTING.md) | Contribution guidelines, development setup |
| [CODE_OF_CONDUCT.md](../CODE_OF_CONDUCT.md) | Community standards |
| [LICENSE](../LICENSE) | GPL-3.0-or-later full text |
| [PRIVACY.md](../PRIVACY.md) | Data handling (none collected) |
| [ARCHITECTURE.md](../ARCHITECTURE.md) | High-level architecture (template-generated) |
| [ROADMAP.md](../ROADMAP.md) | Future plans (template-generated) |

## Documentation catalogue

### Core reference
| File | Description |
|------|-------------|
| [repository-overview.md](repository-overview.md) | High-level summary, purpose, scope |
| [repository-structure.md](repository-structure.md) | Directory/file map with descriptions |
| [architecture.md](architecture.md) | Component diagram, layers, data flow |
| [data-model.md](data-model.md) | Class definitions, relationships, serialization |
| [invariants.md](invariants.md) | System-wide and component invariants |

### Components
| File | Description |
|------|-------------|
| [components/argument-parser.md](components/argument-parser.md) | `ArgumentParser` class, registration, parsing |
| [components/argument.md](components/argument.md) | `Argument` immutable data class |
| [components/arguments-collection.md](components/arguments-collection.md) | `ArgumentsCollection` dictionary wrapper |

### Flows
| File | Description |
|------|-------------|
| [flows/argument-parsing.md](flows/argument-parsing.md) | Two-pass parsing algorithm, token flow |

### Operations
| File | Description |
|------|-------------|
| [configuration.md](configuration.md) | Build config, csproj, solution, nullable/implicit usings |
| [testing.md](testing.md) | Unit tests (NUnit), architecture tests (ArchUnitNET, Roslyn) |
| [build-and-deployment.md](build-and-deployment.md) | CI/CD workflows, NuGet publish, GitHub Releases |
| [dependencies.md](dependencies.md) | Production (NuciExtensions) and dev dependencies |

### Design
| File | Description |
|------|-------------|
| [design-decisions.md](design-decisions.md) | ADRs: minimal scope, net10.0, standard exceptions, etc. |
| [error-handling.md](error-handling.md) | Exception taxonomy, handling patterns, messages |
| [logging.md](logging.md) | No logging framework (rationale) |
| [security.md](security.md) | Threat model, controls, dependency security |
| [concurrency-and-scheduling.md](concurrency-and-scheduling.md) | Single-threaded, no async, no locks |
| [state-and-persistence.md](state-and-persistence.md) | Stateless, no persistence |
| [integrations.md](integrations.md) | External systems (none), NuGet/framework integration |

### Guides
| File | Description |
|------|-------------|
| [quick-start.md](quick-start.md) | Install, basic usage, complete example |
| [faq.md](faq.md) | Common questions (features, usage, errors) |
| [troubleshooting.md](troubleshooting.md) | Error causes and fixes, build/runtime issues |
| [change-guide.md](change-guide.md) | Safe modification patterns for each area |
| [documentation-maintenance.md](documentation-maintenance.md) | Doc structure, update triggers, validation |
| [ambiguities-and-open-questions.md](ambiguities-and-open-questions.md) | Design ambiguities, feature requests, tech debt |
| [api-usage-examples.md](api-usage-examples.md) | C# usage patterns for all scenarios |

## Navigation aids

### By audience
- **New users:** [quick-start.md](quick-start.md) → [faq.md](faq.md) → [api-usage-examples.md](api-usage-examples.md)
- **Contributors:** [contributing.md](../CONTRIBUTING.md) → [change-guide.md](change-guide.md) → [testing.md](testing.md)
- **Maintainers:** [build-and-deployment.md](build-and-deployment.md) → [architecture.md](architecture.md) → [invariants.md](invariants.md)
- **Security reviewers:** [security.md](security.md) → [SECURITY.md](../SECURITY.md) → [dependencies.md](dependencies.md)

### By topic
- **Parsing logic:** [flows/argument-parsing.md](flows/argument-parsing.md) → [components/argument-parser.md](components/argument-parser.md)
- **Error handling:** [error-handling.md](error-handling.md) → [troubleshooting.md](troubleshooting.md)
- **Architecture rules:** [architecture.md](architecture.md) → [testing.md](testing.md) → [invariants.md](invariants.md)
- **Release process:** [build-and-deployment.md](build-and-deployment.md) → [change-guide.md](change-guide.md)

## Maintenance metadata

- **Last updated:** 2025-01-15
- **Version:** 1.0.0 (matches package version)
- **Owner:** Repository maintainers
- **Review cycle:** Quarterly (aligned with release cycle)
- **Generated files:** INDEX.md (this file), ARCHITECTURE.md, ROADMAP.md (template-generated)
- **Source of truth:** Source code in `NuciCLI.Arguments/`, tests in `*.Tests/`, workflows in `.github/workflows/`

## Link validation

All internal links use relative paths from `docs/` directory. Verify with:
```bash
# From repo root
find docs -name "*.md" -exec grep -l "](../" {} \;
```

No external links in documentation (except root documents referencing GitHub/NuGet).