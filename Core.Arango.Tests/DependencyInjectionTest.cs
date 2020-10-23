using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Core.Arango.Tests
{
    public class DependencyInjectionTest
    {
        private const string SERVER = "http://127.0.0.99:8529";
        private const string REALM = "TEST0";
        private const string USER = "UserName";

        [Fact]
        public void DIServiceCollectionWithConfiguratorTest()
        {
            var collection = new ServiceCollection();

            Assert.Same(collection,
                collection.AddArango(sp => $"Server={SERVER};Realm={REALM};User={USER};Password=;"));

            var serviceProvider = collection.BuildServiceProvider();
            var arango = serviceProvider.GetRequiredService<IArangoContext>();

            using (var scope = serviceProvider.CreateScope())
            {
                Assert.Same(arango, scope.ServiceProvider.GetRequiredService<IArangoContext>());
            }

            Assert.Same(arango, serviceProvider.GetRequiredService<IArangoContext>());

            Assert.Equal(SERVER, arango.Configuration.Server);
        }

        [Fact]
        public void DIServiceCollectionWithConnectionStringTest()
        {
            var collection = new ServiceCollection();
            Assert.Same(collection, collection.AddArango($"Server={SERVER};Realm={REALM};User={USER};Password=;"));

            var serviceProvider = collection.BuildServiceProvider();
            var arango = serviceProvider.GetRequiredService<IArangoContext>();

            using (var scope = serviceProvider.CreateScope())
            {
                Assert.Same(arango, scope.ServiceProvider.GetRequiredService<IArangoContext>());
            }

            Assert.Same(arango, serviceProvider.GetRequiredService<IArangoContext>());

            Assert.Equal(SERVER, arango.Configuration.Server);
        }
    }
}