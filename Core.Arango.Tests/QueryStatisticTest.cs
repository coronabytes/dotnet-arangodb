using Core.Arango.Tests.Core;
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

        /*public override async Task InitializeAsync()
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
        public async Task QueryStats()
        {
            await Arango.Collection.CreateAsync("test", "test", ArangoCollectionType.Document);

            await Arango.Document.CreateManyAsync("test", "test", new List<Entity>
            {
                new Entity {Value = 1},
                new Entity {Value = 2},
                new Entity {Value = 3},
                new Entity {Value = 4},
                new Entity {Value = 5}
            });

            var select = new List<int> {1, 2, 3};

            var res = await Arango.Query.ExecuteAsync<Entity>("test",
                $"FOR e IN test FILTER e.Value IN {select} LIMIT 2 RETURN e", fullCount: true);

            Assert.Equal(2, res.Count);
            Assert.Equal(3, res.FullCount);
        }*/
    }
}