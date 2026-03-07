# ClearlyDefined.NET

[![Build](https://img.shields.io/github/actions/workflow/status/JamieMagee/ClearlyDefined.NET/build.yml?branch=main&style=for-the-badge)](https://github.com/JamieMagee/ClearlyDefined.NET/actions/workflows/build.yml?query=branch%3Amain)

A .NET client for the [ClearlyDefined](https://clearlydefined.io) API. Looks up open source component licenses, curations, and harvest data.

## Install

```
dotnet add package ClearlyDefined.Schema
```

## Quick start

```csharp
var client = new ClearlyDefinedClient();

// Batch lookup
var definitions = await client.GetDefinitionsAsync(["npm/npmjs/-/redie/0.3.0"]);

// Single lookup with typed coordinates
var coords = new ComponentCoordinates
{
    Type = ComponentType.Npm,
    Provider = ComponentProvider.NpmJs,
    Namespace = "-",
    Name = "redie",
    Revision = "0.3.0",
};
var definition = await client.GetDefinitionAsync(coords);
Console.WriteLine(definition.Licensed?.Declared); // "MIT"
```

## Dependency injection

Register with `IHttpClientFactory`:

```csharp
services.AddClearlyDefinedClient();
```

Then inject `IClearlyDefinedClient` wherever you need it. To point at a different environment:

```csharp
services.AddClearlyDefinedClient(http =>
{
    http.BaseAddress = new Uri("https://dev-api.clearlydefined.io/");
});
```

## API coverage

Definitions, curations, harvest, attachments, and notices — everything in the [ClearlyDefined API](https://api.clearlydefined.io/api-docs) has a corresponding method on `IClearlyDefinedClient`.

## License

[MIT](LICENSE.md)
