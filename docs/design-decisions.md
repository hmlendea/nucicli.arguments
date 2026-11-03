# Design decisions

## 1. No reflection or attributes for argument registration

**Decision:** Arguments registered programmatically via `AddArgument()` rather than via attributes on properties or reflection scanning.

**Rationale:**
- Explicit registration avoids startup assembly scanning overhead
- Keeps API simple and discoverable
- Enables dynamic argument sets (e.g., based on runtime configuration)
- No magic — consumer sees exactly what arguments exist
- Avoids issues with trimming/AOT compilation

**Trade-offs:**
- More verbose for consumers with many arguments
- No compile-time validation of argument names vs property names

## 2. String-only parsing with caller casting

**Decision:** All parsed values stored as `string` (or default value object). Caller uses `Get<T>()` to cast.

**Rationale:**
- Simplifies parser — no type conversion logic needed
- Maximum flexibility for consumers
- Avoids complex type conversion edge cases (culture, formats, etc.)
- Consistent with argparse model (strings in, caller converts)

**Trade-offs:**
- No built-in validation of value formats
- Caller must know expected types
- Runtime cast exceptions possible (`InvalidCastException`)

## 3. Strict fail-fast parsing

**Decision:** Unknown arguments and missing values throw immediately. No "ignore unknown" or "collect unknown" modes.

**Rationale:**
- Catches typos early (e.g., `--nmae` vs `--name`)
- Prevents silent misconfiguration
- Clear error messages with argument name
- Simpler implementation

**Trade-offs:**
- Less flexible for forward-compatible CLIs
- Can't parse partial arguments for subcommands
- No "pass-through" to nested parsers

## 4. Defaults applied after token consumption

**Decision:** Required check and default application happen in a second pass after all tokens processed.

**Rationale:**
- Order of arguments on command line doesn't matter
- `--required value --optional value` and `--optional value --required value` both work
- Required validation sees complete picture

**Trade-offs:**
- Slightly more complex than single-pass
- Default values for required args ignored (by design)

## 5. Single-pass, `--key value` only syntax

**Decision:** Only `--name value` format supported. No `--flag`, `--name=value`, positional args, subcommands, or `--` terminator.

**Rationale:**
- Minimal viable parser for common use case
- Simpler implementation and testing
- argparse-inspired but deliberately subset

**Trade-offs:**
- Not a full-featured CLI parser
- Consumers needing flags/positionals must wrap or extend
- Limited compared to System.CommandLine, CommandLineParser, etc.

## 6. Mutable ArgumentsCollection

**Decision:** `ArgumentsCollection` indexer has public setter, allowing post-parse mutation.

**Rationale:**
- Enables test scenarios (modify parsed values)
- Allows consumers to normalize/convert values in-place
- Simple dictionary-like semantics

**Trade-offs:**
- Breaks immutability expectation
- Thread-safety concerns if shared
- Could mask parsing errors if consumer mutates before validation

## 7. No help text generation

**Decision:** Library provides `Argument.Description` but no usage formatting or help printing.

**Rationale:**
- Separation of concerns — parsing vs presentation
- Consumers have diverse help formatting needs
- Keeps library small and focused

**Trade-offs:**
- Consumers must implement help themselves
- Duplication of description strings if consumer also shows help

## 8. Single namespace, no sub-namespaces

**Decision:** All public types in `NuciCLI.Arguments` namespace.

**Rationale:**
- Small library — no need for namespace hierarchy
- Simple imports for consumers
- Consistent with project structure (flat file layout)

**Trade-offs:**
- No logical grouping if library grows
- Potential name collisions in large solutions

## 9. Class not record for Argument

**Decision:** `Argument` is a `class` with primary constructor and init-only properties, not a `record`.

**Rationale:**
- Reference semantics for dictionary storage (key identity)
- Primary constructor with init-only provides immutability
- Records have value equality which would break dictionary key semantics

**Trade-offs:**
- More boilerplate than record
- No `with` expressions for modification

## 10. No interfaces for core types

**Decision:** `ArgumentParser`, `Argument`, `ArgumentsCollection` are concrete classes with no public interfaces.

**Rationale:**
- Small library with stable API
- No need for mocking in consumer tests (consumers test their usage, not the library)
- Simpler implementation

**Trade-offs:**
- Consumers can't easily substitute parser for testing
- No abstraction for alternative implementations