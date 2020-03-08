using System.Threading.Tasks;
using Core.Arango.Protocol;
using Xunit;

namespace Core.Arango.Tests
{
    public class CollectionTest : TestBase
    {
        [Fact]
        public async Task Collection()
        {
            Assert.True(await Arango.CreateDatabaseAsync("collections"));

            await Arango.CreateCollectionAsync("collections", new ArangoCollection
            {
                Name = "test",
                Type = ArangoCollectionType.Document,
                KeyOptions = new ArangoKeyOptions
                {
                    Type = ArangoKeyType.Padded,
                    AllowUserKeys = false
                }
            });

            await Arango.CreateDocumentAsync("collections", "test", new
            {
                Name = "test"
            });
        }
    }
}