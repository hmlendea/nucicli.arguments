# Documentation maintenance

## Documentation structure

```
docs/
├── INDEX.md                          # Master index (generated last)
├── repository-overview.md            # High-level summary
├── repository-structure.md           # Directory/file map
├── architecture.md                   # Architecture overview
├── data-model.md                     # Data structures
├── components/
│   ├── argument-parser.md
│   ├── argument.md
│   └── arguments-collection.md
├── flows/
│   └── argument-parsing.md
├── configuration.md                  # Build/config
├── testing.md                        # Test strategy
├── build-and-deployment.md           # CI/CD
├── dependencies.md                   # Dependencies
├── design-decisions.md               # ADRs
├── error-handling.md                 # Exceptions
├── logging.md                        # Logging (none)
├── security.md                       # Security details
├── invariants.md                     # System invariants
├── concurrency-and-scheduling.md     # Threading
├── state-and-persistence.md          # State
├── integrations.md                   # External systems
├── faq.md                            # FAQ
├── troubleshooting.md                # Common issues
├── quick-start.md                    # Getting started
├── change-guide.md                   # Modification guide
├── documentation-maintenance.md      # This file
├── ambiguities-and-open-questions.md # Unresolved items
└── api-usage-examples.md             # Examples (N/A)
```

## Maintenance principles

1. **Docs live with code** — Update docs in same PR as code changes
2. **Single source of truth** — Code is authoritative; docs reflect code
3. **No duplication** — Link instead of copying
4. **Template fidelity** — Follow GitHub markdown templates exactly

## When to update docs

| Trigger | Files to update |
|---------|-----------------|
| New public type/method | Component file + INDEX.md |
| Parsing behavior change | flows/argument-parsing.md, error-handling.md |
| New exception type | error-handling.md, troubleshooting.md |
| New dependency | dependencies.md, integrations.md |
| Architecture rule change | architecture.md, testing.md |
| Build process change | build-and-deployment.md, configuration.md |
| Security consideration | security.md |
| New FAQ entry | faq.md |
| Breaking change | change-guide.md, quick-start.md |

## Updating INDEX.md

**Always update last** — depends on all other files.

Sections to maintain:
1. Repository summary (from repository-overview.md)
2. Root document map (README.md, SECURITY.md, etc.)
3. Documentation catalogue (all docs/ files with 1-line descriptions)
4. Navigation aids (by audience, by topic)
5. Maintenance metadata (last updated, version, owner)

## Validation checklist

Before merging doc changes:
- [ ] All internal links resolve (relative paths)
- [ ] Code snippets compile (copy-paste test)
- [ ] No stale references to deleted/renamed files
- [ ] INDEX.md includes new/changed files
- [ ] Root documents (README.md, etc.) not duplicated
- [ ] Template prose unchanged (only placeholders)

## Tooling

- **Markdown lint:** Not configured (add if needed)
- **Link check:** Manual or CI step
- **Spell check:** Not configured
- **Preview:** VS Code Markdown preview

## Ownership

- **Primary:** Repository maintainers
- **Reviews:** All PRs touching docs require doc review
- **Stale detection:** Quarterly review of "last updated" dates

## Versioning

Docs versioned with code (same git history). No separate versioning.

## Archival

Obsolete docs moved to `docs/archive/` with date prefix, not deleted.