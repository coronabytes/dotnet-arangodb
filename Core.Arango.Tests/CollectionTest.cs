using System;
using System.Threading.Tasks;
using Core.Arango.Protocol;
using Core.Arango.Tests.Core;
using Xunit;
using Xunit.Abstractions;

namespace Core.Arango.Tests
{
    public class CollectionTest : TestBase
    {
        private readonly ITestOutputHelper _output;

        public CollectionTest(ITestOutputHelper output)
        {
            _output = output;
        }

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
            if (await Arango.GetVersionAsync() < Version.Parse("3.7"))
                return;

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
                        required = new[] { "name" }
                        //additionalProperties = true
                    }
                }
            });

            await Arango.Document.CreateAsync("test", "test", new
            {
                name = "test",
            });

            var exception = await Assert.ThrowsAsync<ArangoException>(async () =>
            {
                await Arango.Document.CreateAsync("test", "test", new
                {
                    name = 2,
                    name2 = "test"
                });
            });

            Assert.Collection<ArangoError>(exception.Errors, 
                error => Assert.Equal(ArangoErrorCode.ErrorValidationFailed, error.ErrorNumber)
            );

           await Arango.Collection.UpdateAsync("test", "test", new ArangoCollectionUpdate
           {
               Schema = null
           });

           await Task.Delay(5000);

           await Arango.Document.CreateAsync("test", "test", new
           {
               name = 2,
               name2 = "test"
           });
        }

        [Theory]
        [ClassData(typeof(PascalCaseData))]
        public async Task CollectionParameter(string serializer)
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

            var test = "test";
            var res = await Arango.Query.ExecuteAsync<ArangoVoid>("test", 
                $"FOR x IN @{test} FILTER x.Name == {test} RETURN x");

            Assert.Single(res);
        }
    }
}