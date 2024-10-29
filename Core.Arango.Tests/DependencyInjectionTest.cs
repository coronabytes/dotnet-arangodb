using Core.Arango.Tests.Core;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Core.Arango.Tests
{
    public class DependencyInjectionTest : TestBase
    {
        public DependencyInjectionTest()
        {
            InitializeAsync().ConfigureAwait(false).GetAwaiter().GetResult();
        }

        [Fact]
        public void AddArangoConfigurationCallback()
        {
            var collection = new ServiceCollection();

            collection.AddArango((sp, config) =>
            {
                config.ConnectionString = UniqueTestRealm();
                config.BatchSize = 1337;
            });

            var serviceProvider = collection.BuildServiceProvider();
            Arango = serviceProvider.GetRequiredService<IArangoContext>();

            Arango.GetVersionAsync();

            Assert.Equal(1337, Arango.Configuration.BatchSize);
        }

        [Fact]
        public void AddArangoConnectionStringCallback()
        {
            var collection = new ServiceCollection();

            collection.AddArango(sp => UniqueTestRealm());

            var serviceProvider = collection.BuildServiceProvider();
            Arango = serviceProvider.GetRequiredService<IArangoContext>();

            Arango.GetVersionAsync();
        }

        [Fact]
        public void AddArangoConnectionString()
        {
            var collection = new ServiceCollection();

            collection.AddArango(UniqueTestRealm());

            var serviceProvider = collection.BuildServiceProvider();
            Arango = serviceProvider.GetRequiredService<IArangoContext>();

            Arango.GetVersionAsync();
        }
    }
}