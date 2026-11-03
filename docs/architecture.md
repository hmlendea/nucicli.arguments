# Architecture

This document complements the root `ARCHITECTURE.md` with deeper implementation detail.

## High-level decomposition

```
┌─────────────────────────────────────┐
│           ArgumentParser            │  ← Registration + Parsing facade
└──────────────┬──────────────────────┘
               │
               ▼
┌─────────────────────────────────────┐
│            Argument                 │  ← Immutable definition (name, help, required, default)
└──────────────┬──────────────────────┘
               │
               ▼
┌─────────────────────────────────────┐
│         ArgumentsCollection         │  ← Dictionary-backed result container
└─────────────────────────────────────┘
```

## Component responsibilities

### ArgumentParser

**Location:** `NuciCLI.Arguments/ArgumentParser.cs`

**Public API:**
- `AddArgument(string name, string help = "", bool required = false, object defaultValue = null)` — Registers an argument definition
- `ParseArgs(string[] args)` — Parses tokens, returns `ArgumentsCollection`

**Internal state:**
- `Dictionary<string, Argument> arguments` — Registered definitions, keyed by name (without `--`)

**Parsing algorithm:**
1. Create empty `ArgumentsCollection` and `Queue<string>` from input tokens
2. While queue not empty:
   - Dequeue token
   - Validate `--` prefix → throw `ArgumentException` if missing
   - Extract name (substring after `--`)
   - Look up in `arguments` dictionary → throw `ArgumentException` if unknown
   - Check queue not empty → throw `ArgumentNullException` if missing value
   - Dequeue value, store in collection
3. Post-process registered arguments:
   - Skip if already parsed
   - If required → throw `ArgumentNullException`
   - Otherwise store default value (may be null)
4. Return populated collection

**Error semantics:**
- `ArgumentException` — Invalid format (missing `--`) or unknown argument
- `ArgumentNullException` — Missing value for known argument, or missing required argument

### Argument

**Location:** `NuciCLI.Arguments/Argument.cs`

**Immutable properties:**
- `Name` — Argument name (without `--`)
- `Description` — Help text
- `IsRequired` — Whether argument must be provided
- `DefaultValue` — Fallback when omitted (can be null)

**Construction:** Primary constructor with all properties as init-only

### ArgumentsCollection

**Location:** `NuciCLI.Arguments/ArgumentsCollection.cs`

**Internal storage:** `Dictionary<string, object> _values`

**Public API:**
- `this[string key]` — Get/set as `object` (returns null if missing)
- `Has(string key)` — Existence check
- `Get<T>(string key)` — Typed retrieval (casts, throws `KeyNotFoundException` if missing)
- `GetEnumerator()` — `IEnumerable<KeyValuePair<string, object>>` iteration

**Behaviour notes:**
- Indexer setter allows mutation after parsing
- `Get<T>` uses non-null-forgiving cast (`!`); caller must ensure key exists
- No built-in type conversion; values stored as raw strings from parsing

## Dependency direction

```
ArgumentParser ──uses──► Argument (definition)
ArgumentParser ──produces──► ArgumentsCollection (result)
ArgumentsCollection ──stores──► object values (raw strings)
```

No circular dependencies. All dependencies flow downward.

## Layer boundaries

Single-layer library. No separation of domain/application/infrastructure. All types in `NuciCLI.Arguments` namespace.

## Extension points

None currently. The library is closed for extension:
- No virtual methods
- No interfaces for substitution
- No event hooks
- No plugin mechanism

Consumers extend by wrapping or post-processing `ArgumentsCollection`.

## Design decisions (expanded)

1. **No reflection/attributes** — Explicit registration avoids startup scanning, keeps API simple, enables dynamic argument sets
2. **String-only parsing** — Values remain strings; caller casts via `Get<T>()`. No built-in type conversion (int, bool, enum, etc.)
3. **Strict mode** — Fail-fast on unknown args and missing values. No "ignore unknown" or "collect unknown" modes
4. **Defaults applied post-parse** — Required check happens after all tokens consumed, so `--required value --optional value` order doesn't matter
5. **Single-pass parsing** — No support for flags (`--flag`), positional args, subcommands, or `--` terminator
6. **Mutable result** — `ArgumentsCollection` indexer allows post-parse modification for testing or advanced scenarios
7. **No help generation** — Library doesn't format usage text; consumers build their own from `Argument` metadata

## External dependencies

| Dependency | Purpose | Version |
|------------|---------|---------|
| `NuciExtensions` | `EnumerableExt.IsNullOrEmpty()` for queue check | 6.0.2 |

No other runtime dependencies. Test dependencies (NUnit, ArchUnitNET, Roslyn) are dev-only.