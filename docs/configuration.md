# Configuration

## Project configuration

All projects configured via `.csproj` files with consistent settings:

### Common properties (all projects)

```xml
<TargetFramework>net10.0</TargetFramework>
<ImplicitUsings>disable</ImplicitUsings>
<LangVersion>latest</LangVersion>
<Nullable>enable</Nullable>
```

### Production project (NuciCLI.Arguments)

```xml
<IsPackable>true</IsPackable>
<RootNamespace>NuciCLI.Arguments</RootNamespace>
<PackageId>NuciCLI.Arguments</PackageId>
<Version>1.0.2</Version>
<Description>CLI arguments parser inspired by Python's argparse.</Description>
<Authors>Horațiu Mlendea</Authors>
<Copyright>Copyright 2026 © Horațiu Mlendea</Copyright>
<RepositoryUrl>https://github.com/hmlendea/nucicli.arguments</RepositoryUrl>
<PackageLicenseExpression>GPL-3.0-or-later</PackageLicenseExpression>
<PackageTags>Console Terminal CommandLine CLI Parser ArgParse</PackageTags>
```

### Test projects (UnitTests, ArchitectureTests)

```xml
<IsPackable>false</IsPackable>
<IsTestProject>true</IsTestProject>
<ExcludeFromCodeCoverage>true</ExcludeFromCodeCoverage>
```

## Runtime configuration

The library has **no runtime configuration**. No:
- Configuration files (appsettings.json, etc.)
- Environment variables
- Command-line flags for the library itself
- Dependency injection setup
- Options pattern

Consumers configure the parser programmatically via `AddArgument` calls.

## Build configuration

### Solution file

`NuciCLI.Arguments.slnx` (SDK-style):
```xml
<Solution>
  <Project Path="NuciCLI.Arguments/NuciCLI.Arguments.csproj" />
  <Project Path="NuciCLI.Arguments.ArchitectureTests/NuciCLI.Arguments.ArchitectureTests.csproj" />
  <Project Path="NuciCLI.Arguments.UnitTests/NuciCLI.Arguments.UnitTests.csproj" />
</Solution>
```

### CI/CD pipelines

Defined in `.github/workflows/`:

| Workflow | Trigger | Purpose |
|----------|---------|---------|
| `dotnet.yml` | Push/PR to master | Build, restore, test |
| `nuget-release.yml` | GitHub Release published | Pack and publish to NuGet.org |
| `github-release.yml` | GitHub Release published | Pack and upload to GitHub Release |

All workflows use `dotnet-version: 10.0.x` on `ubuntu-latest`.

## Dependency management

### Production dependencies

| Package | Version | Purpose |
|---------|---------|---------|
| `NuciExtensions` | 6.0.2 | `EnumerableExt.IsNullOrEmpty()` |

### Test dependencies

| Package | Version | Project |
|---------|---------|---------|
| `Microsoft.NET.Test.Sdk` | 18.11.0 | UnitTests, ArchitectureTests |
| `NUnit` | 5.0.0 | UnitTests, ArchitectureTests |
| `NUnit3TestAdapter` | 6.3.1 | UnitTests, ArchitectureTests |
| `ArchUnitNET` | 2.4.0-alpha.1 | ArchitectureTests |
| `Microsoft.CodeAnalysis.CSharp` | 5.9.0 | ArchitectureTests |

Package references ordered alphabetically in each `.csproj`.

## Versioning

- **Library version:** Set in `NuciCLI.Arguments.csproj` `<Version>` element
- **Release process:** GitHub Release → triggers both NuGet and GitHub Release workflows
- **Version extraction:** Workflows strip `v` prefix from tag name (e.g., `v1.0.3` → `1.0.3`)
- **Scheme:** Semantic versioning (major.minor.patch)

## Precedence

No configuration precedence hierarchy exists — library has no configuration sources.