# Integrations

## External system integrations

**None.** The library has no external system integrations.

## NuGet dependencies

### Production
| Package | Version | Purpose |
|---------|---------|---------|
| `NuciExtensions` | 6.0.2 | `EnumerableExt.IsNullOrEmpty()` |

### Development
| Package | Version | Purpose |
|---------|---------|---------|
| `NUnit` | 4.3.2 | Unit testing |
| `NUnit3TestAdapter` | 5.0.0 | Test runner integration |
| `Microsoft.NET.Test.Sdk` | 17.13.0 | Test SDK |
| `ArchUnitNET` | 2.4.0-alpha.1 | Architecture testing (compiled) |
| `Microsoft.CodeAnalysis.CSharp` | 5.9.0 | Roslyn source analysis |

## Framework integration

### .NET
- Target: `net10.0`
- Requires: .NET 10.0 runtime
- Nullable: enabled
- Implicit usings: enabled

### Build
- SDK-style `.csproj`
- Solution: `.slnx` format
- `dotnet build` / `dotnet test` / `dotnet pack`

### CI/CD
- GitHub Actions (3 workflows)
- NuGet.org OIDC publishing
- GitHub Releases asset upload

## IDE/editor

- VS Code tasks configured (build, publish, watch)
- C# Dev Kit compatible
- Roslyn analyzers via `Microsoft.CodeAnalysis.CSharp`

## No integrations with

- Cloud providers (AWS, Azure, GCP)
- Databases (SQL, NoSQL)
- Message queues (RabbitMQ, Kafka, etc.)
- Service mesh (Istio, Linkerd)
- Monitoring (Prometheus, Datadog, etc.)
- Logging frameworks (Serilog, NLog, etc.)
- Configuration systems (Consul, etcd, etc.)
- Authentication/authorization (OAuth, OIDC, etc.)
- API gateways
- Container orchestration (Kubernetes, etc.)