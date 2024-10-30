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
using System.Collections.Generic;
using System.IO;
using DotNet.Testcontainers.Builders;

namespace Core.Arango.Tests.Core
{
    public abstract class TestBase : IAsyncLifetime
    {
        private const string ARANGO_LICENSE_KEY_ENVAR = "ARANGO_LICENSE_KEY";
        private const string ARANGODB_VERSION_ENVAR = "ARANGODB_VERSION";
        private const string DefaultImage = "arangodb:latest";
        private const string DefaultImagePassword = "password";
        private const string DefaultImageUser = "root";

        public IArangoContext Arango { get; protected set; }
        public IEnumerable<ArangoDbContainer> Containers { get; protected set; }

        public virtual async Task InitializeAsync()
        {
            var environment = new Dictionary<string, string>();

            if (string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable(ARANGO_LICENSE_KEY_ENVAR)))
            {
                environment.Add(ARANGO_LICENSE_KEY_ENVAR, Environment.GetEnvironmentVariable(ARANGO_LICENSE_KEY_ENVAR));
            }

            var version = string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable(ARANGODB_VERSION_ENVAR))
                ? DefaultImage
                : Environment.GetEnvironmentVariable(ARANGODB_VERSION_ENVAR);

            Containers = [SetupSingleServer(environment, version)];
            // Containers = await SetupClusterServer(environment, version);

            foreach (var container in Containers)
            {
                await container.StartAsync()
                    .ConfigureAwait(false);
            }
        }

        private static ArangoDbContainer SetupSingleServer(Dictionary<string, string> environment, string version) 
            => new ArangoDbBuilder()
                .WithImage(version)
                .WithEnvironment(environment)
                .WithPassword(DefaultImagePassword)
                .Build();

        private static async Task<List<ArangoDbContainer>> SetupClusterServer(Dictionary<string, string> environment, string version)
        {
            // This is not yet tested. But is a largely compatible replication of the start_db_cluster script.
            // 2024-10-29 20:11:32 Error while processing command-line options for arangod:
            // 2024 - 10 - 29 20:11:32   unknown option '--cluster.start-dbserver false'
            // I suspect that's because Cluster is only available for Enterprise?
            var arangoDBNetwork = new NetworkBuilder()
              .WithName($"ArangoDB-{Guid.NewGuid():D}")
              .Build();

            var jwt = Path.GetTempFileName();
            File.Move(jwt, jwt + ".jwt");
            jwt += ".jwt";
            File.AppendAllText(jwt, "Averysecretword");

            var leadAgent = $"leadagent-{Guid.NewGuid():N}";
            var extraAgents = Enumerable.Range(0, 2);
            var dbs = Enumerable.Range(0, 2);
            var coordinators = Enumerable.Range(0, 2);

            var containers = new List<ArangoDbContainer>();

            var DefaultContainerConfiguration = new ArangoDbBuilder()
                .WithImage(version)
                .WithResourceMapping(jwt, "/jwtSecret")
                .WithEnvironment(environment)
                .WithPassword(DefaultImagePassword)
                .WithNetwork(arangoDBNetwork);

            containers.Add(DefaultContainerConfiguration
                .WithName(leadAgent)
                .WithCommand(
                    "--cluster.start-dbserver", "false",
                    "--cluster.start-coordinator", "false",
                    "--auth.jwt-secret", "/jwtSecret")
                .Build());

            foreach (var _ in extraAgents)
            {
                containers.Add(DefaultContainerConfiguration
                    .WithName($"{extraAgents}-{Guid.NewGuid():N}")
                    .WithCommand(
                        "--cluster.start-dbserver", "false",
                        "--cluster.start-coordinator", "false",
                        "--auth.jwt-secret", "/jwtSecret",
                        "--starter.join", leadAgent)
                    .Build());
            }

            foreach (var _ in dbs)
            {
                containers.Add(DefaultContainerConfiguration
                    .WithName($"{dbs}-{Guid.NewGuid():N}")
                    .WithCommand(
                        "--cluster.start-dbserver", "true",
                        "--cluster.start-coordinator", "false",
                        "--auth.jwt-secret", "/jwtSecret",
                        "--starter.join", leadAgent)
                    .Build());
            }

            foreach (var _ in coordinators)
            {
                containers.Add(DefaultContainerConfiguration
                    .WithName($"{coordinators}-{Guid.NewGuid():N}")
                    .WithCommand(
                        "--cluster.start-dbserver", "false",
                        "--cluster.start-coordinator", "true",
                        "--auth.jwt-secret", "/jwtSecret",
                        "--starter.join", leadAgent)
                    .Build());
            }

            await arangoDBNetwork.CreateAsync()
                .ConfigureAwait(false);

            return containers;
        }

        public async Task DisposeAsync()
        {
            try
            {
                foreach (var container in Containers)
                {
                    await container.DisposeAsync().AsTask();
                }
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
                if (databaseCreateSuccessful == false)
                {
                    throw new Exception("Database creation failed");
                }
            }
        }

        protected string UniqueTestRealm()
            // Last to get the Coordinators for clusters, or the only existing one for a single server.
            => $"Server={Containers.Last().GetTransportAddress()};User={DefaultImageUser};Realm=CI-{Guid.NewGuid():D};Password={DefaultImagePassword};";

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