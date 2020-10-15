![Build](https://github.com/coronabytes/dotnet-arangodb/workflows/Build/badge.svg)
![Nuget](https://img.shields.io/nuget/v/Core.Arango)
![Nuget](https://img.shields.io/nuget/dt/Core.Arango)

# .NET driver for ArangoDB
- .NET Standard 2.1 driver for ArangoDB 3.6 and 3.7+
- The key difference to any other available driver is the ability to switch databases on a per request basis, which allows for easy database per tenant deployments
- Id, Key, From, To properties will always be translated to their respective arango form (_id, _key, _from, _to), which allows to construct updates from anonymous types
- First parameter of any method in most cases is an ArangoHandle which has implicit conversion from string and GUID
  - e.g. "master" and "logs" database and GUID for each tenant
- It does not support optimistic concurrency with _rev as constructing patch updates is way easier

## Initialize context
- Realm optionally prefixes all further database handles (e.g. "myproject-database")
- Context is completely thread-safe an can be shared for your whole application
```csharp
var arango = new ArangoContext("Server=http://localhost:8529;Realm=myproject;User=root;Password=;");
```
- For AspNetCore DI extension is available:
```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddArango(Configuration.GetConnectionString("Arango"))
}
```

```csharp
[ApiController]
[Route("api/demo")]
public class DemoController : Controller 
{
    private readonly IArangoContext _arango;

    public DemoController(IArangoContext arango)
    {
        _arango = arango;
    }
}
```

## Create collection
```csharp
await arango.Collection.CreateAsync("database", "collection", ArangoCollectionType.Document);
```

## Create index
```csharp
await arango.Index.CreateAsync("database", "collection", new ArangoIndex
{
    Fields = new List<string> {"SomeValue"},
    Type = ArangoIndexType.Hash
});
```

## Create analyzer
```csharp
await arango.Analyzer.CreateAsync("database", new ArangoAnalyzer
{
    Name = "text_de_nostem",
    Type = "text",
    Properties = new ArangoAnalyzerProperties
    {
        Locale = "de.utf-8",
        Case = "lower",
        Accent = false,
        Stopwords = new List<string>(),
        Stemming = false
    },
    Features = new List<string> { "position", "norm", "frequency" }
});
```

## Create view
```csharp
await arango.View.CreateAsync("database", new ArangoView
{
    Name = "SomeView",
    Links = new Dictionary<string, ArangoLinkProperty>
    {
        ["collection"] = new ArangoLinkProperty
        {
            Fields = new Dictionary<string, ArangoLinkProperty>
            {
                ["SomeProperty"] = new ArangoLinkProperty
                {
                    Analyzers = new List<string>
                    {
                        "text_en"
                    }
                }
            }
        }
    },
    PrimarySort = new List<ArangoSort>
    {
        new ArangoSort
        {
            Field = "SomeProperty",
            Direction = "asc"
        }
    }
});
```

## Create graph
```csharp
await arango.Graph.CreateAsync("database", new ArangoGraph
{
    Name = "SomeGraph",
    EdgeDefinitions = new List<ArangoEdgeDefinition>
    {
        new ArangoEdgeDefinition
        {
            Collection = "collection_edges",
            From = new List<string> {"collection"},
            To = new List<string> {"collection"}
        }
    }
});
```

## Create document
```csharp
await arango.Document.CreateAsync("database", "collection", new
{
    Key = Guid.NewGuid(),
    SomeValue = 1
});
```

## Update document
```csharp
await arango.Document.UpdateAsync("database", "collection", new
{
    Key = Guid.Parse("some-guid"),
    SomeValue = 2
});
```

## Query with bind vars through string interpolation
```csharp
var list = new List<int> {1, 2, 3};

var result = await arango.Query.ExecuteAsync<JObject>("database",
  $"FOR c IN collection FILTER c.SomeValue IN {list} RETURN c");
```

## Stream transactions
```csharp
var transaction = await arango.Transaction.BeginAsync("database", new ArangoTransaction
{
    Collections = new ArangoTransactionScope
    {
        Write = new List<string> { "collection" }
    }
});

await arango.Document.CreateAsync(transaction, "collection", new
{
    Key = Guid.NewGuid(),
    SomeValue = 1
});

await arango.Document.CreateAsync(transaction, "collection", new
{
    Key = Guid.NewGuid(),
    SomeValue = 2
});

await arango.Transaction.CommitAsync(transaction);
```
