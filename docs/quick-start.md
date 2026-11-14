# Quick start

## Install

```bash
dotnet add package NuciCLI.Arguments
```

## Basic usage

```csharp
using NuciCLI.Arguments;

var parser = new ArgumentParser();

parser.AddArgument("name", "Your name", required: true);
parser.AddArgument("age", "Your age", required: false, defaultValue: "25");

ArgumentsCollection args = parser.ParseArgs(new[] { "--name", "Alice", "--age", "30" });

string name = args.Get<string>("name");  // "Alice"
int age = args.Get<int>("age");          // 30
```

## Step by step

1. **Create parser**
   ```csharp
   var parser = new ArgumentParser();
   ```

2. **Register arguments**
   ```csharp
   parser.AddArgument(
       name: "output",
       help: "Output file path",
       required: true);

   parser.AddArgument(
       name: "verbose",
       help: "Enable verbose output",
       required: false,
       defaultValue: "false");
   ```

3. **Parse command line**
   ```csharp
   // In Program.cs:
   ArgumentsCollection args = parser.ParseArgs(args);
   ```

4. **Access values**
   ```csharp
   string output = args.Get<string>("output");
   bool verbose = args.Get<bool>("verbose");
   ```

## Complete example

```csharp
// Program.cs
using NuciCLI.Arguments;

var parser = new ArgumentParser();

parser.AddArgument("input", "Input file", required: true);
parser.AddArgument("output", "Output file", required: true);
parser.AddArgument("format", "Output format", required: false, defaultValue: "json");
parser.AddArgument("compress", "Compress output", required: false, defaultValue: "false");

try
{
    ArgumentsCollection args = parser.ParseArgs(args);

    string input = args.Get<string>("input");
    string output = args.Get<string>("output");
    string format = args.Get<string>("format");
    bool compress = args.Get<bool>("compress");

    // Your logic here
    Console.WriteLine($"Processing {input} -> {output} ({format})");
}
catch (ArgumentException ex) when (ex.Message.StartsWith("Invalid argument format"))
{
    Console.Error.WriteLine("Error: Arguments must use --name format");
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

return 0;
```

## Run

```bash
dotnet run -- --input data.txt --output result.json --format xml --compress true
```

## Key points

- All arguments use `--name value` format
- Names are case-sensitive
- Required arguments throw if missing
- Optional arguments get default value if not provided
- `Get<T>` throws if key missing or type mismatch
- Use `Has("key")` to check existence safely