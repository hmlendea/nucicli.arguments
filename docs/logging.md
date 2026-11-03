# Logging

## Logging framework

**None.** The library has no logging framework, no logging calls, and no logging configuration.

## Log levels

Not applicable — no logging.

## Structured fields

Not applicable — no logging.

## Correlation

Not applicable — no logging.

## Sinks/destinations

Not applicable — no logging.

## Redaction/masking

Not applicable — no logging.

## Retention

Not applicable — no logging.

## Sampling

Not applicable — no logging.

## Performance considerations

No logging overhead. Zero allocation for logging in hot paths.

## Error logging

Exceptions thrown by the library (`ArgumentException`, `ArgumentNullException`) contain argument names in messages. Consumers should log these exceptions if desired.

## Audit logging

No audit logging. The library performs no security-relevant operations.

## Configuration

No logging configuration. Consumers configure their own logging.

## Rationale

The library is a pure in-memory parser with no I/O, no external dependencies, and no long-running operations. Logging would add unnecessary overhead and complexity for a component that either succeeds quickly or throws immediately.