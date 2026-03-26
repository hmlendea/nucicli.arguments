[![Donate](https://img.shields.io/badge/-%E2%99%A5%20Donate-%23ff69b4)](https://hmlendea.go.ro/fund.html) [![Build Status](https://github.com/hmlendea/nucicli.arguments/actions/workflows/dotnet.yml/badge.svg)](https://github.com/hmlendea/nucicli.arguments/actions/workflows/dotnet.yml) [![Latest GitHub release](https://img.shields.io/github/v/release/hmlendea/nucicli.arguments)](https://github.com/hmlendea/nucicli.arguments/releases/latest)

# NuciCLI.Arguments

## About

A lightweight C# library for parsing command-line arguments using a familiar, argparse-inspired model.

`NuciCLI.Arguments` helps you:

- Define expected arguments in one place
- Parse `--name value` style input into a strongly accessible collection
- Validate unknown and missing arguments early
- Set required/default values without repetitive boilerplate

## Features

- Simple API with `AddArgument(...)` and `ParseArgs(...)`
- `--argument value` parsing flow
- Required argument enforcement
- Default value support for optional arguments
- Unknown argument detection with clear exceptions
- Parsed value access by key or by typed helper (`Get<T>`)

## Installation

[![Get it from NuGet](https://raw.githubusercontent.com/hmlendea/readme-assets/master/badges/stores/nuget.png)](https://nuget.org/packages/NuciCLI.Arguments)

### .NET CLI
```bash
dotnet add package NuciCLI.Arguments
```

### Package Manager
```powershell
Install-Package NuciCLI.Arguments
```

## Quick Start

```csharp
using NuciCLI.Arguments;

ArgumentParser parser = new();
parser.AddArgument("name", help: "Name of the user", required: true);
parser.AddArgument("age", help: "Age of the user", defaultValue: "18");

ArgumentsCollection args = parser.ParseArgs(["--name", "John"]);

string name = args.Get<string>("name");
string age = args.Get<string>("age"); // Uses default value "18"
```

Input:

```text
--name John --age 30
```

Parsed keys:

- `name` => `"John"`
- `age` => `"30"`

## API Overview

### `ArgumentParser`

- `AddArgument(string name, string help = "", bool required = false, object defaultValue = null)`
	Registers an accepted CLI argument.
- `ParseArgs(string[] args)`
	Parses command-line tokens and returns an `ArgumentsCollection`.

### `ArgumentsCollection`

- `args["key"]`
	Gets or sets a parsed value by key (`object`). Returns `null` when the key does not exist.
- `Has(string key)`
	Checks whether a key is present.
- `Get<T>(string key)`
	Returns the stored value cast to the target type.
- `IEnumerable<KeyValuePair<string, object>>`
	Supports iteration over all parsed entries.

### `Argument`

Represents registered argument metadata:

- `Name`
- `Description`
- `IsRequired`
- `DefaultValue`

## Behaviour Notes

- Argument names are defined without leading dashes:
	register `name`, parse as `--name John`.
- Tokens must follow the `--key value` pattern.
- If an unknown argument is provided, parsing throws `ArgumentException`.
- If a value is missing for a provided argument, parsing throws `ArgumentNullException`.
- If a required argument is not provided, parsing throws `ArgumentNullException`.
- Default values are applied only when an optional argument is missing.

Current parser scope:

- Does not parse short aliases like `-n`
- Does not parse `--key=value` syntax
- Does not infer types automatically
- Does not support positional arguments

## Related Packages

The NuciCLI ecosystem is split into focused packages:

- [NuciCLI](https://github.com/hmlendea/nucicli) for core console helpers
- [NuciCLI.Arguments](https://github.com/hmlendea/nucicli.arguments) for command-line argument handling
- [NuciCLI.Menus](https://github.com/hmlendea/nucicli.menus) for interactive terminal menus

## Target Framework

The current package targets `.NET 10`.

## Testing

Unit tests are built with NUnit and verify core parser behavior, including:

- successful parsing of known arguments
- exception flow for unknown arguments

## License

This project is licensed under the `GNU General Public License v3.0` or later. See [LICENSE](./LICENSE) for details.
