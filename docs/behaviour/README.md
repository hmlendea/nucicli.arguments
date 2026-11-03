# Behaviour documentation

This directory documents runtime behaviour, invariants, and guarantees.

## Files

| File | Description |
|------|-------------|
| [../invariants.md](../invariants.md) | System-wide and component invariants |
| [../error-handling.md](../error-handling.md) | Exception taxonomy and handling patterns |
| [../concurrency-and-scheduling.md](../concurrency-and-scheduling.md) | Threading model (single-threaded) |
| [../state-and-persistence.md](../state-and-persistence.md) | Stateless design, no persistence |
| [../logging.md](../logging.md) | No logging framework |
| [../security.md](../security.md) | Threat model and security controls |

## Behaviour summary

- **Parsing:** Two-pass (token consumption → default application)
- **Errors:** Fail-fast with standard .NET exceptions
- **Threading:** Not thread-safe, single-threaded use
- **State:** No persistent state, stateless per parse
- **Logging:** None
- **Security:** No I/O, no external calls, minimal attack surface