![.NET Core](https://github.com/coronabytes/ArangoDB/workflows/.NET%20Core/badge.svg)

# Disclaimer
- This is a minimalistic .NET driver for ArangoDB (3.5+) with adapters to Serilog and DevExtreme
- The key difference to any other available driver is the ability to switch databases on a per request basis, which allows for easy database per tenant deployments
- Id, Key, From, To properties will always be translated to their respective arango form (_id, _key, _from, _to), which allows to construct updates from anonymous types
- First parameter of any method in most cases is an ArangoHandle which has implicit conversion from string and GUID
  - e.g. "master" and "logs" database and GUID for each tenant

## Initialize context
- Realm prefixes all further database handles (e.g. "myproject-database")
```csharp
var arango = new ArangoContext("Server=http://localhost:8529;Realm=myproject;User=root;Password=;");
```

## Create collection
```csharp
await arango.CreateCollectionAsync("database", "collection", ArangoCollectionType.Document);
```

## Create index
```csharp
await arango.EnsureIndexAsync("database", "collection", new ArangoIndex
{
    Fields = new List<string> {"SomeValue"},
    Type = ArangoIndexType.Hash
});
```

## Create analyzer
```csharp
await arango.CreateAnalyzerAsync("database", new ArangoAnalyzer
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
await arango.CreateViewAsync("database", new ArangoView
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
await arango.CreateGraphAsync("database", new ArangoGraph
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
await arango.CreateDocumentAsync("database", "collection", new
{
    Key = Guid.NewGuid(),
    SomeValue = 1
});
```

## Update document
```csharp
await arango.UpdateDocumentAsync("database", "collection", new
{
    Key = Guid.Parse("some-guid"),
    SomeValue = 2
});
```

## Query with bind vars through string interpolation
```csharp
var list = new List<int> {1, 2, 3};

var result = await arango.QueryAsync<JObject>("database",
  $"FOR c IN collection FILTER c.SomeValue IN {list} RETURN c");
```

## Stream transactions
```csharp
var transaction = await arango.BeginTransactionAsync("database", new ArangoTransaction
{
    Collections = new ArangoTransactionScope
    {
        Write = new List<string> { "collection" }
    }
});

await arango.CreateDocumentAsync(transaction, "collection", new
{
    Key = Guid.NewGuid(),
    SomeValue = 1
});

await arango.CreateDocumentAsync(transaction, "collection", new
{
    Key = Guid.NewGuid(),
    SomeValue = 2
});

await arango.CommitTransactionAsync(transaction);
```

# Serilog
- Collection is only created on initial database creation for now
- If you want to use an existing database create the collection manually
```csharp
webBuilder.UseSerilog((c, log) =>
{
    var arango = c.Configuration.GetConnectionString("Arango");

    log.MinimumLevel.Debug()
        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
        .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
        .Enrich.FromLogContext()
        .WriteTo.Sink(new ArangoSerilogSink(new ArangoContext(arango), 
            database: "logs", 
            collection: "logs", 
            batchPostingLimit: 50, 
            TimeSpan.FromSeconds(2)), 
            restrictedToMinimumLevel: LogEventLevel.Information);

    // This is unreliable...
    if (Environment.UserInteractive)
        log.WriteTo.Console(theme: AnsiConsoleTheme.Code);
});
```

# DataProtection
- Collection is only created on initial database creation for now
- If you want to use an existing database create the collection manually
```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddSingleton(new ArangoContext(Configuration.GetConnectionString("Arango")));

    var dataProtection = services.AddDataProtection()
        .SetApplicationName("App")
        .SetDefaultKeyLifetime(TimeSpan.FromDays(30));
    dataProtection.PersistKeysToArangoDB(database: "dataprotection", collection: "keys");
}
```

# DevExtreme Query
- Translates DevExtreme queries to AQL with filtering, sorting, grouping and summaries on a 'best effort basis'
- DataSourceLoadOptions need to be parsed by Newtonsoft Json and not System.Text.Json
  - services.AddControllers().AddNewtonsoftJson();
- Parameters are escaped with bindvars
- Property names are not - may include security filter later
- Developer retains full control over the projection - full document by default
- Check safety limits in settings if your query fails
- Support for ArangoSearch is coming soon
```csharp

private static readonly ArangoTransformSettings Transform = new ArangoTransformSettings
{
    IteratorVar = "x",
    Key = "key",
    Filter = "x.Active == true",
    RestrictGroups = new HashSet<string>() // No Grouping allowed
};

[HttpGet("dx-query")]
public async Task<ActionResult<DxLoadResult>> DxQuery([FromQuery] DataSourceLoadOptions loadOptions)
{
    var arangoTransform = new ArangoTransform(loadOptions, Transform);

    if (!arangoTransform.Transform(out var error))
        return BadRequest(error);

    return await arangoTransform.ExecuteAsync<SomeEntity>(arango, "database", "collection");
}
```
