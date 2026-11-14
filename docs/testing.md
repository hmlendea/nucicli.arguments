# Testing

## Test strategy

Three test layers with distinct purposes:

| Layer | Project | Framework | Purpose |
|-------|---------|-----------|---------|
| Unit | `NuciCLI.Arguments.UnitTests` | NUnit 5.x | Behavioural correctness of parsing logic |
| Architecture | `NuciCLI.Arguments.ArchitectureTests` | NUnit + ArchUnitNET + Roslyn | Structural constraints, conventions, dependencies |
| Integration | None | N/A | Not applicable (no external dependencies) |

## Unit tests

**Project:** `NuciCLI.Arguments.UnitTests`
**Files:** `ArgumentParserTests.cs`, `ArgumentTests.cs`, `ArgumentsCollectionTests.cs`

### Current coverage

| Fixture | Tests | Scenarios |
|---------|-------|-----------|
| `ArgumentParserTests` | 10 | Happy path, unknown argument, missing `--` prefix, missing value, missing required argument, default value applied, omitted argument without default, duplicate registration, case sensitivity, repeated parsing |
| `ArgumentTests` | 3 | Property exposure, null default value, reference equality |
| `ArgumentsCollectionTests` | 12 | Indexer get/set, `Has()` including null values, `Get<T>()` success and failures, enumeration |

### Remaining gaps

- Thread-safety behaviour (documented as not thread-safe)
- Performance characteristics for very large token arrays

### Test conventions

- **Naming:** `Given[Context]_When[Action]_Then[Outcome]`
- **Framework:** NUnit 5.x with `Assert.That()` constraint model
- **Setup:** `[SetUp] SetUp()` creates a fresh `ArgumentParser` per test
- **No Moq used** — parser has no external dependencies to mock
- **No `InternalsVisibleTo`** — tests only exercise public API

## Architecture tests

**Project:** `NuciCLI.Arguments.ArchitectureTests`

### ProductionArchitectureTests.cs

| Test | Rule |
|------|------|
| `GivenProductionProject_WhenChecked_ThenItsSourceGraphHasNoCycles` | No dependency cycles in production source (ArchUnitNET) |
| `GivenProductionAssembly_WhenLoaded_ThenItContainsExpectedTypes` | Assembly contains `Argument`, `ArgumentParser`, `ArgumentsCollection` |
| `GivenProductionTypes_WhenChecked_ThenTheyUseTheProductionNamespace` | All public types in `NuciCLI.Arguments` namespace |
| `GivenProductionAssembly_WhenChecked_ThenItDoesNotExposeInternalsToTests` | No `InternalsVisibleTo` attributes |
| `GivenProductionProject_WhenChecked_ThenItHasNoTestProjectReferences` | Production csproj doesn't reference test projects |
| `GivenProductionProject_WhenChecked_ThenItTargetsNet10` | Target framework is `net10.0` |
| `GivenProductionProject_WhenChecked_ThenItEnablesNullableAndDisablesImplicitUsings` | `<Nullable>enable</Nullable>`, `<ImplicitUsings>disable</ImplicitUsings>` |

### SourceFileConventionTests.cs

| Test | Rule |
|------|------|
| `GivenProductionSourceFiles_WhenChecked_ThenEveryFileHasExactlyOneDeclaredType` | One type per `.cs` file (Roslyn) |
| `GivenProductionSourceFiles_WhenChecked_ThenTypeNamesMatchFileNames` | Type name matches filename stem |
| `GivenProductionSourceFiles_WhenChecked_ThenAllTypesUseExplicitAccessibility` | All types declare `public`/`internal`/etc. |
| `GivenProductionSourceFiles_WhenChecked_ThenControlFlowBodiesUseBraces` | All `if`/`for`/`while` bodies use braces |

### Architecture test conventions

- **Never modified by agents** — Only changed when user explicitly requests
- **Cross-platform** — No OS-specific paths, uses project metadata for paths
- **Explicit assembly loading** — Loads production assembly via `typeof(Argument).Assembly`
- **Non-vacuous selectors** — Tests fail if selector matches zero types
- **Separate fixtures** — Dependency rules vs source conventions in separate classes

## Running tests

```bash
# All tests
dotnet test --no-build --verbosity normal

# Unit tests only
dotnet test NuciCLI.Arguments.UnitTests --no-build --verbosity normal

# Architecture tests only
dotnet test NuciCLI.Arguments.ArchitectureTests --no-build --verbosity normal
```

## CI integration

`.github/workflows/dotnet.yml` runs `dotnet test --no-build --verbosity normal` on every push/PR to master.

`.github/workflows/security.yml` also builds the solution on every push/PR to master (and weekly) as part of the CodeQL analysis, but does not execute the test suite.

## Test data

Uses standard test values from repository conventions:
- Names: `John Doe`, `John Wick`, `Solaire of Astora`
- Ages: `32`, `42` (as strings)
- No external fixtures or data files

## Coverage

No code coverage enforcement configured. `<ExcludeFromCodeCoverage>true</ExcludeFromCodeCoverage>` on test projects.