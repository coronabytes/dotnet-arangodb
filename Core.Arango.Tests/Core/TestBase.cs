using System;
using System.Threading.Tasks;
using Xunit;

namespace Core.Arango.Tests.Core
{
    public abstract class TestBase : IAsyncLifetime
    {
        protected readonly ArangoContext Arango =
            new ArangoContext($"Server=http://localhost:8529;Realm=CI-{Guid.NewGuid():D};User=root;Password=;");

        public async Task InitializeAsync()
        {
            await Arango.CreateDatabaseAsync("test");
        }

        public async Task DisposeAsync()
        {
            try
            {
                foreach (var db in await Arango.ListDatabasesAsync())
                    await Arango.DropDatabaseAsync(db);
            }
            catch (Exception e)
            {
                //
            }
        }
    }
}