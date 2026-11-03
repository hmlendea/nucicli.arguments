# Ambiguities and open questions

## Design ambiguities

### 1. Duplicate argument handling
**Current:** Last `AddArgument` call wins (dictionary overwrite).
**Question:** Should this be an error? Throw on duplicate registration?
**Impact:** Breaking change if made strict.

### 2. Case sensitivity
**Current:** Case-sensitive (ordinal comparison).
**Question:** Should case-insensitive option be added?
**Impact:** New API surface, parsing behavior change.

### 3. Value type handling
**Current:** Parsed values stored as strings. Defaults keep original type. `Get<T>` uses `Convert.ChangeType`.
**Question:** Should parsed values be type-converted at parse time?
**Impact:** Earlier failure, but requires type info at registration.

### 4. Null default values
**Current:** `DefaultValue` can be null. Stored as null. `Get<T>` throws `InvalidCastException` for null→value type.
**Question:** Should null defaults be disallowed for value types?
**Impact:** Compile-time vs runtime safety tradeoff.

## Feature requests (not planned)

### 1. Short flags (`-v` vs `--verbose`)
**Status:** Explicitly out of scope. Library intentionally minimal.

### 2. Positional arguments
**Status:** Out of scope. All arguments must be named.

### 3. Subcommands (`git commit`, `docker run`)
**Status:** Out of scope. Flat structure only.

### 4. Boolean flags without value (`--verbose` = true)
**Status:** Out of scope. Requires `--verbose true`.

### 5. Help generation (`--help`)
**Status:** Not implemented. Could be added as separate feature.

### 6. Argument groups / mutually exclusive
**Status:** Not implemented. Consumer responsibility.

### 7. Value validation (range, regex, enum)
**Status:** Not implemented. Consumer responsibility after parsing.

## Technical debt

### 1. No `TryGet<T>` pattern
**Current:** `Get<T>` throws. `Has` + `Get` required for safe access.
**Improvement:** Add `bool TryGet<T>(string key, out T value)`.

### 2. No enumeration of parsed arguments
**Current:** `ArgumentsCollection` implements `IEnumerable<KeyValuePair<string, object>>` but only for registered args with values.
**Question:** Should it include all registered args (with defaults)?

### 3. Exception message consistency
**Current:** `ArgumentException` for format/unknown, `ArgumentNullException` for missing value/required.
**Question:** Unify under custom exception hierarchy?

### 4. No parsing result object
**Current:** Throws on error, returns collection on success.
**Alternative:** Return `ParseResult` with success/failure, errors, warnings.

## Architecture questions

### 1. `NuciExtensions` dependency
**Current:** Used only for `EnumerableExt.IsNullOrEmpty(Queue<string>)`.
**Question:** Replace with inline check to remove dependency?
**Tradeoff:** Dependency vs code duplication.

### 2. Target framework
**Current:** net10.0 only.
**Question:** Multi-target (net8.0, net9.0, net10.0)?
**Impact:** CI complexity, package size.

### 3. Source generators
**Current:** None.
**Question:** Generate `ArgumentParser` registration from attributes?
**Impact:** Major API change, compile-time safety.

## Testing gaps

### 1. Missing unit test coverage
- Missing value for known argument
- Required argument not provided
- Default value application
- `ArgumentsCollection` methods (Has, Get, indexer, IEnumerable)
- Case sensitivity
- Duplicate registration behavior

### 2. Architecture test coverage
- No test for `Argument` immutability
- No test for `ArgumentsCollection` thread-safety (documented as not safe)
- No test for public API surface stability

## Documentation gaps

### 1. No API usage examples for error handling
### 2. No migration guide (no previous version)
### 3. No performance benchmarks

## Decision log

| Question | Decision | Date | Author |
|----------|----------|------|--------|
| Short flags? | No — minimal scope | — | — |
| Positional args? | No — minimal scope | — | — |
| Subcommands? | No — minimal scope | — | — |
| Boolean flags? | No — minimal scope | — | — |
| Help generation? | Not yet — separate feature | — | — |
| Case-insensitive? | No — case-sensitive by default | — | — |
| Custom exceptions? | No — standard .NET exceptions | — | — |