[![Donate](https://img.shields.io/badge/-%E2%99%A5%20Donate-%23ff69b4)](https://hmlendea.go.ro/funding)
[![Latest Release](https://img.shields.io/github/v/release/hmlendea/nucicli.arguments)](https://github.com/hmlendea/nucicli.arguments/releases/latest)
[![Build Status](https://github.com/hmlendea/nucicli.arguments/actions/workflows/dotnet.yml/badge.svg)](https://github.com/hmlendea/nucicli.arguments/actions/workflows/dotnet.yml)
[![NuGet](https://img.shields.io/nuget/v/NuciCLI.Arguments)](https://nuget.org/packages/NuciCLI.Arguments)
[![License](https://img.shields.io/github/license/hmlendea/nucicli.arguments)](https://github.com/hmlendea/nucicli.arguments/blob/master/LICENSE)

# NuciCLI.Arguments

A lightweight C# library for parsing command-line arguments using a familiar, argparse-inspired model.

## 📑 Table of Contents

- [Capabilities](#-capabilities)
- [Usage](#-usage)
  - [API Overview](#api-overview)
    - [`ArgumentParser`](#argumentparser)
    - [`ArgumentsCollection`](#argumentscollection)
    - [`Argument`](#argument)
  - [Behaviour Notes](#behaviour-notes)
- [Installation](#-installation)
  - [Package Manager Installation](#package-manager-installation)
- [Architecture](#-architecture)
- [Security](#-security)
- [Project Engagement](#-project-engagement)
- [License](#-license)

## ✨ Capabilities

- Define expected arguments in one place with `AddArgument(...)`
- Parse `--name value` style input into a strongly accessible collection
- Validate unknown and missing arguments early with clear exceptions
- Set required and default values without repetitive boilerplate
- Access parsed values by key or by typed helper (`Get<T>`)

## 🚀 Usage

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

### API Overview

#### `ArgumentParser`

- `AddArgument(string name, string help = "", bool required = false, object defaultValue = null)`
  Registers an accepted CLI argument.
- `ParseArgs(string[] args)`
  Parses command-line tokens and returns an `ArgumentsCollection`.

#### `ArgumentsCollection`

- `args["key"]`
  Gets or sets a parsed value by key (`object`). Returns `null` when the key does not exist.
- `Has(string key)`
  Checks whether a key is present.
- `Get<T>(string key)`
  Returns the stored value cast to the target type.
- `IEnumerable<KeyValuePair<string, object>>`
  Supports iteration over all parsed entries.

#### `Argument`

Represents registered argument metadata:

- `Name`
- `Description`
- `IsRequired`
- `DefaultValue`

### Behaviour Notes

- Argument names are defined without leading dashes: register `name`, parse as `--name John`.
- Tokens must follow the `--key value` pattern.
- If an unknown argument is provided, parsing throws `ArgumentException`.
- If a value is missing for a provided argument, parsing throws `ArgumentNullException`.

## 📦 Installation

[![Obtain it from NuGet](https://raw.githubusercontent.com/hmlendea/readme-assets/master/badges/stores/nuget.png)](https://nuget.org/packages/NuciCLI.Arguments)

### Package Manager Installation

```bash
dotnet add package NuciCLI.Arguments
```

Or, via the `Package Manager Console`:
```powershell
Install-Package NuciCLI.Arguments
```

## 🏗️ Architecture

NuciCLI.Arguments is a lightweight C# library for parsing command-line arguments using a familiar, argparse-inspired model.

See the [architecture documentation](./ARCHITECTURE.md) for the system context, principal components, runtime flows, ownership boundaries, dependencies, constraints, and extension points.

## 🔒 Security

NuciCLI.Arguments does not handle sensitive data, authentication, or network operations. It parses command-line arguments provided by the consuming application.

For information on reporting security vulnerabilities, see [SECURITY.md](./SECURITY.md).

## 💝 Project Engagement

Discovered a problem or have a suggestion? [Open an issue](https://github.com/hmlendea/nucicli.arguments/issues)!

If you find this project useful, consider [funding it](https://hmlendea.go.ro/funding) or starring ⭐️ it on GitHub!

[![Donate](https://raw.githubusercontent.com/hmlendea/readme-assets/master/donate_generic.png)](https://hmlendea.go.ro/funding)

## 📄 License

This project is being distributed under the `GNU General Public License v3.0` or later.
See [LICENSE](./LICENSE) for additional information.
