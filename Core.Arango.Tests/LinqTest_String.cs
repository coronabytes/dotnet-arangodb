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

namespace Core.Arango.Tests
{
    public class LinqTest_String : TestBase
    {
        private const string D = "test";
        private readonly ITestOutputHelper _output;
        public LinqTest_String(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task StringConcat()
        {
            var project1 = "Project";
            var project2 = " A";

            var p = await Arango.Query<Project>("test").Where(x => x.Name == string.Concat(project1, project2)).FirstOrDefaultAsync();
            var q = Arango.Query<Project>("test").Where(x => x.Name.Concat(" 10") == "Project A 10");
            Assert.Equal("Project A", p.Name);
            _output.WriteLine(q.ToAql().aql);
        }

        [Fact]
        public async Task StringContains()
        {
            var p = await Arango.Query<Project>("test").Where(x => x.Name.Contains("A")).FirstOrDefaultAsync();
            Assert.Equal("Project A", p.Name);
        }

        [Fact]
        public async Task StringTrim()
        {
            var project1 = " Project A ";
            var project2 = "-||Project A||-";

            char[] charsToDelete = { '|', '-' };

            var p1 = await Arango.Query<Project>("test").Where(x => project1.Trim() == "Project A").FirstOrDefaultAsync();
            var p2 = await Arango.Query<Project>("test").Where(x => project2.Trim(charsToDelete) == "Project A").FirstOrDefaultAsync();

            Assert.Equal("Project A", p1.Name);
            Assert.Equal("Project A", p2.Name);
        }

        [Fact]
        public async Task StringTrimStart()
        {
            var project1 = " Project A";
            var project2 = "-||Project A";

            char[] charsToDelete = { '|', '-' };

            var p1 = await Arango.Query<Project>("test").Where(x => project1.TrimStart() == "Project A").FirstOrDefaultAsync();
            var p2 = await Arango.Query<Project>("test").Where(x => project2.TrimStart(charsToDelete) == "Project A").FirstOrDefaultAsync();

            Assert.Equal("Project A", p1.Name);
            Assert.Equal("Project A", p2.Name);
        }

        [Fact]
        public async Task StringTrimEnd()
        {
            var project1 = "Project A ";
            var project2 = "Project A||-";

            char[] charsToDelete = { '|', '-' };

            var p1 = await Arango.Query<Project>("test").Where(x => project1.TrimEnd() == "Project A").FirstOrDefaultAsync();
            var p2 = await Arango.Query<Project>("test").Where(x => project2.TrimEnd(charsToDelete) == "Project A").FirstOrDefaultAsync();

            Assert.Equal("Project A", p1.Name);
            Assert.Equal("Project A", p2.Name);
        }

        //TODO: Handle cases when properties can be translated as functions in AQL
        [Fact]
        public async Task StringLen()
        {
            var p = await Arango.Query<Project>("test").Where(x => x.Name.Length == "Project A".Length).FirstOrDefaultAsync();
            //var q = Arango.Query<Project>("test").Where(x => x.Name.Length == "Project A".Length); //Doesn't work at the moment
            var q = Arango.Query<Project>("test").Where(x => x.Name.Count() == "Project A".Count()); //Doesn't work at the moment
            Assert.Equal("Project A", p.Name);
            //_output.WriteLine(q.ToAql().aql);
            //_output.WriteLine((await q.FirstOrDefaultAsync()).Name);
        }

        [Fact]
        //TODO: Here we would use the CONTAINS() AQL method using the optional bool parameter. Need to check how to do that.
        public async Task StringIndexOf()
        {
            //var p = await Arango.Query<Project>("test").Where(x => x.Name.IndexOf("A") == "Project A".IndexOf("A")).FirstOrDefaultAsync();
            //var q = Arango.Query<Project>("test").Where(x => x.Name.IndexOf("A") == "Project A".IndexOf("A"));
            //Assert.Equal("Project A", p.Name);
            //_output.WriteLine(q.ToAql().aql);
        }

        //TODO: Check how to handle optional parameters
        [Fact]
        public async Task StringSplit()
        {
            //var p = await Arango.Query<Project>("test").Where(x => x.Name.Split(' ').First() == "A").FirstOrDefaultAsync(); 
            //Assert.Equal("Project A", p.Name);
            //_output.WriteLine(q.ToAql().aql);
        }

        [Fact]
        public async Task StringReplace()
        {
            var p = await Arango.Query<Project>("test").Where(x => x.Name.Replace('A', 'C') == "Project C").FirstOrDefaultAsync();
            Assert.Equal("Project A", p.Name);
        }

        [Fact]
        public async Task StringSubstring()
        {
            var p1 = await Arango.Query<Project>("test").Where(x => x.Name.Substring(8) == "A").FirstOrDefaultAsync();
            var p2 = await Arango.Query<Project>("test").Where(x => x.Name.Substring(1, 8) == "roject A").FirstOrDefaultAsync();
            Assert.Equal("Project A", p1.Name);
            Assert.Equal("Project A", p2.Name);
        }

        [Fact]
        public async Task StringToLower()
        {
            var p = await Arango.Query<Project>("test").Where(x => x.Name.ToLower() == "project a").FirstOrDefaultAsync();
            Assert.Equal("Project A", p.Name);
        }

        [Fact]
        public async Task StringToUpper()
        {
            var p = await Arango.Query<Project>("test").Where(x => x.Name.ToUpper() == "PROJECT A").FirstOrDefaultAsync();
            Assert.Equal("Project A", p.Name);
        }

        [Fact]
        public async Task Temp()
        {
            String[] keys = { "PA", "PB" };

            var arrKeys = keys.ToArray();

            //var p = Arango.Query<Project>("test");
            //var q = p.Where(x => Aql.Position(keys, x.Name));
            //var q = p.Where(x => keys.Contains(x.Key));
            //var qq = q.ToAql().aql;

            //var p1 = Arango.Query<Project>("test");
            //var q1 = p1.Where(x => x.Name.ToUpper() == "Project A");
            //var qq1 = q1.ToAql().aql;

            //var p = Arango.Query<Project>("test").Where(x => x.Name.Equals("Project A"));
            //var q = p.ToAql();

            //var p2 = Arango.Query<Project>("test").Where(x => x.Name == "abc");
            //var q2 = p2.ToAql();

            //var a = await p.FirstOrDefaultAsync();
            //_output.WriteLine(a.Name);
            //_output.WriteLine(q.aql);
            //_output.WriteLine(q2.aql);

            //Math functions seem to be working
            var q = Arango.Query<Project>("test")
                //.Where(x => Math.Floor(x.Budget) == 10)
                .Where(x => Math.Abs(x.Budget) == 10)
                ;
            _output.WriteLine(q.ToAql().aql);

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
                    ClientKey = "CA",
                    Budget = 10.1
                },
                new ()
                {
                    Key = "PB",
                    Name = "Project B",
                    ClientKey = "CB",
                    Budget = 11.2
                }
            });
        }
    }
}
