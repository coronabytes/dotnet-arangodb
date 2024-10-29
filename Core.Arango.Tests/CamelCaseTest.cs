using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Arango.Protocol;
using Core.Arango.Tests.Core;
using Xunit;

namespace Core.Arango.Tests
{
    public class CamelCaseTest : TestBase
    {
        public CamelCaseTest()
        {
            InitializeAsync().ConfigureAwait(false).GetAwaiter().GetResult();
        }

        [Theory]
        [ClassData(typeof(CamelCaseData))]
        public async Task GetCamelCase(string serializer)
        {
            await SetupAsync(serializer);
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