# Nullable and AOT

## Nullable annotations

Every public API is annotated. Null-check helpers tell the compiler about the result:

```csharp
string? input = GetInput();

if (input.IsNotNullOrEmpty())
    Console.WriteLine(input.Length); // no warning: input is not null here
```

## Trimming and Native AOT

The package is marked `IsAotCompatible`. CI publishes a test app with Native AOT on every pull request and fails on any trimming warning.

A few reflection-based helpers cannot be fully trimmed. They carry `[RequiresUnreferencedCode]` or `[RequiresDynamicCode]`, so you get a warning at the call site:

- `TypesExtensions.GetExtensionMethods`, `GetExtensionMethod`, `GetRealType`
- `PropertyInfoExtensions.TrySetConvertedValue`
- `StringConversionExtensions.ToNullable`
