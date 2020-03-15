using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Arango.Protocol;
using Core.Arango.Tests.Core;
using Xunit;

namespace Core.Arango.Tests
{
    public class TransactionTest : TestBase
    {
        [Fact]
        public async Task StreamTransaction()
        {
            await Arango.CreateCollectionAsync("test", "test", ArangoCollectionType.Document);

            var t1 = await Arango.BeginTransactionAsync("test", new ArangoTransaction
            {
                Collections = new ArangoTransactionScope
                {
                    Write = new List<string> {"test"}
                }
            });

            await Arango.CreateDocumentsAsync(t1, "test", new List<Entity>
            {
                new Entity {Value = 1},
                new Entity {Value = 2},
                new Entity {Value = 3}
            });

            await Arango.CommitTransactionAsync(t1);

            Assert.Equal(3, (await Arango.FindAsync<Entity>("test", "test", $"true")).Count);

            var t2 = await Arango.BeginTransactionAsync("test", new ArangoTransaction
            {
                Collections = new ArangoTransactionScope
                {
                    Write = new List<string> {"test"}
                }
            });

            await Arango.CreateDocumentsAsync(t2, "test", new List<Entity>
            {
                new Entity {Value = 1},
                new Entity {Value = 2},
                new Entity {Value = 3}
            });

            await Arango.AbortTransactionAsync(t2);

            Assert.Equal(3, (await Arango.FindAsync<Entity>("test", "test", $"true")).Count);
        }
    }
}