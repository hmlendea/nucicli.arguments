# Repository overview

## Purpose

NuciCLI.Arguments is a lightweight C# library for parsing command-line arguments using an argparse-inspired model. It provides a simple API for defining expected arguments, parsing `--name value` style input, and validating unknown/missing arguments.

## Scope

The library owns:
- Argument definition and registration (`Argument`, `ArgumentParser.AddArgument`)
- Token parsing and validation (`ArgumentParser.ParseArgs`)
- Parsed value storage and retrieval (`ArgumentsCollection`)

The library deliberately does not own:
- Flag/boolean arguments (`--flag` without value)
- Positional arguments
- Subcommands
- Help text generation
- Configuration file parsing
- Environment variable fallback

## Principal capabilities

1. **Programmatic argument registration** — Arguments registered via `AddArgument(name, help, required, defaultValue)` without reflection or attributes
2. **Strict parsing** — Unknown arguments and missing values throw immediately with clear exceptions
3. **Default value application** — Optional arguments receive defaults after token consumption
4. **Typed retrieval** — `Get<T>(key)` provides cast access to stored values
5. **Dictionary-like access** — Indexer, `Has()`, and `IEnumerable` iteration

## Entry points

- `ArgumentParser` — Main parsing facade, instantiated by consumers
- `ArgumentParser.ParseArgs(string[])` — Primary operation, returns `ArgumentsCollection`

## Runtime topology

Single-threaded, synchronous, no background tasks, no external I/O. The library is a pure in-memory parser with no network, filesystem, or persistence dependencies.

## Major integrations

- **NuciExtensions** — Provides `EnumerableExt.IsNullOrEmpty()` for queue emptiness checks
- **NUnit** — Test framework (test projects only)
- **ArchUnitNET** — Architecture fitness tests (ArchitectureTests project only)
- **Microsoft.CodeAnalysis** — Roslyn-based source convention tests (ArchitectureTests project only)

## System-wide invariants

1. Every registered argument name is unique (dictionary key)
2. Parsing either succeeds completely or throws before returning
3. Required arguments without values always throw `ArgumentNullException`
4. Unknown arguments always throw `ArgumentException`
5. Default values applied only to missing optional arguments
6. All public types reside in `NuciCLI.Arguments` namespace