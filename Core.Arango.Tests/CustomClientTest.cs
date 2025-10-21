using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Core.Arango.Tests.Core;
using Xunit;

namespace Core.Arango.Tests;

public class CustomClientTest : TestBase
{
    private static bool _usedTheHttpClient = false;

    private class SimpleTransport : DelegatingHandler
    {
        public SimpleTransport()
        {
            InnerHandler = new HttpClientHandler();
        }
        
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            _usedTheHttpClient = true;
            return await base.SendAsync(request, cancellationToken);
        }
    }

    private readonly HttpClient _client = new HttpClient(new SimpleTransport());

    [Fact]
    public async Task Test()
    {
        var exists = await Arango.Database.ExistAsync("test", CancellationToken.None);
        Assert.False(exists);

        Assert.True(_usedTheHttpClient);
    }

    public override async Task InitializeAsync()
    {
        Arango = new ArangoContext(UniqueTestRealm(), new ArangoConfiguration
        {
            HttpClient = _client
        });

        await Task.CompletedTask;
    }
}