using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Core.Arango.Tests.Core;
using Xunit;
using Xunit.Abstractions;

namespace Core.Arango.Tests;

/// <summary>
///     Example: use BeforeRequest / AfterResponse for HTTP-level observability
///     (URI, method, status code) without coupling to a specific logging framework.
/// </summary>
public class RequestCallbackTest(ITestOutputHelper output) : TestBase
{
    private int _beforeRequestCount;
    private int _afterResponseCount;

    [Fact]
    public async Task BeforeRequest_And_AfterResponse_Are_Invoked()
    {
        var exists = await Arango.Database.ExistAsync("test", CancellationToken.None);

        Assert.False(exists);
        Assert.True(_beforeRequestCount > 0);
        Assert.True(_afterResponseCount > 0);
    }

    [Fact]
    public async Task Throwing_Hooks_Do_Not_Break_Request()
    {
        Arango.Configuration.BeforeRequest = _ => throw new InvalidOperationException("before");
        Arango.Configuration.AfterResponse = _ => throw new InvalidOperationException("after");

        var exists = await Arango.Database.ExistAsync("test", CancellationToken.None);

        Assert.False(exists);
    }

    public override async Task InitializeAsync()
    {
        // Typical usage: attach lightweight HTTP tracing when creating the context.
        Arango = new ArangoContext(UniqueTestRealm(), new ArangoConfiguration
        {
            BeforeRequest = request =>
            {
                _beforeRequestCount++;

                // e.g. logger.LogDebug("Sending {Method} to {Uri}", request.Method, request.RequestUri);
                output.WriteLine($"Sending {request.Method} request to {request.RequestUri}");
            },
            AfterResponse = response =>
            {
                _afterResponseCount++;

                // e.g. logger.LogDebug("Received {StatusCode} ({Reason}) from {Uri}",
                //     (int)response.StatusCode, response.ReasonPhrase, response.RequestMessage?.RequestUri);
                output.WriteLine(
                    $"Received {(int)response.StatusCode} ({response.ReasonPhrase}) from {response.RequestMessage?.RequestUri}");
            }
        });

        await Task.CompletedTask;
    }
}
