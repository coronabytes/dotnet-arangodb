using System;
using System.Threading.Tasks;
using Xunit;

namespace Core.Arango.Tests.Core
{
    public abstract class TestBase : IAsyncLifetime
    {
        protected readonly IArangoContext Arango =
            new ArangoContext($"Server=http://172.28.3.1:8529;Realm=CI-{Guid.NewGuid():D};User=root;Password=test;");

        public async Task InitializeAsync()
        {
            await Arango.Database.CreateAsync("test");
        }

        public async Task DisposeAsync()
        {
            try
            {
                foreach (var db in await Arango.Database.ListAsync())
                    await Arango.Database.DropAsync(db);
            }
            catch
            {
                //
            }
        }
    }
}