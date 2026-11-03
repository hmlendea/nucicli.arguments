# Data model

## Domain entities

### Argument

**File:** `NuciCLI.Arguments/Argument.cs`

**Purpose:** Immutable definition of a single expected command-line argument.

**Properties:**

| Property | Type | Nullable | Description |
|----------|------|----------|-------------|
| `Name` | `string` | No | Argument name without `--` prefix. Used as dictionary key. |
| `Description` | `string` | No | Help text for documentation. Empty string by default. |
| `IsRequired` | `bool` | No | Whether the argument must be provided. Default `false`. |
| `DefaultValue` | `object` | Yes | Fallback value when argument omitted. Default `null`. |

**Equality:** Reference equality only (class, no `IEquatable<T>`). Two `Argument` instances with same name are not equal.

**Construction:** Primary constructor `Argument(string name, string help, bool isRequired, object defaultValue = null)`. All properties init-only.

**Usage:** Created internally by `ArgumentParser.AddArgument()`. Not intended for direct consumer instantiation.

### ArgumentsCollection

**File:** `NuciCLI.Arguments/ArgumentsCollection.cs`

**Purpose:** Container for parsed argument values, keyed by argument name.

**Internal storage:** `Dictionary<string, object> _values` — Keys are argument names (without `--`), values are raw strings from parsing (or default values).

**Public members:**

| Member | Signature | Behaviour |
|--------|-----------|-----------|
| Indexer (get) | `object this[string key]` | Returns value or `null` if key missing |
| Indexer (set) | `object this[string key] { set }` | Sets/replaces value |
| `Has` | `bool Has(string key)` | Returns `_values.ContainsKey(key)` |
| `Get<T>` | `T Get<T>(string key)` | Returns `(T)_values[key]!` — throws `KeyNotFoundException` if missing |
| `GetEnumerator` | `IEnumerator<KeyValuePair<string, object>> GetEnumerator()` | Delegates to `_values.GetEnumerator()` |

**Mutability:** Collection is mutable after parsing via indexer setter. This enables test scenarios and post-processing.

**Type safety:** No built-in conversion. Values stored as `object` (typically `string`). Caller must know expected type and use `Get<T>()`.

**Iteration:** Implements `IEnumerable<KeyValuePair<string, object>>` for foreach and LINQ.

## Relationships

```
ArgumentParser
    │
    ├── registers ──► Argument (multiple, unique by Name)
    │
    └── produces ──► ArgumentsCollection
                          │
                          └── stores ──► object values (keyed by Argument.Name)
```

## Constraints and invariants

1. **Unique names** — `ArgumentParser.arguments` dictionary enforces unique `Argument.Name` keys. Duplicate `AddArgument` calls overwrite previous registration.
2. **Required vs default** — If `IsRequired=true`, `DefaultValue` is ignored (missing required throws before default applied).
3. **Default value type** — `DefaultValue` stored as `object`; no validation that it matches expected consumer type.
4. **Parsed value type** — All parsed values are `string` (from token consumption). Default values retain their original type.
5. **Case sensitivity** — Argument names are case-sensitive (dictionary uses ordinal comparison).
6. **Null values** — `DefaultValue` can be `null`. Parsed values are never `null` (missing value throws). Indexer returns `null` for missing keys.

## Serialisation

No built-in serialisation. `ArgumentsCollection` is not marked `[Serializable]` and contains no serialisation logic. Consumers serialise by iterating and converting to their preferred format.

## Persistence

No persistence. The library is stateless between parses. Each `ArgumentParser` instance maintains its registration dictionary; `ParseArgs` creates a new `ArgumentsCollection` each call.