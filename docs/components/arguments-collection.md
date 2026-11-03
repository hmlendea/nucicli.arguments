# Component: ArgumentsCollection

## Overview

**File:** `NuciCLI.Arguments/ArgumentsCollection.cs`
**Namespace:** `NuciCLI.Arguments`
**Type:** `public class ArgumentsCollection : IEnumerable<KeyValuePair<string, object>>`

Dictionary-backed container for parsed argument values. Returned by `ArgumentParser.ParseArgs()`.

## Public API

### Indexer (get)

```csharp
public object this[string key]
{
    get => _values.TryGetValue(key, out var value) ? value : null;
}
```

Returns value as `object` or `null` if key not present. No exception.

### Indexer (set)

```csharp
public object this[string key]
{
    set => _values[key] = value;
}
```

Sets or replaces value. Allows post-parse mutation.

### Has

```csharp
public bool Has(string key) => _values.ContainsKey(key);
```

Existence check. Returns `true` if key exists (even with `null` value).

### Get<T>

```csharp
public T Get<T>(string key) => (T)_values[key]!;
```

Typed retrieval. Casts stored value to `T`.

**Throws:** `KeyNotFoundException` if key missing. `InvalidCastException` if stored value not compatible with `T`.

**Note:** Uses non-null-forgiving operator (`!`); caller must ensure key exists via `Has()` or try/catch.

### GetEnumerator

```csharp
public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
    => _values.GetEnumerator();

IEnumerator IEnumerable.GetEnumerator()
    => GetEnumerator();
```

Enables `foreach` and LINQ over parsed arguments.

## Internal implementation

### Storage

```csharp
private readonly Dictionary<string, object> _values = [];
```

Keys: argument names (without `--`). Values: raw strings from parsing, or default values (any type).

### Mutability

Collection is mutable after creation via indexer setter. This enables:
- Test scenarios (modify parsed values)
- Post-processing (convert strings to typed values in-place)
- Advanced consumers building on the library

## Behavioural details

### Value types

- **Parsed values:** Always `string` (from token consumption in `ArgumentParser`)
- **Default values:** Retain original type from `Argument.DefaultValue` (can be any `object`)
- **Mixed types possible:** Collection can contain `string`, `int`, `null`, etc. simultaneously

### Type safety

No built-in conversion. Consumer must:
1. Know expected type for each key
2. Use `Get<T>()` with correct `T`
3. Handle `KeyNotFoundException` and `InvalidCastException`

### Iteration order

Follows `Dictionary<string, object>` enumeration order (insertion order in .NET Core 3.0+). Order matches:
1. Arguments provided in command line (parsed order)
2. Arguments with defaults applied (registration order)

### Null handling

- `Has(key)` returns `true` for keys with `null` value
- Indexer get returns `null` for missing keys AND for keys with `null` value (ambiguous)
- `Get<T>(key)` throws `KeyNotFoundException` for missing keys; returns `default(T)` for `null` values (cast behavior)

## Dependencies

- `System.Collections.Generic.Dictionary<TKey, TValue>` — Storage
- `System.Collections.IEnumerable` — Iteration interface

## Thread safety

Not thread-safe. Dictionary operations not synchronized. Typical usage: single-threaded after parsing.

## Testing

Covered indirectly by `ArgumentParserTests`. No dedicated `ArgumentsCollectionTests` file exists.

**Gaps:** No tests for:
- Indexer get/set behaviour
- `Has()` with null values
- `Get<T>()` casting and exceptions
- Enumeration order
- Mixed value types