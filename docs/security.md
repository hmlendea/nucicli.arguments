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
- **Library ↔ NuciExtensions:** Single call to `EnumerableExt.IsNullOrEmpty()`. No data flow.

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
- `NuciExtensions` 6.0.2 — Internal package, source available at https://github.com/hmlendea/nuciextensions
- Single method used: `EnumerableExt.IsNullOrEmpty(Queue<string>)`

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

## Compliance

- No personal data processed (per `PRIVACY.md`)
- No telemetry
- No external communication
- GPL-3.0-or-later license (copyleft)