using System;
using System.Threading.Tasks;
using Core.Arango.Protocol;
using Core.Arango.Tests.Core;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Core.Arango.Tests
{
    public class DocumentTest : TestBase
    {
        [Fact]
        public async Task Update()
        {
            await Arango.Collection.CreateAsync("test", "test", ArangoCollectionType.Document);

            await Arango.Document.CreateAsync("test", "test", new
            {
                Key = "abc",
                Name = "a"
            });

            var res = await Arango.Document.UpdateAsync("test", "test", new
            {
                Key = "abc",
                Name = "c"
            }, returnNew: true, returnOld: true);
        }

        [Fact]
        public async Task CreateUpdateMode()
        {
            await Arango.Collection.CreateAsync("test", "test", ArangoCollectionType.Document);

            await Arango.Document.CreateAsync("test", "test", new
            {
                Key = "abc",
                Name = "a"
            });

            await Arango.Document.CreateAsync("test", "test", new
            {
                Key = "abc",
                Value = "c"
            }, overwriteMode: ArangoOverwriteMode.Update);

            var obj = await Arango.Query.SingleOrDefaultAsync<JObject>("test", "test", $"x._key == {"abc"}");

            Assert.Equal("a", obj["Name"]);
            Assert.Equal("c", obj["Value"]);
        }

        [Fact]
        public async Task CreateReplaceMode()
        {
            await Arango.Collection.CreateAsync("test", "test", ArangoCollectionType.Document);

            await Arango.Document.CreateAsync("test", "test", new
            {
                Key = "abc",
                Name = "a"
            });

            await Arango.Document.CreateAsync("test", "test", new
            {
                Key = "abc",
                Value = "c"
            }, overwriteMode: ArangoOverwriteMode.Replace);

            var obj = await Arango.Query.SingleOrDefaultAsync<JObject>("test", "test", $"x._key == {"abc"}");

            Assert.Equal(null, obj["Name"]);
            Assert.Equal("c", obj["Value"]);
        }

        [Fact]
        public async Task CreateConflictMode()
        {
            await Arango.Collection.CreateAsync("test", "test", ArangoCollectionType.Document);

            await Arango.Document.CreateAsync("test", "test", new
            {
                Key = "abc",
                Name = "a"
            });

            var ex = await Assert.ThrowsAsync<ArangoException>(async () =>
            {
                await Arango.Document.CreateAsync("test", "test", new
                {
                    Key = "abc",
                    Value = "c"
                }, overwriteMode: ArangoOverwriteMode.Conflict);
            });

            Assert.Contains("unique constraint", ex.Message);
        }
    }
}