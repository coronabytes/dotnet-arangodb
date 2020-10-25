using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Core.Arango.Tests
{
    public class DependencyInjectionTest
    {
        private const string SERVER = "http://localhost:8529";
        private const string REALM = "TEST0";
        private const string USER = "UserName";

        [Fact]
        public void AddArangoConfigurationCallback()
        {
            var collection = new ServiceCollection();

            Assert.Same(collection,
                collection.AddArango((sp, config) =>
                {
                    config.Server = SERVER;
                    config.Realm = REALM;
                    config.User = USER;
                }));

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
        public void AddArangoConnectionStringCallback()
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
        public void AddArangoConnectionString()
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