using System;
using System.Threading.Tasks;
using Xunit;

namespace Core.Arango.Tests.Core
{
    public abstract class TestBase : IAsyncLifetime
    {
        public IArangoContext Arango { get; protected set; }

        protected string UniqueTestRealm()
        {
            var cs = Environment.GetEnvironmentVariable("ARANGODB_CONNECTION");

            if (string.IsNullOrWhiteSpace(cs))
                cs = "Server=http://localhost:8529;Realm=CI-{UUID};User=root;Password=;";

            return cs.Replace("{UUID}", Guid.NewGuid().ToString("D"));
        }

        public virtual async Task InitializeAsync()
        {
            Arango = new ArangoContext(UniqueTestRealm());
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