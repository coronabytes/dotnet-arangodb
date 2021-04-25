using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Core.Arango.Protocol;
using Core.Arango.Linq;
using Core.Arango.Tests.Core;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace Core.Arango.Tests
{
    public class LinqTest : TestBase
    {
        private const string D = "test";
        private readonly ITestOutputHelper _output;

        public LinqTest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task Test1()
        {
            var q = Arango.Query<Project>("test")
                .Where(x => x.Name == "Project A")
                .Select(x => new
                {
                    x.Key,
                    x.Name,
                    ClientName = Aql.Document<Client>("Client", x.ClientKey).Name
                });

            _output.WriteLine(q.ToAql().aql);
            _output.WriteLine("");
            _output.WriteLine(JsonConvert.SerializeObject(await q.ToListAsync(), Formatting.Indented));
        }

        [Fact]
        public async Task Test2()
        {
            var q =
                from p in Arango.Query<Project>("test")
                let clients = (from c in Arango.Query<Client>() select c.Key)
                select new {p.Name, Clients = Aql.As<string[]>(clients) };

            _output.WriteLine(q.ToAql().aql);
            _output.WriteLine("");
            _output.WriteLine(JsonConvert.SerializeObject(await q.ToListAsync(), Formatting.Indented));
        }

        public override async Task InitializeAsync()
        {
            Arango = new ArangoContext(UniqueTestRealm());
            await Arango.Database.CreateAsync(D);
            await Arango.Collection.CreateAsync(D, nameof(Client), ArangoCollectionType.Document);
            await Arango.Collection.CreateAsync(D, nameof(Project), ArangoCollectionType.Document);
            await Arango.Collection.CreateAsync(D, nameof(Activity), ArangoCollectionType.Document);

            await Arango.Document.CreateManyAsync(D, nameof(Client), new List<Client>
            {
                new()
                {
                    Key = "CA",
                    Name = "Client A"
                },
                new()
                {
                    Key = "CB",
                    Name = "Client B"
                }
            });

            await Arango.Document.CreateManyAsync(D, nameof(Project), new List<Project>
            {
                new ()
                {
                    Key = "PA",
                    Name = "Project A",
                    ClientKey = "CA"
                },
                new ()
                {
                    Key = "PB",
                    Name = "Project B",
                    ClientKey = "CB"
                }
            });

        }
    }
}