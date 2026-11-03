# Invariants

## System-wide invariants

These properties must remain true for the library to function correctly.

### 1. Unique argument names

**Invariant:** Every registered argument has a unique name within an `ArgumentParser` instance.

**Enforcement:** `Dictionary<string, Argument>` keyed by name. Duplicate `AddArgument` calls overwrite previous registration (last-write-wins).

**Consequence:** No way to register two arguments with same name. Consumers must use distinct names.

### 2. Parse atomicity

**Invariant:** `ParseArgs` either returns a fully populated `ArgumentsCollection` or throws. Never returns partial results.

**Enforcement:** All validation happens before return. Exceptions thrown during parsing or default application.

**Consequence:** Consumers never receive incomplete parse results.

### 3. Required argument enforcement

**Invariant:** If `Argument.IsRequired == true` and argument not provided in tokens, `ParseArgs` throws `ArgumentNullException`.

**Enforcement:** Post-loop default application phase checks `IsRequired` before applying default.

**Consequence:** Required arguments always present in returned collection (or exception thrown).

### 4. Unknown argument rejection

**Invariant:** Any token with `--` prefix not matching a registered argument name causes `ArgumentException`.

**Enforcement:** Dictionary lookup during token loop.

**Consequence:** No silent ignoring of typos or unregistered arguments.

### 5. Value presence for provided arguments

**Invariant:** Every argument explicitly provided in tokens has a non-null value in the result.

**Enforcement:** Queue emptiness check before value consumption. `ArgumentNullException` if no value token follows.

**Consequence:** Parsed values are never null for explicitly provided arguments.

### 6. Default application for missing optional arguments

**Invariant:** Every registered optional argument not provided in tokens receives its `DefaultValue` (possibly null) in the result.

**Enforcement:** Post-loop iteration over all registered arguments.

**Consequence:** `Has()` returns true for all registered arguments after successful parse.

### 7. Case-sensitive names

**Invariant:** Argument names are case-sensitive. `--Name` and `--name` are different arguments.

**Enforcement:** `Dictionary<string, Argument>` uses ordinal case-sensitive comparison.

**Consequence:** Consumers must match exact case in registration and usage.

### 8. Namespace consistency

**Invariant:** All public types reside in `NuciCLI.Arguments` namespace.

**Enforcement:** Architecture test `GivenProductionTypes_WhenChecked_ThenTheyUseTheProductionNamespace`.

**Consequence:** Single `using NuciCLI.Arguments;` sufficient for all public API.

### 9. No InternalsVisibleTo

**Invariant:** Production assembly exposes no internals to test assemblies.

**Enforcement:** Architecture test `GivenProductionAssembly_WhenChecked_ThenItDoesNotExposeInternalsToTests`.

**Consequence:** Tests exercise only public API. No test-only internal access.

### 10. No test project references in production

**Invariant:** Production project does not reference test projects.

**Enforcement:** Architecture test `GivenProductionProject_WhenChecked_ThenItHasNoTestProjectReferences`.

**Consequence:** Production package contains no test dependencies.

## Component invariants

### ArgumentParser

- `arguments` dictionary never null after construction
- `AddArgument` always succeeds (no validation of name/help)
- `ParseArgs` creates new `ArgumentsCollection` per call (no state reuse)

### Argument

- All properties immutable after construction
- `Name` never null (primary constructor parameter)
- `Description` never null (defaults to empty string)
- `DefaultValue` may be null

### ArgumentsCollection

- `_values` dictionary never null after construction
- Indexer get returns null for missing keys (no exception)
- `Get<T>` throws for missing keys (no null return)
- `Has` returns true even for null values

## Cross-component invariants

- `ArgumentParser.ParseArgs` only stores values for registered argument names
- `ArgumentsCollection` keys always match registered `Argument.Name` values
- Default values stored with original type (not converted to string)
- Parsed values always stored as string (from token consumption)