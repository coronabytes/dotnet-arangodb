using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Core.Arango.Protocol;
using Core.Arango.Tests.Core;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Core.Arango.Tests
{
    public class TransactionTest : TestBase
    {
        public TransactionTest()
        {
            InitializeAsync().ConfigureAwait(false).GetAwaiter().GetResult();
        }

        [Theory]
        [ClassData(typeof(PascalCaseData))]
        public async Task StreamTransaction(string serializer)
        {
            await SetupAsync(serializer);
            await Arango.Collection.CreateAsync("test", "test", ArangoCollectionType.Document);

            var t1 = await Arango.Transaction.BeginAsync("test", new ArangoTransaction
            {
                Collections = new ArangoTransactionScope
                {
                    Write = new List<string> {"test"}
                }
            });

            await Arango.Document.CreateManyAsync(t1, "test", new List<Entity>
            {
                new() {Value = 1},
                new() {Value = 2},
                new() {Value = 3}
            });

            await Arango.Transaction.CommitAsync(t1);

            Assert.Equal(3, (await Arango.Query.FindAsync<Entity>("test", "test", $"true")).Count);

            var t2 = await Arango.Transaction.BeginAsync("test", new ArangoTransaction
            {
                Collections = new ArangoTransactionScope
                {
                    Write = new List<string> {"test"}
                }
            });

            await Arango.Document.CreateManyAsync(t2, "test", new List<Entity>
            {
                new() {Value = 1},
                new() {Value = 2},
                new() {Value = 3}
            });

            await Arango.Transaction.AbortAsync(t2);

            Assert.Equal(3, (await Arango.Query.FindAsync<Entity>("test", "test", $"true")).Count);
        }


        [Theory]
        [ClassData(typeof(PascalCaseData))]
        public async Task StreamTransactionQuery(string serializer)
        {
            var documents = new List<Entity>
            {
                new() {Value = 1},
                new() {Value = 2},
                new() {Value = 3}
            };

            await SetupAsync(serializer);
            await Arango.Collection.CreateAsync("test", "test", ArangoCollectionType.Document);

            var t1 = await Arango.Transaction.BeginAsync("test", new ArangoTransaction
            {
                Collections = new ArangoTransactionScope
                {
                    Write = new List<string> {"test"}
                }
            });

            foreach (var document in documents)
                await Arango.Query.ExecuteAsync<JObject>(t1, $"INSERT {document} INTO {"test":@}");

            Assert.Equal(3, (await Arango.Query.FindAsync<Entity>(t1, "test", $"true")).Count);

            await Arango.Transaction.CommitAsync(t1);

            Assert.Equal(3, (await Arango.Query.FindAsync<Entity>("test", "test", $"true")).Count);

            var t2 = await Arango.Transaction.BeginAsync("test", new ArangoTransaction
            {
                Collections = new ArangoTransactionScope
                {
                    Write = new List<string> {"test"}
                }
            });

            foreach (var document in documents)
                await Arango.Query.ExecuteAsync<JObject>(t2, $"INSERT {document} INTO {"test":@}");

            Assert.Equal(6, (await Arango.Query.FindAsync<Entity>(t2, "test", $"true")).Count);
            Assert.Equal(3, (await Arango.Query.FindAsync<Entity>("test", "test", $"true")).Count);

            await Arango.Transaction.AbortAsync(t2);

            Assert.Equal(3, (await Arango.Query.FindAsync<Entity>("test", "test", $"true")).Count);

            var exception =
                await Assert.ThrowsAsync<ArangoException>(() => Arango.Query.FindAsync<Entity>(t2, "test", $"true"));

            Assert.NotNull(exception.ErrorNumber);
            Assert.NotNull(exception.Code);
            Assert.Equal(ArangoErrorCode.ErrorTransactionAborted, exception.ErrorNumber);
            Assert.Equal(HttpStatusCode.Gone, exception.Code);
        }

        [Theory]
        [ClassData(typeof(PascalCaseData))]
        public async Task StreamTransactionBatchStreamQuery(string serializer)
        {
            async Task AssertStreamQuery(ArangoHandle handle, int expected)
            {
                var i = 0;

                await foreach (var x in Arango.Query.ExecuteStreamAsync<string>(handle,
                    $"FOR e IN test RETURN e._id", batchSize: 1000))
                    ++i;

                Assert.Equal(expected, i);
            }

            await SetupAsync(serializer);
            await Arango.Collection.CreateAsync("test", "test", ArangoCollectionType.Document);

            var t1 = await Arango.Transaction.BeginAsync("test", new ArangoTransaction
            {
                Collections = new ArangoTransactionScope
                {
                    Write = new List<string> {"test"}
                }
            });

            await Arango.Document.CreateManyAsync(t1, "test",
                Enumerable.Range(1, 100000)
                    .Select(x => new Entity {Value = x}));

            await AssertStreamQuery(t1, 100000);

            await Arango.Transaction.CommitAsync(t1);

            await AssertStreamQuery("test", 100000);

            var t2 = await Arango.Transaction.BeginAsync("test", new ArangoTransaction
            {
                Collections = new ArangoTransactionScope
                {
                    Write = new List<string> {"test"}
                }
            });

            await Arango.Document.CreateManyAsync(t2, "test",
                Enumerable.Range(1, 100000)
                    .Select(x => new Entity {Value = x}));

            await AssertStreamQuery(t2, 200000);
            await AssertStreamQuery("test", 100000);

            await Arango.Transaction.AbortAsync(t2);

            await AssertStreamQuery("test", 100000);

            var exception = await Assert.ThrowsAsync<ArangoException>(() => AssertStreamQuery(t2, 100000));

            Assert.NotNull(exception.ErrorNumber);
            Assert.NotNull(exception.Code);
            Assert.Equal(ArangoErrorCode.ErrorTransactionAborted, exception.ErrorNumber);
            Assert.Equal(HttpStatusCode.Gone, exception.Code);
        }
    }
}