# API reference

This is a **.NET class library**, not an HTTP/Web API. There is no OpenAPI/Swagger specification.

## Public API surface

All public types are in the `NuciCLI.Arguments` namespace.

### ArgumentParser

```csharp
public sealed class ArgumentParser
{
    public void AddArgument(Argument argument);
    public ArgumentsCollection ParseArgs(string[] args);
}
```

### Argument

```csharp
public sealed class Argument
{
    public Argument(string name, string description = "", bool isRequired = false, object? defaultValue = null);
    public string Name { get; init; }
    public string Description { get; init; }
    public bool IsRequired { get; init; }
    public object? DefaultValue { get; init; }
}
```

### ArgumentsCollection

```csharp
public sealed class ArgumentsCollection : IEnumerable<KeyValuePair<string, object>>
{
    public object? this[string key] { get; set; }
    public bool Has(string key);
    public T Get<T>(string key);
    public IEnumerator<KeyValuePair<string, object>> GetEnumerator();
}
```

## Versioning

Single version per NuGet package. No API versioning scheme.

## See also

- [components/argument-parser.md](components/argument-parser.md)
- [components/argument.md](components/argument.md)
- [components/arguments-collection.md](components/arguments-collection.md)
- [api-usage-examples.md](api-usage-examples.md)