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
    public class LinqTest_BasicOperations : TestBase
    {
        private const string D = "test";
        private readonly ITestOutputHelper _output;
        public LinqTest_BasicOperations(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void Any()
        {
            var q = Arango.Query<Activity>("test").Any();

            Assert.True(q);
        }

        [Fact]
        public async Task GroupBy()
        {
            var q = Arango.Query<Activity>("test")
            .GroupBy(x => new
            {
                x.Start.Year,
                x.Start.Month,
                x.Start.Day
            })
            .Select(g => new
            {
                Day = g.Min(x => x.Revenue) //The error is here. The query on the database generates an array with a single item
                                            //and its trying to parse it as a decimal.
            });

            var result = await q.ToListAsync();

            _output.WriteLine(q.ToAql().aql);
        }

        [Fact]
        public void Count()
        {
            var count = Arango.Query<Activity>("test")
                .Count();
            var longCount = Arango.Query<Activity>("test")
                .LongCount();

            Assert.Equal(5, count);
            Assert.Equal(5L, longCount);
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
                    End = new DateTime(2022, 2, 10),
                    Revenue = 3.4m
                },
                new()
                {
                    Key = "AB",
                    Start = new DateTime(2022, 5, 15),
                    End = new DateTime(2050, 10, 3),
                    Revenue = 4.4m
                },
                new()
                {
                    Key = "AC",
                    Start = new DateTime(2022, 5, 15),
                    End = new DateTime(2050, 10, 3),
                    Revenue = 4.4m
                },
                new()
                {
                    Key = "AD",
                    Start = new DateTime(2022, 5, 15),
                    End = new DateTime(2050, 10, 3),
                    Revenue = 4.4m
                },
                new()
                {
                    Key = "AE",
                    Start = new DateTime(2026, 3, 15),
                    End = new DateTime(2058, 2, 3),
                    Revenue = 4.4m
                }
            });
        }
    }
}
