using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Arango.Protocol;
using Core.Arango.Tests.Core;
using Xunit;

namespace Core.Arango.Tests
{
    public class QueryTest : TestBase
    {
        [Fact]
        public async Task NullParameter()
        {
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

        [Fact]
        public async Task QueryIntegerContains()
        {
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
    }
}