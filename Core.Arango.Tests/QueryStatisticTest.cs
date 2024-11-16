using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Arango.Protocol;
using Core.Arango.Tests.Core;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace Core.Arango.Tests
{
    public class QueryStatisticTest : TestBase
    {
        private readonly ITestOutputHelper _output;

        public QueryStatisticTest(ITestOutputHelper output)
        {
            _output = output;
        }

        public override async ValueTask InitializeAsync()
        {
            Arango =
                new ArangoContext(UniqueTestRealm(),
                    new ArangoConfiguration
                    {
                        QueryProfile = (query, bindVars, stats) =>
                        {
                            var boundQuery = query;

                            foreach (var p in bindVars.OrderByDescending(x => x.Key.Length))
                                boundQuery = boundQuery.Replace("@" + p.Key, JsonConvert.SerializeObject(p.Value));

                            _output.WriteLine(
                                $"{boundQuery}\n{JsonConvert.SerializeObject(stats, Formatting.Indented)}");
                        }
                    });
            await Arango.Database.CreateAsync("test");
        }

        [Fact]
        public async ValueTask QueryStats()
        {
            await Arango.Collection.CreateAsync("test", "test", ArangoCollectionType.Document);

            await Arango.Document.CreateManyAsync("test", "test", new List<Entity>
            {
                new() {Value = 1},
                new() {Value = 2},
                new() {Value = 3},
                new() {Value = 4},
                new() {Value = 5}
            });

            var select = new List<int> {1, 2, 3};

            var res = await Arango.Query.ExecuteAsync<Entity>("test",
                $"FOR e IN test FILTER e.Value IN {select} LIMIT 2 RETURN e", fullCount: true);

            Assert.Equal(2, res.Count);
            Assert.Equal(3, res.FullCount);
        }
    }
}