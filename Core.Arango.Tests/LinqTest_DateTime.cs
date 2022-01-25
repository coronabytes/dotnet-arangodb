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
    public class LinqTest_DateTime : TestBase
    {
        private const string D = "test";
        private readonly ITestOutputHelper _output;
        public LinqTest_DateTime(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task GetDateProperty()
        {
            //The same as the Lenght property on the string tests, we need a way to transform properties from objects to methods in AQL.
            var time = new DateTime(2021, 1, 30);
            //var q = Arango.Query<Activity>("test").Where(x => x.Start.Month < time.Month); //This is how the end user should write the LINQ method
            var q = Arango.Query<Activity>("test").Where(x => Aql.DateMonth(x.Start) < time.Month);
            var a = await q.FirstOrDefaultAsync();
            Assert.Null(a);
            _output.WriteLine(q.ToAql().aql);
        }

        [Fact]
        public async Task Add()
        {
            var time = new DateTime(2021, 1, 30);
            var time2 = time.AddYears(1);
            //time = time.ToUniversalTime();
            //var time2 = time.AddYears(1);
            //var q = Arango.Query<Activity>("test").Where(x => x.Start.AddYears(1) == time2);
            var q2 = Arango.Query<Activity>("test").Where(x => x.Start.AddYears(1) == time2);

            var a = await q2.FirstOrDefaultAsync();
            Assert.Equal("AA", a.Key);
            //_output.WriteLine(q.ToAql().aql);
            _output.WriteLine(q2.ToAql().aql);
        }

        public override async Task InitializeAsync()
        {
            Arango = new ArangoContext(UniqueTestRealm());
            await Arango.Database.CreateAsync(D);
            await Arango.Collection.CreateAsync(D, nameof(Activity), ArangoCollectionType.Document);

            await Arango.Document.CreateManyAsync(D, nameof(Activity), new List<Activity>
            {
                new()
                {
                    Key = "AA",
                    Start = new DateTime(2021, 1, 30),
                    End = new DateTime(2022, 2, 10)
                },
                new()
                {
                    Key = "AB",
                    Start = new DateTime(2022, 5, 15),
                    End = new DateTime(2050, 10, 3)
                }
            });
        }
    }
}
