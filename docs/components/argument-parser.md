# Component: ArgumentParser

## Overview

**File:** `NuciCLI.Arguments/ArgumentParser.cs`
**Namespace:** `NuciCLI.Arguments`
**Type:** `public class ArgumentParser`

Primary parsing facade. Consumers instantiate, register arguments via `AddArgument`, then call `ParseArgs`.

## Public API

### AddArgument

```csharp
public void AddArgument(string name, string help = "", bool required = false, object defaultValue = null)
```

Registers an argument definition. Overwrites any existing registration with the same `name`.

**Parameters:**
- `name` — Argument name without `--` prefix. Used as lookup key.
- `help` — Description for documentation. Default empty string.
- `required` — Whether argument must be provided. Default `false`.
- `defaultValue` — Fallback when omitted. Default `null`.

**Side effects:** Inserts/updates `arguments[name] = new Argument(name, help, required, defaultValue)`.

### ParseArgs

```csharp
public ArgumentsCollection ParseArgs(string[] args)
```

Parses command-line tokens into an `ArgumentsCollection`.

**Parameters:**
- `args` — Raw command-line tokens (typically `Environment.GetCommandLineArgs()` minus executable).

**Returns:** Populated `ArgumentsCollection`.

**Throws:**
- `ArgumentException` — Token missing `--` prefix, or unknown argument name.
- `ArgumentNullException` — Known argument missing value, or required argument not provided.

## Internal implementation

### State

```csharp
private readonly Dictionary<string, Argument> arguments = [];
```

Registered definitions, keyed by name (ordinal case-sensitive).

### Parsing algorithm (detailed)

```csharp
public ArgumentsCollection ParseArgs(string[] args)
{
    ArgumentsCollection parsed = new();
    Queue<string> remaining = new(args);

    while (remaining.Count > 0)
    {
        var current = remaining.Dequeue();

        // 1. Validate format
        if (!current.StartsWith("--"))
        {
            throw new ArgumentException($"Invalid argument format: {current}");
        }

        // 2. Extract name
        var argName = current[2..];

        // 3. Look up registration
        if (!arguments.ContainsKey(argName))
        {
            throw new ArgumentException($"Unknown argument: {argName}");
        }

        // 4. Ensure value token exists
        if (EnumerableExt.IsNullOrEmpty(remaining))
        {
            throw new ArgumentNullException(argName);
        }

        // 5. Consume and store value
        parsed[argName] = remaining.Dequeue();
    }

    // 6. Apply defaults for missing arguments
    foreach (Argument argument in arguments.Values)
    {
        if (parsed.Has(argument.Name))
        {
            continue; // Already provided
        }

        if (argument.IsRequired)
        {
            throw new ArgumentNullException(argument.Name);
        }

        parsed[argument.Name] = argument.DefaultValue;
    }

    return parsed;
}
```

## Behavioural details

### Token consumption

- Input treated as flat token sequence: `--name value --other value`
- No support for `--name=value` syntax
- No support for flags (`--flag` without following value)
- No support for positional arguments
- No `--` terminator to stop parsing

### Error precedence

1. Format validation (missing `--`) — first token check
2. Unknown argument — registration lookup
3. Missing value — queue emptiness after known argument
4. Missing required — post-loop default application

### Default application order

Defaults applied after all tokens consumed, iterating `arguments.Values` (registration order). This means:
- Required check happens after all explicit values processed
- Order of `AddArgument` calls doesn't affect required validation
- Later `AddArgument` with same name overwrites earlier (dictionary semantics)

## Dependencies

- `NuciExtensions.EnumerableExt.IsNullOrEmpty()` — Checks `Queue<string>` emptiness
- `System.Collections.Generic.Queue<T>` — Token consumption
- `System.Collections.Generic.Dictionary<TKey, TValue>` — Registration storage

## Thread safety

Not thread-safe. `ArgumentParser` instance should not be shared across threads during registration or parsing. Typical usage: single-threaded CLI entry point.

## Testing

Covered by `NuciCLI.Arguments.UnitTests.ArgumentParserTests`:
- Known arguments parse correctly
- Unknown argument throws `ArgumentException`
- (Additional tests needed for: missing value, required missing, defaults, overwrite registration)