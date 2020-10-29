using System.Threading.Tasks;
using Core.Arango.Protocol;
using Core.Arango.Tests.Core;
using Xunit;

namespace Core.Arango.Tests
{
    public class CollectionTest : TestBase
    {
        [Fact]
        public async Task Create()
        {
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

        [Fact]
        public async Task CreateExistGetDrop()
        {
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
    }
}