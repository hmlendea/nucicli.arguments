# Troubleshooting

## Common issues

### "Invalid argument format: {token}"
**Cause:** Token doesn't start with `--`.
**Fix:** Ensure all arguments use `--name` format. Positional arguments not supported.

### "Unknown argument: {argName}"
**Cause:** Argument used but not registered with `AddArgument`.
**Fix:** Register argument before parsing, or fix typo in argument name.

### ArgumentNullException (ParamName = "argName")
**Cause 1:** Argument provided but no value follows it.
**Fix 1:** Provide value after argument: `--name value`.

**Cause 2:** Required argument not provided at all.
**Fix 2:** Include required argument in command line.

### KeyNotFoundException from Get<T>
**Cause:** Key not present in parsed results.
**Fix:** Check `Has("key")` first, or ensure argument registered and parsed.

### InvalidCastException from Get<T>
**Cause:** Stored value not convertible to requested type.
**Fix:** Use correct type. Values stored as strings from parsing; defaults keep original type.

## Build issues

### "Target framework net10.0 not found"
**Cause:** .NET 10.0 SDK not installed.
**Fix:** Install .NET 10.0 SDK from https://dotnet.microsoft.com.

### "Package NuciExtensions 6.0.2 not found"
**Cause:** NuGet source missing or package not published.
**Fix:** Ensure nuget.org source enabled. Package is public.

### Architecture tests fail
**Cause:** Code violates architectural rules (cycles, namespace, etc.).
**Fix:** Run tests locally, fix violations. Rules in `ProductionArchitectureTests.cs` and `SourceFileConventionTests.cs`.

### Source convention tests fail
**Cause:** File doesn't match conventions (one type per file, name match, explicit accessibility, braced control flow).
**Fix:** Fix source file to match conventions.

## Runtime issues

### Parsing succeeds but values wrong
**Cause:** Case mismatch (names are case-sensitive).
**Fix:** Use exact case in both registration and command line.

### Default value not applied
**Cause:** Argument registered as required (`IsRequired = true`).
**Fix:** Set `IsRequired = false` for optional arguments with defaults.

### Duplicate argument registration
**Cause:** Calling `AddArgument` twice with same name.
**Fix:** Use unique names. Second call overwrites first (last-write-wins).

## Debugging tips

1. **Inspect registered arguments:** `parser` internal dictionary not exposed. Add debug logging in consumer.
2. **Inspect parsed values:** `args` is `IEnumerable<KeyValuePair<string, object>>` — iterate to see all.
3. **Check token array:** Print `args` array before passing to `ParseArgs`.
4. **Enable .NET runtime logging:** Not applicable — library has no logging.

## Getting help

- GitHub Issues for bugs
- GitHub Discussions for questions
- Check `FAQ.md` for common questions