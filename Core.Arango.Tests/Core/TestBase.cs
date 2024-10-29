using System;
using System.Linq;
using System.Threading.Tasks;
using Core.Arango.Linq;
using Core.Arango.Serialization.Json;
using Core.Arango.Serialization.Newtonsoft;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;
using Testcontainers.ArangoDb;

namespace Core.Arango.Tests.Core
{
    public abstract class TestBase : IAsyncLifetime
    {
        private const string Image = "arangodb:latest";
        private const string DefaultImagePassword = "password";
        public const string DefaultImageUser = "root";

        public IArangoContext Arango { get; protected set; }
        public ArangoDbContainer Container { get; protected set; }

        public virtual async Task InitializeAsync()
        {
            Container = new ArangoDbBuilder()
                .WithImage(Image)
                .WithPassword(DefaultImagePassword)
                .Build();

            await Container.StartAsync()
                .ConfigureAwait(false);
        }

        public async Task DisposeAsync()
        {
            try
            {
                await (Container?.DisposeAsync().AsTask() ?? Task.CompletedTask);
            }
            catch { }
        }

        public async Task SetupAsync(string serializer, string createDatabase = "test")
        {
            Arango = new ArangoContext(UniqueTestRealm(), new ArangoConfiguration
            {
                Serializer = serializer switch
                {
                    "newton-default" => new ArangoNewtonsoftSerializer(new ArangoNewtonsoftDefaultContractResolver()),
                    "newton-camel" => new ArangoNewtonsoftSerializer(new ArangoNewtonsoftCamelCaseContractResolver()),
                    "system-default" => new ArangoJsonSerializer(new ArangoJsonDefaultPolicy()),
                    "system-camel" => new ArangoJsonSerializer(new ArangoJsonCamelCasePolicy()),
                    _ => new ArangoNewtonsoftSerializer(new ArangoNewtonsoftDefaultContractResolver())
                }
            });

            if (!string.IsNullOrEmpty(createDatabase))
            {
                var databaseCreateSuccessful = await Arango.Database.CreateAsync("test");
                if(databaseCreateSuccessful == false)
                {
                    throw new Exception("Database creation failed");
                }
            }
        }

        protected string UniqueTestRealm()
            => $"Server={Container.GetTransportAddress()};User={DefaultImageUser};Realm=CI-{Guid.NewGuid():D};Password={DefaultImagePassword};";

        protected void PrintQuery<T>(IQueryable<T> query, ITestOutputHelper output)
        {
            var aql = query.ToAql();
            output.WriteLine("QUERY:");
            output.WriteLine(aql.aql);
            output.WriteLine("VARS:");
            output.WriteLine(JsonConvert.SerializeObject(aql.bindVars));
        }
    }
}