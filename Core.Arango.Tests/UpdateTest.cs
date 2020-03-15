using System.Threading.Tasks;
using Core.Arango.Protocol;
using Core.Arango.Tests.Core;
using Xunit;

namespace Core.Arango.Tests
{
    public class UpdateTest : TestBase
    {
        [Fact]
        public async Task Collection()
        {
            await Arango.CreateCollectionAsync("test", "test", ArangoCollectionType.Document);

            await Arango.CreateDocumentAsync("test", "test", new
            {
                Key = "abc",
                Name = "a"
            });

            var res = await Arango.UpdateDocumentAsync("test", "test", new
            {
                Key = "abc",
                Name = "c"
            }, returnNew: true, returnOld: true);
        }
    }
}