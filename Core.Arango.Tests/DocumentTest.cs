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
        public async Task Get()
        {
            await Arango.Collection.CreateAsync("test", "test", ArangoCollectionType.Document);

            await Arango.Document.CreateAsync("test", "test", new
            {
                Key = "abc",
                Name = "a"
            });

            var doc = await Arango.Document.GetAsync<JObject>("test", "test", "abc");
            Assert.Equal("a", doc["Name"]);

            var nodoc = await Arango.Document.GetAsync<JObject>("test", "test", "nonexistant", false);
            Assert.Null(nodoc);

            var ex = await Assert.ThrowsAsync<ArangoException>(
                async () => await Arango.Document.GetAsync<JObject>("test", "test", "nonexistant"));

            Assert.Contains("document not found", ex.Message);
        }

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
            if (await Arango.GetVersionAsync() < Version.Parse("3.7"))
                return;

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
            if (await Arango.GetVersionAsync() < Version.Parse("3.7"))
                return;

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

            Assert.Null(obj["Name"]);
            Assert.Equal("c", obj["Value"]);
        }

        [Fact]
        public async Task CreateConflictMode()
        {
            if (await Arango.GetVersionAsync() < Version.Parse("3.7"))
                return;

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