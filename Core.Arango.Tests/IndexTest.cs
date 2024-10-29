using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Arango.Protocol;
using Core.Arango.Tests.Core;
using Xunit;

namespace Core.Arango.Tests
{
    public class IndexTest : TestBase
    {
        public IndexTest()
        {
            InitializeAsync().ConfigureAwait(false).GetAwaiter().GetResult();
        }

        [Theory]
        [ClassData(typeof(PascalCaseData))]
        public async Task DropAll(string serializer)
        {
            await SetupAsync(serializer);
            await Arango.Collection.CreateAsync("test", "test", ArangoCollectionType.Document);

            await Arango.Index.CreateAsync("test", "test", new ArangoIndex
            {
                Fields = new List<string> {"test"},
                Type = ArangoIndexType.Hash
            });

            await Arango.Index.DropAllAsync("test");
        }
    }
}