using System.Threading.Tasks;
using Core.Arango.Protocol;
using Core.Arango.Tests.Core;
using Xunit;

namespace Core.Arango.Tests
{
    public class CollectionTest : TestBase
    {
        [Fact]
        public async Task Collection()
        {
            await Arango.CreateCollectionAsync("test", new ArangoCollection
            {
                Name = "test",
                Type = ArangoCollectionType.Document,
                KeyOptions = new ArangoKeyOptions
                {
                    Type = ArangoKeyType.Padded,
                    AllowUserKeys = false
                }
            });

            await Arango.CreateDocumentAsync("test", "test", new
            {
                Name = "test"
            });
        }
    }
}