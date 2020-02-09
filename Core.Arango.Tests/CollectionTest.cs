using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Core.Arango.Protocol;
using Xunit;

namespace Core.Arango.Tests
{
    public class CollectionTest
    {
        [Fact]
        public async Task Collection()
        {
            var arango = new ArangoContext("Server=http://localhost:8529;Realm=unittest;User ID=root;Password=;");
            await arango.RefreshJwtAuth();

            await arango.CreateDatabaseAsync("collections");

            await arango.CreateCollectionAsync("collections", new ArangoCollection
            {
                Name = "test",
                Type = ArangoCollectionType.Document,
                KeyOptions = new ArangoKeyOptions
                {
                    Type = ArangoKeyType.Padded,
                    AllowUserKeys = false
                }
            });

            await arango.CreateDocumentAsync("collections", "test", new
            {
                Name = "test"
            });
        }
    }
}
