using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Arango.Protocol;
using Core.Arango.Tests.Core;
using Xunit;

namespace Core.Arango.Tests
{
    public class QueryTest : TestBase
    {
        [Theory]
        [ClassData(typeof(PascalCaseData))]
        public async Task NullParameter(IArangoContext arango)
        {
            await SetupAsync(arango);
            await Arango.Collection.CreateAsync("test", "test", ArangoCollectionType.Document);
            await Arango.Document.CreateManyAsync("test", "test", new List<Entity>
            {
                new Entity {Value = 1},
                new Entity {Value = 2},
                new Entity {Value = 3}
            });

            Guid? nullParam = null;

            var res = await Arango.Query.SingleOrDefaultAsync<Entity>("test", "test",
                $"x.Value == {nullParam}");

            var res2 = await Arango.Query.SingleOrDefaultAsync<Entity>("test", "test",
                $"x.Value == {null}");
        }

        [Theory]
        [ClassData(typeof(PascalCaseData))]
        public async Task QueryIntegerContains(IArangoContext arango)
        {
            await SetupAsync(arango);
            await Arango.Collection.CreateAsync("test", "test", ArangoCollectionType.Document);
            await Arango.Document.CreateManyAsync("test", "test", new List<Entity>
            {
                new Entity {Value = 1},
                new Entity {Value = 2},
                new Entity {Value = 3}
            });

            var select = new List<int> {1, 2};

            var res = await Arango.Query.ExecuteAsync<Entity>("test",
                $"FOR e IN test FILTER e.Value IN {select} RETURN e");

            Assert.Equal(2, res.Count);
        }

        [Theory]
        [ClassData(typeof(PascalCaseData))]
        public async Task Batch(IArangoContext arango)
        {
            await SetupAsync(arango);
            await Arango.Collection.CreateAsync("test", "test", ArangoCollectionType.Document);

            await Arango.Document.CreateManyAsync("test", "test", 
                Enumerable.Range(1, 100000)
                    .Select(x => new Entity { Value = x }));

            var res = await Arango.Query.ExecuteAsync<string>("test",
                $"FOR e IN test RETURN e._id");

            Assert.Equal(100000, res.Count);
        }

        [Theory]
        [ClassData(typeof(PascalCaseData))]
        public async Task BatchStream(IArangoContext arango)
        {
            await SetupAsync(arango);
            await Arango.Collection.CreateAsync("test", "test", ArangoCollectionType.Document);

            await Arango.Document.CreateManyAsync("test", "test",
                Enumerable.Range(1, 100000)
                    .Select(x => new Entity { Value = x }));

            int i = 0;

            await foreach (var x in Arango.Query.ExecuteStreamAsync<string>("test", 
                $"FOR e IN test RETURN e._id", batchSize: 1000))
                ++i;

            Assert.Equal(100000, i);
        }
    }
}