# Dependencies

## External dependencies

### Production (runtime)

None. The library depends only on the .NET 10.0 base class library.

### Development (test/build only)

| Package | Version | Project | Purpose |
|---------|---------|---------|---------|
| `Microsoft.NET.Test.Sdk` | 18.11.0 | UnitTests, ArchitectureTests | Test host |
| `NUnit` | 5.0.0 | UnitTests, ArchitectureTests | Test framework |
| `NUnit3TestAdapter` | 6.3.1 | UnitTests, ArchitectureTests | VS Code/test runner integration |
| `ArchUnitNET` | 2.4.0-alpha.1 | ArchitectureTests | Compiled architecture rules |
| `Microsoft.CodeAnalysis.CSharp` | 5.9.0 | ArchitectureTests | Roslyn source analysis |

**Total dev dependencies:** 5

## Internal dependencies

### Project references

```
NuciCLI.Arguments.UnitTests
    └── ProjectReference → NuciCLI.Arguments

NuciCLI.Arguments.ArchitectureTests
    └── ProjectReference → NuciCLI.Arguments
```

No other internal dependencies. Test projects do not reference each other.

## Dependency graph

```
NuciCLI.Arguments (production)
    │
    ├── NuciCLI.Arguments.UnitTests (test)
    │   ├── Microsoft.NET.Test.Sdk
    │   ├── NUnit
    │   └── NUnit3TestAdapter
    │
    └── NuciCLI.Arguments.ArchitectureTests (test)
        ├── Microsoft.NET.Test.Sdk
        ├── NUnit
        ├── NUnit3TestAdapter
        ├── ArchUnitNET
        └── Microsoft.CodeAnalysis.CSharp
```

## Dependency management

- All versions pinned in `.csproj` files
- Package references ordered alphabetically
- No central package management (Directory.Packages.props)
- No transitive dependency pinning beyond direct references
- No `PackageReference` version ranges

## Vulnerability scanning

No automated vulnerability scanning configured in CI. Dependencies updated manually.

## License compatibility

- Production: no third-party runtime dependencies
- Test: All test packages MIT/Apache-2.0 compatible
- Project license: GPL-3.0-or-later (copyleft)

GPL-3.0 requires source distribution for derivatives. Test dependencies not distributed with package.

## Framework dependencies

- **Target:** .NET 10.0 (`net10.0`)
- **Runtime:** .NET 10.0 or compatible
- **No native dependencies**
- **No platform-specific code**

## Update policy

- Major updates: Manual review required
- Minor/patch: Can update if tests pass
- Test dependencies: Updated with framework upgrades
- No automated dependabot/renovate configured