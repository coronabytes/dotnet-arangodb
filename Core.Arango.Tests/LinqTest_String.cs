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
        public async ValueTask StringConcat()
        {
            var q = Arango.Query<Project>("test").Where(x => String.Concat(x.Name, " 10") == "Project A 10");
            var p = await q.FirstOrDefaultAsync();
            
            Assert.Equal("Project A", p.Name);
            //_output.WriteLine(q.ToAql().aql);
        }

        [Fact]
        public async ValueTask StringContains()
        {
            var q = Arango.Query<Project>("test").Where(x => x.Name.Contains("A"));
            var p = await q.FirstOrDefaultAsync();

            Assert.Equal("Project A", p.Name);
            //_output.WriteLine(q.ToAql().aql);
        }

        /*//Not working at the moment. Check the comment on the second Assert.
        [Fact]
        public async ValueTask StringTrim()
        {
            await Arango.Document.CreateManyAsync(D, nameof(Project), new List<Project>
            {
                new ()
                {
                    Key = "PC",
                    Name = " Project C ",
                    ClientKey = "CA"
                },
                new ()
                {
                    Key = "PD",
                    Name = "-||Project D||-",
                    ClientKey = "CB"
                }
            });

            char[] charsToDelete = { '|', '-' };

            var q1 = Arango.Query<Project>("test").Where(x => x.Name.Trim() == "Project C");
            var q2 = Arango.Query<Project>("test").Where(x => x.Name.Trim(charsToDelete) == "Project D");

            var p1 = await q1.FirstOrDefaultAsync();
            var p2 = await q2.FirstOrDefaultAsync();

            Assert.Equal(" Project C ", p1.Name);
            Assert.Equal("-||Project D||-", p2.Name); //This isn't working right now, because for some reason, AQL LTRIM and RTRIM accepts arrays of chars for deletion, but TRIM doesn't
            //So it's an issue with AQL itself. https://github.com/arangodb/arangodb/issues/15500
            //It could be fixed by doing some logic inside the ArangoExpressionTreeVistor, but as I said, it's an issue with AQL, so I'll wait your call on this.

            //_output.WriteLine(q1.ToAql().aql);
            //_output.WriteLine(q2.ToAql().aql);
        }*/

        [Fact]
        public async ValueTask StringTrimStart()
        {
            await Arango.Document.CreateManyAsync(D, nameof(Project), new List<Project>
            {
                new ()
                {
                    Key = "PC",
                    Name = " Project C",
                    ClientKey = "CA"
                },
                new ()
                {
                    Key = "PD",
                    Name = "-||Project D",
                    ClientKey = "CB"
                }
            });

            char[] charsToDelete = { '|', '-' };

            var q1 = Arango.Query<Project>("test").Where(x => x.Name.TrimStart() == "Project C");
            var q2 = Arango.Query<Project>("test").Where(x => x.Name.TrimStart(charsToDelete) == "Project D");

            var p1 = await q1.FirstOrDefaultAsync();
            var p2 = await q2.FirstOrDefaultAsync();

            Assert.Equal(" Project C", p1.Name);
            Assert.Equal("-||Project D", p2.Name);

            //_output.WriteLine(q1.ToAql().aql);
            //_output.WriteLine(q2.ToAql().aql);
        }

        [Fact]
        public async ValueTask StringTrimEnd()
        {
            await Arango.Document.CreateManyAsync(D, nameof(Project), new List<Project>
            {
                new ()
                {
                    Key = "PC",
                    Name = "Project C ",
                    ClientKey = "CA"
                },
                new ()
                {
                    Key = "PD",
                    Name = "Project D||-",
                    ClientKey = "CB"
                }
            });

            char[] charsToDelete = { '|', '-' };

            var q1 = Arango.Query<Project>("test").Where(x => x.Name.TrimEnd() == "Project C");
            var q2 = Arango.Query<Project>("test").Where(x => x.Name.TrimEnd(charsToDelete) == "Project D");

            var p1 = await q1.FirstOrDefaultAsync();
            var p2 = await q2.FirstOrDefaultAsync();

            Assert.Equal("Project C ", p1.Name);
            Assert.Equal("Project D||-", p2.Name);

            //_output.WriteLine(q1.ToAql().aql);
            //_output.WriteLine(q2.ToAql().aql);
        }

        [Fact]
        public async ValueTask StringLen()
        {
            var q = Arango.Query<Project>("test").Where(x => x.Name.Length == "Project A".Length);
            var p = await q.FirstOrDefaultAsync();

            Assert.Equal("Project A", p.Name);
            //_output.WriteLine(q.ToAql().aql);
        }

        [Fact]
        public async ValueTask StringIndexOf()
        {
            var q = Arango.Query<Project>("test").Where(x => x.Name.IndexOf("A") == "Project A".IndexOf("A"));
            var p = await q.FirstOrDefaultAsync();
            
            Assert.Equal("Project A", p.Name);
            //_output.WriteLine(q.ToAql().aql);
        }

        [Fact]
        public async ValueTask StringSplit()
        {
            char[] splitChars = { ' ' };

            var q = Arango.Query<Project>("test").Where(x => x.Name.Split(splitChars) == ("Project A").Split(splitChars));
            var p = await q.FirstOrDefaultAsync();

            Assert.Equal("Project A", p.Name);
            //_output.WriteLine(q.ToAql().aql);
        }

        [Fact]
        public async ValueTask StringReplace()
        {
            var q = Arango.Query<Project>("test").Where(x => x.Name.Replace('A', 'C') == "Project C");
            var p = await q.FirstOrDefaultAsync();

            Assert.Equal("Project A", p.Name);
            //_output.WriteLine(q.ToAql().aql);
        }

        [Fact]
        public async ValueTask StringSubstring()
        {
            var q1 = Arango.Query<Project>("test").Where(x => x.Name.Substring(8) == "A");
            var q2 = Arango.Query<Project>("test").Where(x => x.Name.Substring(1, 8) == "roject A");

            var p1 = await q1.FirstOrDefaultAsync();
            var p2 = await q2.FirstOrDefaultAsync();

            Assert.Equal("Project A", p1.Name);
            Assert.Equal("Project A", p2.Name);

            _output.WriteLine(q1.ToAql().aql);
            _output.WriteLine(q2.ToAql().aql);
        }

        [Fact]
        public async ValueTask StringToLower()
        {
            var q = Arango.Query<Project>("test").Where(x => x.Name.ToLower() == "project a");
            var p = await q.FirstOrDefaultAsync();

            Assert.Equal("Project A", p.Name);
            _output.WriteLine(q.ToAql().aql);
        }

        [Fact]
        public async ValueTask StringToUpper()
        {
            var q = Arango.Query<Project>("test").Where(x => x.Name.ToUpper() == "PROJECT A");
            var p = await q.FirstOrDefaultAsync();

            Assert.Equal("Project A", p.Name);
            _output.WriteLine(q.ToAql().aql);
        }

        public override async ValueTask InitializeAsync()
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
