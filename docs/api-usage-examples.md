# API usage examples

This library is a .NET class library, not an HTTP API. Examples show C# usage patterns.

## Basic parsing

```csharp
using NuciCLI.Arguments;

var parser = new ArgumentParser();
parser.AddArgument("name", "User name", required: true);
parser.AddArgument("count", "Repeat count", required: false, defaultValue: "1");

ArgumentsCollection args = parser.ParseArgs(new[] { "--name", "Alice", "--count", "3" });

string name = args.Get<string>("name");  // "Alice"
int count = args.Get<int>("count");      // 3
```

## Required argument missing

```csharp
var parser = new ArgumentParser();
parser.AddArgument("input", "Input file", required: true);

try
{
    ArgumentsCollection args = parser.ParseArgs(Array.Empty<string>());
}
catch (ArgumentNullException ex)
{
    Console.WriteLine($"Missing required argument: {ex.ParamName}");
    // Output: Missing required argument: input
}
```

## Unknown argument

```csharp
var parser = new ArgumentParser();
parser.AddArgument("name", "Name", required: true);

try
{
    ArgumentsCollection args = parser.ParseArgs(new[] { "--name", "Alice", "--unknown", "value" });
}
catch (ArgumentException ex)
{
    Console.WriteLine(ex.Message);
    // Output: Unknown argument: unknown
}
```

## Missing value for known argument

```csharp
var parser = new ArgumentParser();
parser.AddArgument("name", "Name", required: true);

try
{
    ArgumentsCollection args = parser.ParseArgs(new[] { "--name" });
}
catch (ArgumentNullException ex)
{
    Console.WriteLine($"Missing value for: {ex.ParamName}");
    // Output: Missing value for: name
}
```

## Default value applied

```csharp
var parser = new ArgumentParser();
parser.AddArgument("verbose", "Verbose mode", required: false, defaultValue: "false");

ArgumentsCollection args = parser.ParseArgs(Array.Empty<string>());

bool verbose = args.Get<bool>("verbose");  // false (from default)
```

## Safe access with Has()

```csharp
var parser = new ArgumentParser();
parser.AddArgument("optional", "Optional arg", required: false, defaultValue: "default");

ArgumentsCollection args = parser.ParseArgs(Array.Empty<string>());

if (args.Has("optional"))
{
    string value = args.Get<string>("optional");  // Safe - key exists
}

if (!args.Has("missing"))
{
    // Handle missing - no exception
}
```

## Type conversion

```csharp
var parser = new ArgumentParser();
parser.AddArgument("port", "Port number", required: true);
parser.AddArgument("enabled", "Enabled flag", required: false, defaultValue: "true");
parser.AddArgument("ratio", "Ratio", required: false, defaultValue: "1.5");

ArgumentsCollection args = parser.ParseArgs(new[] { "--port", "8080", "--enabled", "false", "--ratio", "2.5" });

int port = args.Get<int>("port");       // 8080
bool enabled = args.Get<bool>("enabled"); // false
double ratio = args.Get<double>("ratio"); // 2.5
```

## Invalid type conversion

```csharp
var parser = new ArgumentParser();
parser.AddArgument("count", "Count", required: true);

ArgumentsCollection args = parser.ParseArgs(new[] { "--count", "not-a-number" });

try
{
    int count = args.Get<int>("count");
}
catch (InvalidCastException)
{
    Console.WriteLine("Value 'not-a-number' cannot be converted to int");
}
```

## Case sensitivity

```csharp
var parser = new ArgumentParser();
parser.AddArgument("Name", "Name", required: true);

// This works (exact case match)
ArgumentsCollection args1 = parser.ParseArgs(new[] { "--Name", "Alice" });

// This fails (case mismatch)
try
{
    ArgumentsCollection args2 = parser.ParseArgs(new[] { "--name", "Bob" });
}
catch (ArgumentException ex)
{
    Console.WriteLine(ex.Message);
    // Output: Unknown argument: name
}
```

## Iterating all parsed values

```csharp
var parser = new ArgumentParser();
parser.AddArgument("a", "A", required: true);
parser.AddArgument("b", "B", required: false, defaultValue: "default");

ArgumentsCollection args = parser.ParseArgs(new[] { "--a", "value1" });

foreach (var kvp in args)
{
    Console.WriteLine($"{kvp.Key} = {kvp.Value}");
}
// Output:
// a = value1
// b = default
```

## Real-world CLI pattern

```csharp
// Program.cs
using NuciCLI.Arguments;

var parser = new ArgumentParser();

parser.AddArgument("input", "Input file path", required: true);
parser.AddArgument("output", "Output file path", required: true);
parser.AddArgument("format", "Output format (json|xml|csv)", required: false, defaultValue: "json");
parser.AddArgument("pretty", "Pretty print output", required: false, defaultValue: "false");
parser.AddArgument("threads", "Worker threads", required: false, defaultValue: "4");

try
{
    ArgumentsCollection args = parser.ParseArgs(args);

    string input = args.Get<string>("input");
    string output = args.Get<string>("output");
    string format = args.Get<string>("format");
    bool pretty = args.Get<bool>("pretty");
    int threads = args.Get<int>("threads");

    // Validate format
    if (!new[] { "json", "xml", "csv" }.Contains(format))
    {
        Console.Error.WriteLine($"Invalid format: {format}. Use json, xml, or csv.");
        return 1;
    }

    // Validate threads
    if (threads < 1 || threads > 64)
    {
        Console.Error.WriteLine("Threads must be between 1 and 64");
        return 1;
    }

    // Process...
    Console.WriteLine($"Processing {input} -> {output} as {format} with {threads} threads");
    return 0;
}
catch (ArgumentException ex) when (ex.Message.StartsWith("Invalid argument format"))
{
    Console.Error.WriteLine("Usage: program --input <file> --output <file> [--format json|xml|csv] [--pretty true|false] [--threads <n>]");
    return 1;
}
catch (ArgumentException ex) when (ex.Message.StartsWith("Unknown argument"))
{
    Console.Error.WriteLine($"Error: {ex.Message}");
    return 1;
}
catch (ArgumentNullException ex)
{
    Console.Error.WriteLine($"Error: Missing value for --{ex.ParamName}");
    return 1;
}
catch (InvalidCastException ex)
{
    Console.Error.WriteLine($"Error: Invalid value type - {ex.Message}");
    return 1;
}
```

Run:
```bash
dotnet run -- --input data.txt --output result.json --format xml --pretty true --threads 8
```