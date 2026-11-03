# Component: Argument

## Overview

**File:** `NuciCLI.Arguments/Argument.cs`
**Namespace:** `NuciCLI.Arguments`
**Type:** `public class Argument`

Immutable argument definition. Created by `ArgumentParser.AddArgument()`, stored in parser's registration dictionary.

## Public API

### Constructor

```csharp
public Argument(string name, string help, bool isRequired, object defaultValue = null)
```

Primary constructor with all properties as init-only.

### Properties

| Property | Type | Description |
|----------|------|-------------|
| `Name` | `string` | Argument name without `--` prefix. Dictionary key. |
| `Description` | `string` | Help text. Empty string by default. |
| `IsRequired` | `bool` | Whether argument must be provided. Default `false`. |
| `DefaultValue` | `object` | Fallback when omitted. Default `null`. |

All properties have public getters, no setters (immutable).

## Behavioural details

### Equality

Reference equality only (class, no `IEquatable<Argument>` override). Two `Argument` instances with identical property values are not equal.

### Usage in parser

- Stored in `ArgumentParser.arguments` dictionary keyed by `Name`
- Iterated during `ParseArgs` default application phase
- `DefaultValue` stored as-is (no type validation)
- `IsRequired` checked after token consumption loop

### Mutability

Fully immutable after construction. No methods modify state.

## Design rationale

- **Class not record** — Allows primary constructor with init-only properties while maintaining reference semantics for dictionary storage
- **object DefaultValue** — Maximum flexibility; consumer responsible for type correctness
- **No validation** — Parser doesn't validate that `DefaultValue` type matches expected consumer type
- **No help formatting** — Raw description string; formatting left to consumer

## Testing

No dedicated unit tests for `Argument` alone. Tested indirectly through `ArgumentParserTests`.