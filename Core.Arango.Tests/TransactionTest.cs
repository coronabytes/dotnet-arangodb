using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Arango.Protocol;
using Core.Arango.Tests.Core;
using Xunit;

namespace Core.Arango.Tests
{
    public class TransactionTest : TestBase
    {
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
                new Entity {Value = 1},
                new Entity {Value = 2},
                new Entity {Value = 3}
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
                new Entity {Value = 1},
                new Entity {Value = 2},
                new Entity {Value = 3}
            });

            await Arango.Transaction.AbortAsync(t2);

            Assert.Equal(3, (await Arango.Query.FindAsync<Entity>("test", "test", $"true")).Count);
        }
    }
}