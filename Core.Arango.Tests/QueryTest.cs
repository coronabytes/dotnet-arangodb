using System;
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
    public class QueryTest : TestBase
    {
        private readonly ITestOutputHelper _output;

        public QueryTest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Theory]
        [ClassData(typeof(PascalCaseData))]
        public async Task NullParameter(string serializer)
        {
            await SetupAsync(serializer);
            await Arango.Collection.CreateAsync("test", "test", ArangoCollectionType.Document);
            await Arango.Document.CreateManyAsync("test", "test", new List<Entity>
            {
                new() {Value = 1},
                new() {Value = 2},
                new() {Value = 3}
            });

            Guid? nullParam = null;

            var res = await Arango.Query.SingleOrDefaultAsync<Entity>("test", "test",
                $"x.Value == {nullParam}");

            var res2 = await Arango.Query.SingleOrDefaultAsync<Entity>("test", "test",
                $"x.Value == {null}");
        }

        [Theory]
        [ClassData(typeof(PascalCaseData))]
        public async Task QueryIntegerContains(string serializer)
        {
            await SetupAsync(serializer);
            await Arango.Collection.CreateAsync("test", "test", ArangoCollectionType.Document);
            await Arango.Document.CreateManyAsync("test", "test", new List<Entity>
            {
                new() {Value = 1},
                new() {Value = 2},
                new() {Value = 3}
            });

            var select = new List<int> {1, 2};

            var res = await Arango.Query.ExecuteAsync<Entity>("test",
                $"FOR e IN test FILTER e.Value IN {select} RETURN e");

            Assert.Equal(2, res.Count);
        }

        [Theory]
        [ClassData(typeof(PascalCaseData))]
        public async Task CollectionNameParameter(string serializer)
        {
            await SetupAsync(serializer);
            await Arango.Collection.CreateAsync("test", "test", ArangoCollectionType.Document);
            await Arango.Document.CreateManyAsync("test", "test", new List<Entity>
            {
                new() {Value = 1},
                new() {Value = 2},
                new() {Value = 3}
            });

            var collection = "test";
            var select = 1;

            var res = await Arango.Query.ExecuteAsync<Entity>("test",
                $"FOR e IN {collection:@} FILTER e.Value == {select} RETURN e");

            Assert.Single(res);
        }

        [Theory]
        [ClassData(typeof(PascalCaseData))]
        public async Task InlineFormattable(string serializer)
        {
            await SetupAsync(serializer);
            await Arango.Collection.CreateAsync("test", "test", ArangoCollectionType.Document);
            await Arango.Document.CreateManyAsync("test", "test", new List<Entity>
            {
                new() {Value = 1},
                new() {Value = 2},
                new() {Value = 3}
            });

            var collection = "test";
            var select = 1;

            FormattableString @for = $"FOR e IN {collection:@}";
            FormattableString filter = $"FILTER e.Value == {select}";
            FormattableString @return = $"RETURN e";

            var res = await Arango.Query.ExecuteAsync<Entity>("test", $"{@for} {filter} {@return}");

            Assert.Single(res);
        }

        [Theory]
        [ClassData(typeof(PascalCaseData))]
        public async Task Batch(string serializer)
        {
            await SetupAsync(serializer);
            await Arango.Collection.CreateAsync("test", "test", ArangoCollectionType.Document);

            await Arango.Document.CreateManyAsync("test", "test",
                Enumerable.Range(1, 100000)
                    .Select(x => new Entity {Value = x}));

            var res = await Arango.Query.ExecuteAsync<string>("test",
                $"FOR e IN test RETURN e._id");

            Assert.Equal(100000, res.Count);
        }

        [Theory]
        [ClassData(typeof(PascalCaseData))]
        public async Task BatchStream(string serializer)
        {
            await SetupAsync(serializer);
            await Arango.Collection.CreateAsync("test", "test", ArangoCollectionType.Document);

            await Arango.Document.CreateManyAsync("test", "test",
                Enumerable.Range(1, 100000)
                    .Select(x => new Entity {Value = x}));

            var i = 0;

            await foreach (var x in Arango.Query.ExecuteStreamAsync<string>("test",
                $"FOR e IN test RETURN e._id", batchSize: 1000))
                ++i;

            Assert.Equal(100000, i);
        }

        [Theory]
        [ClassData(typeof(PascalCaseData))]
        public async Task Explain(string serializer)
        {
            await SetupAsync(serializer);
            await Arango.Collection.CreateAsync("test", "test", ArangoCollectionType.Document);
            await Arango.Document.CreateManyAsync("test", "test", new List<Entity>
            {
                new() {Value = 1},
                new() {Value = 2},
                new() {Value = 3}
            });

            var select = new List<int> {1, 2};

            var res = await Arango.Query.ExplainAsync("test",
                $"FOR e IN test FILTER e.Value IN {select} RETURN e");

            _output.WriteLine(JsonConvert.SerializeObject(res, Formatting.Indented));
        }

        [Theory]
        [ClassData(typeof(PascalCaseData))]
        public async Task Parse(string serializer)
        {
            await SetupAsync(serializer);
            await Arango.Collection.CreateAsync("test", "test", ArangoCollectionType.Document);

            var res = await Arango.Query.ParseAsync("test",
                "FOR e IN test FILTER e.Value IN [1,2,3] RETURN e");

            _output.WriteLine(JsonConvert.SerializeObject(res, Formatting.Indented));
        }
    }
}