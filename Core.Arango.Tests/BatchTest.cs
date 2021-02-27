using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Core.Arango.Protocol;
using Core.Arango.Tests.Core;
using Xunit;

namespace Core.Arango.Tests
{
    public class BatchTest : TestBase
    {
        [Fact]
        public async Task Batch()
        {
            await SetupAsync("newton-default");
            await Arango.Collection.CreateAsync("test", "test", ArangoCollectionType.Document);

            var trx = await Arango.Transaction.BeginAsync("test", new ArangoTransaction
            {
                Collections = new ArangoTransactionScope
                {
                    Write = new List<string> { "test" }
                }
            });
            var batch = Arango.Batch.Create(trx);

            var r1 = Arango.Document.CreateAsync(batch, "test", new Entity
            {
                Key = "1",
                Name = "a"
            });

            var r2 = Arango.Document.CreateAsync(batch, "test", new Entity
            {
                Key = "1",
                Name = "b"
            });

            await Arango.Batch.ExecuteAsync(batch);
            await Arango.Transaction.AbortAsync(trx);

            await r1;
            //await r2;
            await Assert.ThrowsAsync<ArangoException>(async () => await r2);
        }
    }
}