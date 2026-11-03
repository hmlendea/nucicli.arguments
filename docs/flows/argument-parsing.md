# Flow: Argument parsing

## Overview

End-to-end flow from raw command-line tokens to populated `ArgumentsCollection`.

## Sequence

```
Consumer code
    │
    ▼
new ArgumentParser()
    │
    ▼
AddArgument("name", "User name", required: true)
AddArgument("age", "User age", defaultValue: "18")
    │
    ▼
ParseArgs(["--name", "John", "--age", "30"])
    │
    ├── Queue<string> remaining = ["--name", "John", "--age", "30"]
    │
    ├── Loop iteration 1:
    │   ├── Dequeue "--name"
    │   ├── Validate "--" prefix ✓
    │   ├── Extract "name"
    │   ├── Look up in arguments dict ✓
    │   ├── Check remaining not empty ✓
    │   ├── Dequeue "John"
    │   └── parsed["name"] = "John"
    │
    ├── Loop iteration 2:
    │   ├── Dequeue "--age"
    │   ├── Validate "--" prefix ✓
    │   ├── Extract "age"
    │   ├── Look up in arguments dict ✓
    │   ├── Check remaining not empty ✓
    │   ├── Dequeue "30"
    │   └── parsed["age"] = "30"
    │
    ├── Loop ends (queue empty)
    │
    ├── Default application:
    │   ├── Argument "name": parsed.Has("name") → true → skip
    │   └── Argument "age": parsed.Has("age") → true → skip
    │
    ▼
Return ArgumentsCollection { "name" = "John", "age" = "30" }
    │
    ▼
Consumer: args.Get<string>("name") → "John"
Consumer: args.Get<string>("age") → "30"
```

## Error flows

### Unknown argument

```
ParseArgs(["--unknown", "value"])
    │
    ├── Dequeue "--unknown"
    │   ├── Validate "--" prefix ✓
    │   ├── Extract "unknown"
    │   └── Look up in arguments dict ✗
    │
    ▼
throw ArgumentException("Unknown argument: unknown")
```

### Missing value

```
ParseArgs(["--name"])
    │
    ├── Dequeue "--name"
    │   ├── Validate "--" prefix ✓
    │   ├── Extract "name"
    │   ├── Look up in arguments dict ✓
    │   └── Check remaining not empty ✗ (queue empty)
    │
    ▼
throw ArgumentNullException("name")
```

### Missing required argument

```
AddArgument("required", required: true)
ParseArgs([])
    │
    ├── Loop: queue empty immediately
    │
    ├── Default application:
    │   ├── Argument "required": parsed.Has("required") → false
    │   │   ├── IsRequired → true
    │   │   └── throw ArgumentNullException("required")
    │
    ▼
Exception before return
```

### Invalid format (missing --)

```
ParseArgs(["name", "John"])
    │
    ├── Dequeue "name"
    │   └── Validate "--" prefix ✗
    │
    ▼
throw ArgumentException("Invalid argument format: name")
```

## Default value flow

```
AddArgument("optional", defaultValue: "default")
ParseArgs([])
    │
    ├── Loop: queue empty immediately
    │
    ├── Default application:
    │   ├── Argument "optional": parsed.Has("optional") → false
    │   │   ├── IsRequired → false
    │   │   └── parsed["optional"] = "default"
    │
    ▼
Return ArgumentsCollection { "optional" = "default" }
```

## Consumer usage flow

```
ArgumentsCollection args = parser.ParseArgs(tokens);

if (args.Has("key"))           // Existence check
{
    string value = args["key"];     // Indexer get (object)
    int number = args.Get<int>("key"); // Typed get (cast)
}

foreach (var kvp in args)      // Iteration
{
    Console.WriteLine($"{kvp.Key} = {kvp.Value}");
}
```

## State transitions

```
ArgumentParser (empty registrations)
    │ AddArgument × N
    ▼
ArgumentParser (N registrations)
    │ ParseArgs(tokens)
    ▼
ArgumentsCollection (populated)
    │ Consumer access
    ▼
ArgumentsCollection (potentially mutated via indexer set)
```

No reuse of `ArgumentsCollection` across parses; each `ParseArgs` creates new instance.