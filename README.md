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
  - [Examples](#examples)
- [System Requirements](#-system-requirements)
- [Installation](#-installation)
  - [Package Manager Installation](#package-manager-installation)
- [Configuration](#-configuration)
  - [Settings](#settings)
- [Compatibility](#-compatibility)
- [Privacy and Data](#-privacy-and-data)
- [Development](#-development)
  - [Requirements](#requirements)
  - [Setup](#setup)
  - [Build](#build)
  - [Test](#test)
  - [Release](#release)
  - [Dependencies](#dependencies)
- [GitHub Actions](#-github-actions)
- [Project Structure](#-project-structure)
  - [Projects and Packages](#projects-and-packages)
- [Architecture](#-architecture)
- [Documentation](#-documentation)
- [API Reference](#-api-reference)
- [FAQ](#-faq)
- [Troubleshooting](#-troubleshooting)
- [Contributing](#-contributing)
- [Code of Conduct](#-code-of-conduct)
- [Security](#-security)
- [Project Engagement](#-project-engagement)
- [License](#-license)

## ✨ Capabilities

- Define expected arguments in one place with `AddArgument(...)`
- Parse `--name value` style input into an accessible collection
- Validate unknown and missing arguments with clear exceptions
- Set required and default values without repetitive boilerplate
- Access parsed values by key or by typed helper (`Get<T>`)

## 🚀 Usage

```csharp
using NuciCLI.Arguments;

var parser = new ArgumentParser();
parser.AddArgument("name", help: "Your name", required: true);
parser.AddArgument("age", help: "Your age", defaultValue: "25");

ArgumentsCollection arguments = parser.ParseArgs(["--name", "Alice"]);
string name = arguments.Get<string>("name");
string age = arguments.Get<string>("age");
```

### Examples

See the [API usage examples](./docs/api-usage-examples.md) and [quick start](./docs/quick-start.md) for complete parsing workflows.

## 🖥️ System Requirements

The library targets .NET 10.0 and requires a compatible .NET runtime when used by an application.

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

## ⚙️ Configuration

The library is configured programmatically by registering `Argument` instances with `ArgumentParser`.

### Settings

The subsequent settings are recognised:
| Section | Key | Type | Default | Required | Description |
|---------|-----|------|---------|----------|-------------|
| Argument definition | `name` | `string` | — | Yes | Argument name without the `--` prefix |
| Argument definition | `description` | `string` | `—` | No | Help text for the argument |
| Argument definition | `isRequired` | `bool` | `false` | No | Whether the argument must be provided |
| Argument definition | `defaultValue` | `object` | `—` | No | Value used when the argument is omitted |

## 🧩 Compatibility

| Component | Supported Versions | Notes |
|-----------|--------------------|-------|
| .NET | 10.0 | The project targets `net10.0`. |

## 🛡️ Privacy and Data

NuciCLI.Arguments does not request, collect, transmit, or persist personal data. It parses command-line tokens provided by the consuming application.

For the detailed description of how the application handles privacy and personal data, see [PRIVACY.md](./PRIVACY.md).

## 🛠️ Development

### Requirements

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)

### Setup

Restore the repository dependencies:

```bash
dotnet restore
```

### Build

```bash
dotnet build --no-restore
```

### Test

```bash
dotnet test --no-build --verbosity normal
```

### Release

Published GitHub Releases trigger package creation. The workflows pack the project using the release tag version and publish the package to NuGet.org and the GitHub Release.

### Dependencies

| Package | Version | Scope | Purpose |
|---------|---------|-------|---------|
| NuciExtensions | 6.0.2 | Runtime | Provides extensions used by the parser. |
| NUnit | Project-defined | Development | Test framework used by the test projects. |

## ⚙️ GitHub Actions

| Workflow | Purpose | What it does |
|----------|---------|--------------|
| [ .NET ](https://github.com/hmlendea/nucicli.arguments/blob/master/.github/workflows/dotnet.yml) | Build and test validation | Restores dependencies, builds the solution, and runs tests for pushes and pull requests to `master`. |
| [ GitHub Release ](https://github.com/hmlendea/nucicli.arguments/blob/master/.github/workflows/github-release.yml) | GitHub Release packaging | Packs the NuGet package and uploads it with a SHA256 checksum to the published GitHub Release. |
| [ NuGet.org Release ](https://github.com/hmlendea/nucicli.arguments/blob/master/.github/workflows/nuget-release.yml) | NuGet.org publication | Packs and publishes the package to NuGet.org using OIDC authentication. |

## 🗂️ Project Structure

The repository contains the library, unit tests, architecture tests, and supporting documentation.

### Projects and Packages

| Project | Type | Purpose |
|---------|------|---------|
| `NuciCLI.Arguments/` | .NET library | Public command-line argument parsing API. |
| `NuciCLI.Arguments.UnitTests/` | NUnit test project | Unit tests for parser behaviour. |
| `NuciCLI.Arguments.ArchitectureTests/` | NUnit test project | Architecture and source convention tests. |

## 🏗️ Architecture

NuciCLI.Arguments is a lightweight C# library for parsing command-line arguments using an argparse-inspired model.

See the [architecture documentation](./ARCHITECTURE.md) for the system context, principal components, runtime flows, ownership boundaries, dependencies, constraints, and extension points.

## 📚 Documentation

The repository contains substantive documentation in the [`docs/`](./docs/) directory, including a [documentation index](./docs/INDEX.md), [quick start](./docs/quick-start.md), [testing guide](./docs/testing.md), and [build and deployment guide](./docs/build-and-deployment.md).

| Resource | Description |
|----------|-------------|
| [API usage examples](./docs/api-usage-examples.md) | Complete usage workflows. |
| [FAQ](./docs/faq.md) | Frequently asked questions. |
| [Troubleshooting](./docs/troubleshooting.md) | Diagnostic procedures and resolutions. |

## 🌐 API Reference

The [API reference](./docs/api-reference/) documents the public library API and its components.

## ❓ FAQ

See the [Frequently Asked Questions](./docs/faq.md) for responses to frequently raised inquiries.

## 🩺 Troubleshooting

See [Troubleshooting Guide](./docs/troubleshooting.md) for diagnostic procedures and resolutions to recurring problems.

## 🤝 Contributing

You are welcome to submit any suggestion, feedback, or modification to this project.

When doing so, please:
- Maintain cross-platform compatibility
- Preserve the existing public contract unless a breaking change is intentional
- Submit focused pull requests that conform to the existing code style
- Maintain your branch synchronised with `master`
- Revise the documentation when functionality changes
- Properly test all modifications, including edge cases and error conditions
- Include tests for additional or modified functionality
- Raise a new [issue](https://github.com/hmlendea/nucicli.arguments/issues) for problems or suggestions

See the [contributing guidelines](./CONTRIBUTING.md) for details on how to report issues, suggest enhancements, and submit changes.

## 📜 Code of Conduct

See the [Code of Conduct](./CODE_OF_CONDUCT.md) for the standards of conduct expected in all project spaces.

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
