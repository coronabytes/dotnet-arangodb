using System.Threading.Tasks;
using Core.Arango.Protocol;
using Core.Arango.Tests.Core;
using Newtonsoft.Json.Schema;
using Xunit;

namespace Core.Arango.Tests
{
    public class CollectionTest : TestBase
    {
        [Theory]
        [ClassData(typeof(PascalCaseData))]
        public async Task Create(string serializer)
        {
            await SetupAsync(serializer);
            await Arango.Collection.CreateAsync("test", new ArangoCollection
            {
                Name = "test",
                Type = ArangoCollectionType.Document,
                KeyOptions = new ArangoKeyOptions
                {
                    Type = ArangoKeyType.Padded,
                    AllowUserKeys = true
                }
            });

            await Arango.Document.CreateAsync("test", "test", new
            {
                Name = "test"
            });
        }

        [Theory]
        [ClassData(typeof(PascalCaseData))]
        public async Task CreateExistGetDrop(string serializer)
        {
            await SetupAsync(serializer);
            await Arango.Collection.CreateAsync("test", new ArangoCollection
            {
                Name = "test",
                Type = ArangoCollectionType.Document
            });

            Assert.True(await Arango.Collection.ExistAsync("test", "test"));

            var col = await Arango.Collection.GetAsync("test", "test");

            Assert.NotNull(col);
            Assert.Equal("test", col.Name);

            await Arango.Collection.DropAsync("test", "test");

            Assert.False(await Arango.Collection.ExistAsync("test", "test"));
        }

        [Theory]
        [ClassData(typeof(PascalCaseData))]
        public async Task Schema(string serializer)
        {
            await SetupAsync(serializer);

            await Arango.Collection.CreateAsync("test", new ArangoCollection
            {
                Name = "test",
                Type = ArangoCollectionType.Document,
                Schema = new ArangoSchema
                {
                    Rule = new
                    {
                        type = "object",
                        properties = new
                        {
                            name = new {type = "string"}
                        },
                        required = new[] { "name" },
                        additionalProperties = false
                    }
                }
            });

            await Arango.Document.CreateAsync("test", "test", new
            {
                name = "test",
            });

           await Assert.ThrowsAsync<ArangoException>(async () =>
            {
                await Arango.Document.CreateAsync("test", "test", new
                {
                    name = "test",
                    name2 = "test"
                });
            });

           await Arango.Collection.UpdateAsync("test", "test", new ArangoCollectionUpdate
           {
               Schema = null
           });

           await Arango.Document.CreateAsync("test", "test", new
           {
               name = "test",
               name2 = "test"
           });
        }
    }
}