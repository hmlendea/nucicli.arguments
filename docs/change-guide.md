# Change guide

## Modifying argument parsing logic

### File: `NuciCLI.Arguments/ArgumentParser.cs`

**ParseArgs method** — Core parsing algorithm. Two phases:
1. Token consumption loop (validates format, looks up args, consumes values)
2. Default application loop (applies defaults, enforces required)

**Changes requiring caution:**
- Token format validation (line with `StartsWith("--")`)
- Dictionary lookup for registered arguments
- Queue consumption for values
- Exception types and messages
- Default application order

**Test after changes:** Run unit tests + architecture tests. Add tests for new error cases.

### File: `NuciCLI.Arguments/Argument.cs`

**Immutable data class.** Primary constructor with init-only properties.

**Safe changes:**
- Add new properties (with defaults)
- Change `Description` default
- Add validation in constructor (throw early)

**Breaking changes:**
- Remove/rename properties
- Change constructor signature
- Make properties mutable

### File: `NuciCLI.Arguments/ArgumentsCollection.cs`

**Dictionary wrapper.** Indexer, `Has`, `Get<T>`, `IEnumerable`.

**Safe changes:**
- Add new query methods (e.g., `TryGet<T>`)
- Add `Count`, `Keys` properties

**Breaking changes:**
- Change indexer behavior
- Change `Get<T>` exception types
- Remove `IEnumerable` implementation

## Adding features

### New argument types (flags, positionals, subcommands)

**Impact:** Major. Requires changes to:
- `Argument` class (new properties/kinds)
- `ArgumentParser.ParseArgs` (new parsing logic)
- `ArgumentsCollection` (new storage/retrieval)
- All tests

**Architecture tests will fail:** Namespace, type, and convention rules.

**Process:**
1. Design API first
2. Update architecture tests for new types
3. Implement in parser
4. Add unit tests
5. Update documentation

### Help generation

**New feature.** Would need:
- `Argument` property for help text (already has `Description`)
- `ArgumentParser.GenerateHelp()` method
- Formatting logic

**Safe:** Doesn't affect parsing. Add as extension method or new class.

## Modifying tests

### Unit tests (`NuciCLI.Arguments.UnitTests`)

**Add tests for:**
- New error conditions
- New argument types
- Edge cases (empty input, many args, etc.)

**Pattern:** Follow existing `ArgumentParserTests.cs` style. Use NUnit attributes.

### Architecture tests (`NuciCLI.Arguments.ArchitectureTests`)

**Compiled rules** (`ProductionArchitectureTests.cs`):
- Add rules for new types/namespaces
- Update allowed dependencies

**Source conventions** (`SourceFileConventionTests.cs`):
- New conventions need new test methods
- Use Roslyn APIs

**Run after changes:** `dotnet test` — all must pass.

## Modifying build/packaging

### `.csproj` files

**NuciCLI.Arguments.csproj:**
- Target framework changes
- Package metadata (version, description, tags)
- Dependencies

**Test projects:**
- Test framework versions
- Analyzer packages

### Solution (`.slnx`)

**Add projects:** Edit XML directly or use `dotnet sln add`.

### GitHub Actions (`.github/workflows/`)

**dotnet.yml:** CI triggers, build/test commands.
**nuget-release.yml:** Pack/publish on release.
**github-release.yml:** Asset upload on release.

**Changes:** Test in fork first. YAML syntax errors break CI.

## Versioning

**Scheme:** Semantic versioning (Major.Minor.Patch)
- Major: Breaking API changes
- Minor: New features, backward compatible
- Patch: Bug fixes

**Set in:** `NuciCLI.Arguments.csproj` `<Version>` property.

**Release process:**
1. Update version
2. Commit
3. Tag release (triggers workflows)
4. Workflows publish to NuGet.org and GitHub Releases

## Documentation updates

**When changing public API:**
1. Update relevant `.md` in `docs/`
2. Update `INDEX.md` if new file added
3. Update `README.md` if overview changes

**Files to check:**
- `docs/components/argument-parser.md`
- `docs/components/argument.md`
- `docs/components/arguments-collection.md`
- `docs/flows/argument-parsing.md`
- `docs/quick-start.md`
- `docs/faq.md`