# Quick start

## Install

```bash
dotnet add package NuciCLI.Arguments
```

## Basic usage

```csharp
using NuciCLI.Arguments;

var parser = new ArgumentParser();

parser.AddArgument(new Argument("name", "Your name", isRequired: true));
parser.AddArgument(new Argument("age", "Your age", isRequired: false, defaultValue: "25"));

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
   parser.AddArgument(new Argument(
       name: "output",
       description: "Output file path",
       isRequired: true
   ));

   parser.AddArgument(new Argument(
       name: "verbose",
       description: "Enable verbose output",
       isRequired: false,
       defaultValue: "false"
   ));
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

parser.AddArgument(new Argument("input", "Input file", isRequired: true));
parser.AddArgument(new Argument("output", "Output file", isRequired: true));
parser.AddArgument(new Argument("format", "Output format", isRequired: false, defaultValue: "json"));
parser.AddArgument(new Argument("compress", "Compress output", isRequired: false, defaultValue: "false"));

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