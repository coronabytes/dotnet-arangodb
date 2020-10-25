using System;
using System.Threading.Tasks;
using Core.Arango.Protocol;
using Core.Arango.Serialization;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Core.Arango.Tests
{
    public class CamelCaseTest : IAsyncLifetime
    {
        protected readonly IArangoContext Arango =
            new ArangoContext($"Server=http://localhost:8529;Realm=CI-{Guid.NewGuid():D};User=root;Password=;",
                new ArangoConfiguration
                {
                    Serializer = new ArangoJsonNetSerializer(new ArangoCamelCaseContractResolver())
                });

        public async Task InitializeAsync()
        {
            await Arango.Database.CreateAsync("test");
        }

        public async Task DisposeAsync()
        {
            try
            {
                foreach (var db in await Arango.Database.ListAsync())
                    await Arango.Database.DropAsync(db);
            }
            catch
            {
                //
            }
        }

        [Fact]
        public async Task GetCamelCase()
        {
            await Arango.Collection.CreateAsync("test", "test", ArangoCollectionType.Document);

            await Arango.Document.CreateAsync("test", "test", new
            {
                Key = "abc",
                Name = "a",
                SomeName = "b"
            });

            var doc = await Arango.Document.GetAsync<JObject>("test", "test", "abc");

            Assert.Equal("a", doc["name"]);
            Assert.Equal("b", doc["someName"]);
        }
    }
}