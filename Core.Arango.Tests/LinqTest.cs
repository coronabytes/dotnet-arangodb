using System;
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
using System.Linq.Expressions;

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
        public async Task SelectDocument()
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
        public async Task FirstOrDefault()
        {
            var p = await Arango.Query<Project>("test")
                .FirstOrDefaultAsync(x=>x.Name == "Project A");

            Assert.Equal("Project A", p.Name);
        }

        [Fact]
        public async Task SingleOrDefault()
        {
            var p = await Arango.Query<Project>("test")
                .SingleOrDefaultAsync(x => x.Name == "Project A");

            Assert.Equal("Project A", p.Name);
        }

        [Fact]
        public async Task SingleException()
        {
            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await Arango.Query<Project>("test")
                    .SingleAsync(x => x.Name == "Nope");
            });
        }

        [Fact]
        public async Task MultiFilter()
        {
            var q = Arango.Query<Project>("test")
                .Where(x => x.Name == "Project A")
                .OrderBy(x => x.Name)
                .Where(x => x.Name == "Project B")
                .OrderByDescending(x => x.Name)
                .Skip(0).Take(1);

            Assert.Equal("FOR `x` IN `Project`  FILTER  (  `x`.`Name`  ==  @P1  )  SORT  `x`.`Name`  ASC   FILTER  (  `x`.`Name`  ==  @P2  )  SORT  `x`.`Name`  DESC    LIMIT  @P3   ,  @P4  RETURN   `x`", q.ToAql().aql.Trim());
        }


        [Fact]
        public async Task FilterOrder()
        {
            var q = Arango.Query<Project>("test")
                .Where(x => (x.Name == "Project A" || x.Name == "Project B") && x.Budget <= 0);

            Assert.Equal("FOR `x` IN `Project`  FILTER  (  (  (  `x`.`Name`  ==  @P1  )  OR  (  `x`.`Name`  ==  @P2  )  )  AND  (  `x`.`Budget`  <=  @P3  )  )  RETURN   `x`", q.ToAql().aql.Trim());
        }

        [Fact]
        public async Task GroupBy()
        {
            var q = Arango.Query<Project>("test")
                .GroupBy(y => y.ParentKey)
                .Select(x => new
                {
                    Parent = x.Key,
                    Max = x.Max(y=>y.Budget)
                });

            _output.WriteLine(q.ToAql().aql);
            _output.WriteLine("");
            //_output.WriteLine(JsonConvert.SerializeObject(await q.ToListAsync(), Formatting.Indented));
        }

        [Fact]
        public async Task MathAbs()
        {
            var q = Arango.Query<Project>("test")
                .Select(x => new
                {
                    x.Key,
                    Budget = Math.Abs(x.Budget)
                });

            _output.WriteLine(q.ToAql().aql);
            _output.WriteLine("");
            //_output.WriteLine(JsonConvert.SerializeObject(await q.ToListAsync(), Formatting.Indented));
        }

        [Fact]
        public async Task Ternary()
        {
            var q = Arango.Query<Project>("test")
                .Select(x => new
                {
                    x.Key,
                    Name = x.Name == "Test" ? "-" : x.Name
                });

            _output.WriteLine(q.ToAql().aql);
            _output.WriteLine("");
            //_output.WriteLine(JsonConvert.SerializeObject(await q.ToListAsync(), Formatting.Indented));
        }

        [Fact]
        public async Task Update()
        {
            var q = Arango.Query<Project>("test")
                .Where(x => x.Name == "Project A")
                .Update(x => new
                {
                    x.Key,
                    Name = Aql.Concat(x.Name, "2")
                }, x=>x.Key);

            _output.WriteLine(q.ToAql().aql);
            _output.WriteLine("");
            _output.WriteLine(JsonConvert.SerializeObject(await q.ToListAsync(), Formatting.Indented));
        }

        [Fact]
        public async Task Remove()
        {
            var q = Arango.Query<Project>("test")
                .Where(x => x.Name == "Project A")
                .Remove().In<Project>().Select(x=>x.Key);

            _output.WriteLine(q.ToAql().aql);
        }

        [Fact]
        public async Task Let()
        {
            var q =
                from p in Arango.Query<Project>("test")
                let clients = (from c in Arango.Query<Client>() select c.Key)
                select new {p.Name, Clients = Aql.As<string[]>(clients) };

            _output.WriteLine(q.ToAql().aql);
            _output.WriteLine("");
            _output.WriteLine(JsonConvert.SerializeObject(await q.ToListAsync(), Formatting.Indented));
        }

        [Fact]
        public async Task ListContains()
        {
            var list = new List<string> { "CB" }.ToArray();

            var q = Arango.Query<Project>("test")
                .Where(x => list.Contains(x.ClientKey));

            //var q = Arango.Query<Project>("test")
            //    .Where(x => Aql.Position(list, x.ClientKey));

            _output.WriteLine(q.ToAql().aql);
            _output.WriteLine("");

            var output = await q.ToListAsync();
            _output.WriteLine(JsonConvert.SerializeObject(output, Formatting.Indented));

            Assert.Single(output);
        }

        [Fact]
        public async Task QueryableContains()
        {
            var q = Arango.Query<Project>("test")
                .Select(x => x.ClientKey)
                .Contains("CB");

            Assert.True(q);
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

        [Fact]
        public async Task Count_SubQuery()
        {
            var q = Arango.Query<Client>("test")
                .Select(c => Arango.Query<Project>().Where(p => p.ClientKey == c.Key).Count());

            _output.WriteLine(q.ToAql().aql);

            var clientWithProjects = await q.ToListAsync();

            Assert.Equal(new List<int> { 1, 1 }, clientWithProjects);
        }

        /*[Fact]
        public async Task Except_SubQuery()
        {
            var list = await Arango.Query<Project>("test")
                .Take(1)
                .ToListAsync();

            var q = Arango.Query<Client>("test")
                .Select(c => Arango
                    .Query<Project>()
                    .Where(p => p.ClientKey == c.Key)
                    .Except(list) // TODO : Order of operations is not working correctly ('length' is being called before 'minus')
                    .Count()
                );

            PrintQuery(q, _output);

            var p = await q.ToListAsync();

            Assert.Equal(new List<int> { 0, 1 }, p);
        }*/

        [Fact]
        public async Task StringContains()
        {
            var q = Arango.Query<Project>("test").Where(x => x.Name.Contains("abc"));
            var aql = q.ToAql().aql.Trim();

            Assert.Equal("FOR `x` IN `Project`  FILTER  CONTAINS(  `x`.`Name`  ,  @P1  )  RETURN   `x`", aql);
        }

        [Fact]
        public async Task StringConcat()
        {
            var q = Arango.Query<Project>("test").Where(x => string.Concat(x.Name, "Suffix") == "TestSuffix");
            var aql = q.ToAql().aql.Trim();

            Assert.Equal("FOR `x` IN `Project`  FILTER  (  CONCAT(  `x`.`Name`  ,  @P1  )  ==  @P2  )  RETURN   `x`", aql);
        }
    }
}