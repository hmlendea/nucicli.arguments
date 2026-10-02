# Architecture

## Overview

NuciCLI.Arguments is a lightweight C# library for parsing command-line arguments using an argparse-inspired model. It provides a simple API for defining expected arguments, parsing `--name value` style input, and validating unknown/missing arguments.

## Project Structure

```
NuciCLI.Arguments.sln
├── NuciCLI.Arguments/           # Main library
│   ├── Argument.cs              # Argument metadata model
│   ├── ArgumentParser.cs        # Parser implementation
│   ├── ArgumentsCollection.cs   # Parsed arguments container
│   └── NuciCLI.Arguments.csproj
└── NuciCLI.Arguments.UnitTests/ # Unit tests
    ├── ArgumentParserTests.cs
    └── NuciCLI.Arguments.UnitTests.csproj
```

## Core Components

### Argument
**File:** `NuciCLI.Arguments/Argument.cs`

Immutable record-like class representing a registered CLI argument definition.

| Property | Type | Description |
|----------|------|-------------|
| `Name` | `string` | Argument name (without `--` prefix) |
| `Description` | `string` | Help text |
| `IsRequired` | `bool` | Whether argument must be provided |
| `DefaultValue` | `object` | Fallback value when omitted |

### ArgumentParser
**File:** `NuciCLI.Arguments/ArgumentParser.cs`

Parses raw `string[]` tokens into an `ArgumentsCollection`.

**Algorithm:**
1. Register arguments via `AddArgument()` into internal `Dictionary<string, Argument>`
2. Iterate tokens with a queue:
   - Validate `--` prefix
   - Look up registered argument
   - Consume next token as value
3. After token loop, apply defaults for missing optional arguments
4. Throw on missing required arguments

**Exceptions:**
- `ArgumentException` — unknown argument or invalid format
- `ArgumentNullException` — missing value for provided argument, or missing required argument

### ArgumentsCollection
**File:** `NuciCLI.Arguments/ArgumentsCollection.cs`

Dictionary-backed container for parsed values implementing `IEnumerable<KeyValuePair<string, object>>`.

| Member | Description |
|--------|-------------|
| `this[string key]` | Get/set value as `object` (returns `null` if missing) |
| `Has(string key)` | Existence check |
| `Get<T>(string key)` | Typed retrieval (casts, throws if missing) |
| `GetEnumerator()` | Iteration over all parsed entries |

## Data Flow

```
CLI tokens (string[])
       │
       ▼
ArgumentParser.ParseArgs()
       │
       ├── Validates format (--key value)
       ├── Looks up registered Argument
       ├── Consumes value token
       └── Populates ArgumentsCollection
       │
       ▼
ArgumentsCollection
       │
       ├── Indexer / Has() / Get<T>()
       └── IEnumerable iteration
```

## Dependencies

- **NuciExtensions** — Provides `EnumerableExt.IsNullOrEmpty()` used in `ArgumentParser`
- **NUnit** — Test framework (test project only)
- **Target Framework:** .NET 10.0

## Design Decisions

1. **No reflection/attributes** — Arguments registered programmatically via `AddArgument()`
2. **String-only parsing** — Values stored as `object` (originally `string`); caller casts via `Get<T>()`
3. **Strict mode** — Unknown arguments and missing values throw immediately
4. **Defaults applied post-parse** — Required check happens after token consumption
5. **Single-pass parsing** — No support for flags (`--flag`), positional args, or subcommands

## Extension Points

- Add new argument types (flags, arrays, subcommands) by extending `Argument` and `ArgumentParser`
- Custom validation via callbacks in `Argument` constructor
- Alternative parsers (e.g., POSIX short options) via new `IArgumentParser` interface