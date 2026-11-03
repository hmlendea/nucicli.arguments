# Error handling

## Error taxonomy

| Exception | Trigger | HTTP equivalent | Recoverable |
|-----------|---------|-----------------|-------------|
| `ArgumentException` | Invalid token format (missing `--`) | 400 Bad Request | No — input malformed |
| `ArgumentException` | Unknown argument name | 400 Bad Request | No — argument not registered |
| `ArgumentNullException` | Known argument missing value | 400 Bad Request | No — incomplete input |
| `ArgumentNullException` | Required argument not provided | 400 Bad Request | No — missing required input |
| `KeyNotFoundException` | `Get<T>(key)` for missing key | 500 Internal Error | Yes — consumer bug |
| `InvalidCastException` | `Get<T>(key)` with wrong type | 500 Internal Error | Yes — consumer bug |

## Handling patterns

### In ArgumentParser.ParseArgs

```csharp
try
{
    ArgumentsCollection args = parser.ParseArgs(tokens);
    // Success — use args
}
catch (ArgumentException ex) when (ex.Message.StartsWith("Invalid argument format"))
{
    // Token missing -- prefix
}
catch (ArgumentException ex) when (ex.Message.StartsWith("Unknown argument"))
{
    // Argument not registered
}
catch (ArgumentNullException ex)
{
    // Missing value or missing required argument
    // ex.ParamName contains argument name
}
```

### In consumer code using ArgumentsCollection

```csharp
// Safe existence check
if (args.Has("key"))
{
    string value = args.Get<string>("key");
}

// Or try/catch for Get<T>
try
{
    int value = args.Get<int>("key");
}
catch (KeyNotFoundException)
{
    // Key not present
}
catch (InvalidCastException)
{
    // Value not convertible to int
}
```

## Error message format

All exceptions include the argument name for debugging:

- `ArgumentException`: `"Invalid argument format: {token}"`
- `ArgumentException`: `"Unknown argument: {argName}"`
- `ArgumentNullException`: `ParamName = argName` (standard .NET pattern)

No structured error codes — exception type + message sufficient for CLI context.

## Recovery strategies

| Scenario | Recovery |
|----------|----------|
| Invalid format | Fix input tokens (add `--`) |
| Unknown argument | Register argument or fix typo |
| Missing value | Provide value after argument |
| Missing required | Provide required argument |
| Consumer cast error | Fix consumer type expectation |

No automatic recovery — all errors require input correction.

## Logging

No internal logging. Exceptions propagate to consumer. Consumer responsible for logging/displaying errors.

## Testing error paths

Current unit tests cover:
- Unknown argument → `ArgumentException`
- (Gaps: missing value, missing required, invalid format)

Architecture tests verify:
- No `InternalsVisibleTo` (prevents test-only error paths)
- Public API surface matches expectations