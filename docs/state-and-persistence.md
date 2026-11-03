# State and persistence

## State management

**Stateless by design.** The library maintains no persistent state between operations.

### ArgumentParser
- Holds argument registrations in `Dictionary<string, Argument>` (in-memory only)
- Registrations lost when instance disposed/GC'd
- No serialization, no persistence

### Argument
- Immutable data object
- No state beyond constructor parameters

### ArgumentsCollection
- Holds parsed values in `Dictionary<string, object>` (in-memory only)
- Values lost when instance disposed/GC'd
- No serialization, no persistence

## Persistence

**None.** The library provides no:
- File I/O
- Database access
- Serialization (JSON, XML, binary)
- Configuration file reading/writing
- Caching

## State transitions

```
ArgumentParser created
    ↓
AddArgument() × N  (builds registration dictionary)
    ↓
ParseArgs(tokens)  (creates new ArgumentsCollection)
    ↓
ArgumentsCollection returned
    ↓
Consumer uses values
    ↓
Both objects eligible for GC
```

No state survives beyond the parse operation.

## Recovery

No recovery needed — no persistent state to recover.

## Migration

No migration needed — no persisted schema.

## Backup/restore

Not applicable — no persistent data.

## Data retention

No data retained by library. Consumer controls lifetime of `ArgumentsCollection`.

## Concurrency and state

See `concurrency-and-scheduling.md` — no thread safety, no shared state.