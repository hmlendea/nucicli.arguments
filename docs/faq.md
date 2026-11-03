# Frequently asked questions

## General

**Q: What does this library do?**
A: Parses command-line arguments in `--name value` format. Register arguments, call `ParseArgs`, get typed values from `ArgumentsCollection`.

**Q: What .NET version is required?**
A: .NET 10.0 (net10.0 target framework).

**Q: Is it a NuGet package?**
A: Yes, published to NuGet.org as `NuciCLI.Arguments`.

**Q: What license?**
A: GPL-3.0-or-later.

## Features

**Q: Does it support short flags like `-v`?**
A: No. Only `--long-name` format.

**Q: Does it support positional arguments?**
A: No. All arguments must be named with `--` prefix.

**Q: Does it support subcommands?**
A: No. Flat argument list only.

**Q: Does it generate help text?**
A: No. No built-in help generation.

**Q: Does it support boolean flags (--verbose without value)?**
A: No. Every argument requires a value. Use `--verbose true` / `--verbose false`.

**Q: Can I have optional arguments with defaults?**
A: Yes. Set `IsRequired = false` and provide `DefaultValue` in `Argument` constructor.

**Q: Are argument names case-sensitive?**
A: Yes. `--Name` and `--name` are different.

## Usage

**Q: How do I register an argument?**
```csharp
parser.AddArgument(new Argument("name", "Description", isRequired: true));
```

**Q: How do I parse?**
```csharp
ArgumentsCollection args = parser.ParseArgs(argsArray);
```

**Q: How do I get a typed value?**
```csharp
string value = args.Get<string>("name");
int count = args.Get<int>("count");
```

**Q: What if the argument wasn't provided?**
A: If required → `ArgumentNullException` during parse. If optional → `DefaultValue` applied. `Get<T>` throws `KeyNotFoundException` if key missing.

**Q: Can I check existence without exception?**
```csharp
if (args.Has("name")) { ... }
```

## Errors

**Q: What exceptions can be thrown?**
A: `ArgumentException` (invalid format, unknown arg), `ArgumentNullException` (missing value, missing required), `KeyNotFoundException` / `InvalidCastException` from `Get<T>`.

**Q: How do I handle missing required arguments?**
A: Catch `ArgumentNullException` — `ex.ParamName` contains the argument name.

## Testing

**Q: How do I test my argument parsing?**
A: Create `ArgumentParser`, register args, call `ParseArgs` with test tokens, assert on `ArgumentsCollection`.

**Q: Are there architecture tests?**
A: Yes — ArchUnitNET for compiled rules, Roslyn for source conventions.

## Contributing

**Q: Where do I report bugs?**
A: GitHub Issues.

**Q: Where do I request features?**
A: GitHub Issues (but scope is intentionally minimal).

**Q: Can I add short flags / positionals / subcommands?**
A: Not planned. Library intentionally minimal.