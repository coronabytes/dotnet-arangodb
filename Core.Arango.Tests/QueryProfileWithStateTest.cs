using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Arango.Protocol;
using Core.Arango.Tests.Core;
using Xunit;
using Xunit.Abstractions;

namespace Core.Arango.Tests;

/// <summary>
///     Example: pass per-call state into Execute* so QueryProfileWithState can correlate
///     query stats with a logger, request scope, or other ambient context.
/// </summary>
public class QueryProfileWithStateTest(ITestOutputHelper output) : TestBase
{
    private long? _lastReturnCount;
    private object _lastState;
    private ArangoQueryStatistic _lastStats;

    [Fact]
    public async Task ExecuteAsync_Passes_State_To_QueryProfileWithState()
    {
        await Arango.Collection.CreateAsync("test", "test", ArangoCollectionType.Document);

        await Arango.Document.CreateManyAsync("test", "test", new List<Entity>
        {
            new() { Value = 1 },
            new() { Value = 2 },
            new() { Value = 3 }
        });

        // Typical usage: pass a per-request logger / correlation context as state.
        var requestScope = new QueryRequestScope("req-123");

        var res = await Arango.Query.ExecuteAsync<Entity>(
            "test",
            $"FOR e IN test FILTER e.Value >= {2} RETURN e",
            requestScope);

        Assert.Equal(2, res.Count);
        Assert.Same(requestScope, _lastState);
        Assert.Equal(2, _lastReturnCount);
        Assert.NotNull(_lastStats);
    }

    public override async Task InitializeAsync()
    {
        Arango = new ArangoContext(UniqueTestRealm(), new ArangoConfiguration
        {
            QueryProfileWithState = (query, bindVars, stats, returnCount, state) =>
            {
                _lastReturnCount = returnCount;
                _lastState = state;
                _lastStats = stats;

                // e.g. ((ILogger)state).LogInformation("query={Query} count={Count} time={Time}",
                //     query, returnCount, stats?.ExecutionTime);
                var scope = state as QueryRequestScope;
                output.WriteLine(
                    $"[{scope?.CorrelationId}] count={returnCount}, time={stats?.ExecutionTime}, query={query}");
            }
        });

        await Arango.Database.CreateAsync("test");
    }

    /// <summary>Stand-in for a logger or request-scoped object passed as Execute state.</summary>
    private sealed class QueryRequestScope(string correlationId)
    {
        public string CorrelationId { get; } = correlationId;
    }
}
