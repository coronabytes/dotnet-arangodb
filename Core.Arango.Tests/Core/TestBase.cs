using System;
using System.Threading.Tasks;
using Core.Arango.Serialization;
using Xunit;

namespace Core.Arango.Tests.Core
{
    public abstract class TestBase : IAsyncLifetime
    {
        public IArangoContext Arango { get; protected set; }
        public virtual Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        public async Task SetupAsync(IArangoContext context)
        {
            Arango = context;
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