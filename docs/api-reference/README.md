# API reference

This is a **.NET class library**, not an HTTP/Web API. There is no OpenAPI/Swagger specification.

## Public API surface

All public types are in the `NuciCLI.Arguments` namespace.

### ArgumentParser

```csharp
public class ArgumentParser
{
    public void AddArgument(string name, string help = "", bool required = false, object? defaultValue = null);
    public ArgumentsCollection ParseArgs(string[] args);
}
```

### Argument

```csharp
public class Argument
{
    public Argument(string name, string help, bool isRequired, object? defaultValue = null);
    public string Name { get; }
    public string Description { get; }
    public bool IsRequired { get; }
    public object? DefaultValue { get; }
}
```

### ArgumentsCollection

```csharp
public class ArgumentsCollection : IEnumerable<KeyValuePair<string, object?>>
{
    public object? this[string key] { get; set; }
    public bool Has(string key);
    public T Get<T>(string key);
    public IEnumerator<KeyValuePair<string, object?>> GetEnumerator();
}
```

## Versioning

Single version per NuGet package. No API versioning scheme.

## See also

- [components/argument-parser.md](components/argument-parser.md)
- [components/argument.md](components/argument.md)
- [components/arguments-collection.md](components/arguments-collection.md)
- [api-usage-examples.md](api-usage-examples.md)