using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Core.Arango.Serialization;
using Core.Arango.Serialization.Json;
using Core.Arango.Serialization.Newtonsoft;
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

        public async Task SetupAsync(string serializer)
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

        protected string UniqueTestRealm()
        {
            var cs = Environment.GetEnvironmentVariable("ARANGODB_CONNECTION");

            if (string.IsNullOrWhiteSpace(cs))
                cs = "Server=http://localhost:8529;Realm=CI-{UUID};User=root;Password=;";

            return cs.Replace("{UUID}", Guid.NewGuid().ToString("D"));
        }
    }
}