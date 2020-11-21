using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Arango.Protocol;
using Core.Arango.Tests.Core;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Core.Arango.Tests
{
    public class DocumentTest : TestBase
    {
        [Theory]
        [ClassData(typeof(PascalCaseData))]
        public async Task Get(string serializer)
        {
            await SetupAsync(serializer);
            await Arango.Collection.CreateAsync("test", "test", ArangoCollectionType.Document);

            await Arango.Document.CreateAsync("test", "test", new Entity
            {
                Key = "abc",
                Name = "a"
            });

            var doc = await Arango.Document.GetAsync<Entity>("test", "test", "abc");
            Assert.Equal("a", doc.Name);

            var nodoc = await Arango.Document.GetAsync<Entity>("test", "test", "nonexistant", false);
            Assert.Null(nodoc);

            var ex = await Assert.ThrowsAsync<ArangoException>(
                async () => await Arango.Document.GetAsync<Entity>("test", "test", "nonexistant"));

            Assert.Contains("document not found", ex.Message);
        }

        [Theory]
        [ClassData(typeof(PascalCaseData))]
        public async Task Update(string serializer)
        {
            await SetupAsync(serializer);
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

        [Theory]
        [ClassData(typeof(PascalCaseData))]
        public async Task CreateUpdateMode(string serializer)
        {
            await SetupAsync(serializer);
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

            var obj = await Arango.Query.SingleOrDefaultAsync<Dictionary<string, string>>("test", "test", $"x._key == {"abc"}");

            Assert.Equal("a", obj["Name"]);
            Assert.Equal("c", obj["Value"]);
        }

        [Theory]
        [ClassData(typeof(PascalCaseData))]
        public async Task CreateReplaceMode(string serializer)
        {
            await SetupAsync(serializer);
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

            var obj = await Arango.Query.SingleOrDefaultAsync<Dictionary<string, string>>("test", "test", $"x._key == {"abc"}");

            Assert.DoesNotContain("Name", obj.Keys);
            Assert.Equal("c", obj["Value"]);
        }

        [Theory]
        [ClassData(typeof(PascalCaseData))]
        public async Task CreateConflictMode(string serializer)
        {
            await SetupAsync(serializer);
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