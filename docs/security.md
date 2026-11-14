# Security

This document complements the root `SECURITY.md` with implementation-specific details.

## Threat model

### Assets
- Parsed argument values (strings provided by consumer)
- Argument definitions (registered by consumer)

### Threats
| Threat | Likelihood | Impact | Mitigation |
|--------|------------|--------|------------|
| Argument injection | Low | Low | Strict parsing rejects unknown args |
| Value injection | Low | Low | Values stored as-is, no evaluation |
| Denial of service via large input | Low | Medium | No input size limits (consumer responsibility) |
| Information disclosure via exceptions | Medium | Low | Exception messages contain arg names only |

### Trust boundaries
- **Consumer ↔ Library:** Consumer controls all input (registration + tokens). Library validates format and registration.

There are no other trust boundaries; the library has no runtime dependencies.

## Security controls

### Input validation
- Token format: Must start with `--`
- Argument name: Must match registered name exactly
- Value presence: Required for every provided argument
- Required arguments: Enforced after parsing

### No dangerous operations
- No file I/O
- No network I/O
- No process execution
- No reflection
- No deserialization
- No code evaluation
- No dynamic compilation

### Exception safety
- Exception messages contain only argument names (no stack traces in message)
- No sensitive data in exceptions (library handles no secrets)
- Standard .NET exception types (`ArgumentException`, `ArgumentNullException`)

## Dependency security

### Production dependency

None. The library uses only the .NET 10.0 base class library.

### Transitive dependencies
None beyond .NET 10.0 base class library.

## Vulnerability reporting

Per root `SECURITY.md`:
- GitHub Security Advisories
- security@nucicli.com
- 24-hour acknowledgement target

## Secure usage guidelines

1. **Validate consumer input** — Consumers should validate argument values after parsing (type, range, format)
2. **Don't pass untrusted tokens directly** — If tokens come from untrusted source, validate before parsing
3. **Handle exceptions** — Catch `ArgumentException` and `ArgumentNullException` to avoid crashes
4. **No secrets in arguments** — Don't pass passwords/tokens as CLI arguments (visible in process list)

## Automated scanning

`.github/workflows/security.yml` runs on pushes and pull requests to `master` and weekly:
- `dotnet list package --vulnerable --include-transitive` reports vulnerable direct and transitive NuGet packages
- CodeQL (`github/codeql-action`) analyses the C# source and uploads SARIF results to GitHub Code Scanning

Results appear in the repository **Security** tab and the workflow's **Actions** page. CodeQL requires the repository to be public.

## Compliance

- No personal data processed (per `PRIVACY.md`)
- No telemetry
- No external communication
- GPL-3.0-or-later license (copyleft)