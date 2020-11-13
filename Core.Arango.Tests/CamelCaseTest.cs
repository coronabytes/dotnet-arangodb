using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Arango.Protocol;
using Core.Arango.Serialization;
using Core.Arango.Tests.Core;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Core.Arango.Tests
{
    public class CamelCaseTest : TestBase
    {
       public override async Task InitializeAsync()
        {
            Arango =
                new ArangoContext(UniqueTestRealm(),
                    new ArangoConfiguration
                    {
                        Serializer = new ArangoJsonNetSerializer(new ArangoCamelCaseContractResolver())
                    });
            await Arango.Database.CreateAsync("test");
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

            var doc = await Arango.Document.GetAsync<Dictionary<string, string>>("test", "test", "abc");

            Assert.Equal("a", doc["name"]);
            Assert.Equal("b", doc["someName"]);
        }
    }
}