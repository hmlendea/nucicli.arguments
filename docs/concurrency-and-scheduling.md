# Concurrency and scheduling

## Threading model

**Single-threaded, no concurrency.** The library has no async operations, no background threads, no thread pool usage, and no synchronization primitives.

## Async/await

Not used. All methods are synchronous:
- `ArgumentParser.AddArgument` — synchronous
- `ArgumentParser.ParseArgs` — synchronous
- `ArgumentsCollection` indexer, `Has`, `Get<T>` — synchronous

## Schedulers

None. No `TaskScheduler`, `SynchronizationContext`, or custom schedulers.

## Locks and synchronization

None. No `lock`, `Monitor`, `SemaphoreSlim`, `ReaderWriterLockSlim`, `Interlocked`, or concurrent collections.

## Thread safety

| Type | Thread-safe? | Notes |
|------|--------------|-------|
| `ArgumentParser` | No | Not designed for concurrent registration/parsing |
| `Argument` | Yes | Immutable after construction |
| `ArgumentsCollection` | No | Mutable dictionary, no synchronization |

**Usage pattern:** Create `ArgumentParser`, register arguments, call `ParseArgs` once. All on single thread. If concurrent parsing needed, create separate `ArgumentParser` instances per thread.

## Parallelism

No parallel processing. Token parsing is sequential (Queue consumption). No `Parallel.ForEach`, `PLINQ`, or `Task.WhenAll`.

## Cancellation

No `CancellationToken` support. Parsing cannot be cancelled mid-operation.

## Timers and delays

None. No `Timer`, `Task.Delay`, or time-based operations.

## Resource pooling

None. No object pools, connection pools, or buffer pools.

## Performance characteristics

- **Allocation:** One `ArgumentsCollection` + one `Dictionary<string, object>` per parse. One `Queue<string>` for tokens. Arguments dictionary allocated once at registration.
- **Time complexity:** O(n) where n = token count. Dictionary lookups O(1) per token.
- **No contention:** Single-threaded design eliminates lock contention entirely.

## Guidance for consumers

- Use one `ArgumentParser` per CLI invocation
- Don't share `ArgumentParser` or `ArgumentsCollection` across threads
- If parsing multiple command lines concurrently, create separate parsers
- No async/await needed — call `ParseArgs` directly