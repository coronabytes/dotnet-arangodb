using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Core.Arango.Linq.Tests
{
    public class Project
    {
        public Guid Key { get; set; }
        public string Name { get; set; }
    }

    public class UnitTest1 : IAsyncLifetime
    {
        protected readonly ArangoContext Arango =
            new ArangoContext($"Server=http://localhost:8529;Realm=CI-{Guid.NewGuid():D};User=root;Password=;");

        [Fact]
        public void Test1()
        {
            var test = Arango.AsQueryable<Project>("test").SingleOrDefault(x => x.Name == "A");
        }

        [Fact]
        public void Test2()
        {
            var test = Arango.AsQueryable<Project>("test").Where( x=>x.Name == "A").Select(x => x.Name).ToList();
        }

        public Task InitializeAsync()
        {
            return Task.CompletedTask;
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
