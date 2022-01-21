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
    class Person
    {
        public string Name { get; set; }
    }

    class Pet
    {
        public string Name { get; set; }
        public Person Owner { get; set; }
    }

    class OutterChain
    {
        public string Name { get; set; }
        public List<InnerChain> innerChains { get; set; }
    }

    class InnerChain
    {
        public string A { get; set; }
        public string B { get; set; }
        public string C { get; set; }
        public string D { get; set; }
        public string E { get; set; }
        public string F { get; set; }
    }

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
        public async Task GroupJoin()
        {
            Person magnus = new Person { Name = "Hedlund, Magnus" };
            Person terry = new Person { Name = "Adams, Terry" };
            Person charlotte = new Person { Name = "Weiss, Charlotte" };

            Pet barley = new Pet { Name = "Barley", Owner = terry };
            Pet boots = new Pet { Name = "Boots", Owner = terry };
            Pet whiskers = new Pet { Name = "Whiskers", Owner = charlotte };
            Pet daisy = new Pet { Name = "Daisy", Owner = magnus };

            List<Person> people = new List<Person> { magnus, terry, charlotte };
            List<Pet> pets = new List<Pet> { barley, boots, whiskers, daisy };

            await Arango.Collection.CreateAsync(D, nameof(Person), ArangoCollectionType.Document);
            await Arango.Collection.CreateAsync(D, nameof(Pet), ArangoCollectionType.Document);

            await Arango.Document.CreateManyAsync(D, nameof(Person), people);
            await Arango.Document.CreateManyAsync(D, nameof(Pet), pets);

            var q = Arango.Query<Person>("test")
                .GroupJoin(pets,
                           person => person,
                           pet => pet.Owner,
                           (person, Pet) => //petColletion should be a parameter (@P1). So this will throw an error unless this collection is in the db
                               new          //and it has the same name as said collection. Unless this is intended to work this way.
                               {
                                   OwnerName = person.Name,
                                   pets = Pet.Select(pet => pet.Name)
                               });

            var result = await q.ToListAsync();

            Assert.Single(result[0].pets);
            Assert.Equal(2, result[1].pets.Count());
            Assert.Single(result[3].pets);

            _output.WriteLine(q.ToAql().aql);
        }

        //Check the AQL being generated
        [Fact]
        public async Task Join()
        {
            Person magnus = new Person { Name = "Hedlund, Magnus" };
            Person terry = new Person { Name = "Adams, Terry" };
            Person charlotte = new Person { Name = "Weiss, Charlotte" };

            Pet barley = new Pet { Name = "Barley", Owner = terry };
            Pet boots = new Pet { Name = "Boots", Owner = terry };
            Pet whiskers = new Pet { Name = "Whiskers", Owner = charlotte };
            Pet daisy = new Pet { Name = "Daisy", Owner = magnus };

            List<Person> people = new List<Person> { magnus, terry, charlotte };
            List<Pet> pets = new List<Pet> { barley, boots, whiskers, daisy };

            await Arango.Collection.CreateAsync(D, nameof(Person), ArangoCollectionType.Document);
            await Arango.Collection.CreateAsync(D, nameof(Pet), ArangoCollectionType.Document);

            await Arango.Document.CreateManyAsync(D, nameof(Person), people);
            await Arango.Document.CreateManyAsync(D, nameof(Pet), pets);

            var q = Arango.Query<Person>("test")
                .Join(pets,
                           person => person,
                           pet => pet.Owner,
                           (person, Pet) => //petColletion should be a parameter (@P1). So this will throw an error unless this collection is in the db
                               new          //and it has the same name as said collection. Unless this is intended to work this way.
                               {
                                   OwnerName = person.Name,
                                   Pet = Pet.Name
                               });

            var result = await q.ToListAsync();

            Assert.Equal("Barley", result[0].Pet);
            Assert.Equal("Boots", result[1].Pet);
            Assert.Equal("Whiskers", result[2].Pet);
            Assert.Equal("Daisy", result[3].Pet);

            _output.WriteLine(q.ToAql().aql);
        }

        public void Count()
        {
            var count = Arango.Query<Activity>("test")
                .Count();
            var longCount = Arango.Query<Activity>("test")
                .LongCount();

            Assert.Equal(5, count);
            Assert.Equal(5L, longCount);
        }

        [Fact]
        public void All()
        {
            var boolean = Arango.Query<Activity>("test").All(x => x.Key.Contains("A"));

            Assert.True(boolean);
        }

        [Fact]
        public async Task Contains()
        {
            var p = await Arango.Query<Activity>("test").FirstOrDefaultAsync();

            var boolean = Arango.Query<Activity>("test").Contains(p);

            Assert.True(boolean);
        }

        [Fact]
        public async Task Distinct()
        {
            Person per1 = new Person { Name = "Person1" };
            Person per2 = new Person { Name = "Person1" };
            Person per3 = new Person { Name = "Person2" };

            List<Person> people = new List<Person> { per1, per2, per3 };

            await Arango.Collection.CreateAsync(D, nameof(Person), ArangoCollectionType.Document);
            await Arango.Document.CreateManyAsync(D, nameof(Person), people);
            
            var p = await Arango.Query<Person>("test").Distinct().ToListAsync();

            Assert.Equal(2, p.Count());
        }

        [Fact]
        public async Task Except()
        {
            var list = await Arango.Query<Activity>("test").Take(2).ToListAsync();

            var p = await Arango.Query<Activity>("test").Except(list).ToListAsync();

            Assert.Equal(2, p.Count());
        }

        [Fact]
        public async Task Intersect()
        {
            var list = await Arango.Query<Activity>("test").Take(1).ToListAsync();

            var p = await Arango.Query<Activity>("test").Intersect(list).ToListAsync();

            Assert.Single(p);
        }

        [Fact]
        public async Task Union()
        {
            var personList1 = new List<Person>
            {
                new Person { Name = "Person1" },
                new Person { Name = "Person2" },
                new Person { Name = "Person3" },
                new Person { Name = "Person4" }
            };
            var personList2 = new List<Person>
            {
                new Person { Name = "Person3" },
                new Person { Name = "Person4" },
                new Person { Name = "Person5" },
                new Person { Name = "Person6" }
            };

            await Arango.Collection.CreateAsync(D, nameof(Person), ArangoCollectionType.Document);

            await Arango.Document.CreateManyAsync(D, nameof(Person), personList1);

            var p = await Arango.Query<Person>("test").Union(personList2).ToListAsync();

            Assert.Equal(6, p.Count());
        }

        //This test has precision issues.
        [Fact]
        public async Task Average()
        {
            var expectedAverage = (new List<decimal> { 3.4m, 4.4m }).AsQueryable().Average();
            var average = Arango.Query<Activity>("test").Where(x => x.Key == "AA" || x.Key == "AB").Select(x => x.Revenue).Average();

            Assert.Equal(expectedAverage, average);
        }

        [Fact]
        public async Task Min()
        {
            var min = Arango.Query<Activity>("test").Select(x => x.Revenue).Min();

            Assert.Equal(3.4m, min);
        }

        [Fact]
        public async Task Max()
        {
            var max = Arango.Query<Activity>("test").Select(x => x.Revenue).Max();

            Assert.Equal(4.4m, max);
        }

        [Fact]
        public async Task Sum()
        {
            var expectedSum = (4.4m * 4) + 3.4m;
            var sum = Arango.Query<Activity>("test").Select(x => x.Revenue).Sum();

            Assert.Equal(expectedSum, sum);
        }

        [Fact]
        public async Task SameVariableChained()
        {
            var chainTest = new OutterChain()
            {
                Name = "OutterChain_1",
                innerChains = new List<InnerChain>()
                {
                    new InnerChain()
                    {
                        A = "A",
                        B = "B",
                        C = "C",
                        D = "D",
                        E = "E",
                        F = "F"
                    },
                    new InnerChain()
                    {
                        A = "A",
                        B = "B",
                        C = "C",
                        D = "D",
                        E = "E",
                        F = "F"
                    },
                    new InnerChain()
                    {
                        A = "A",
                        B = "B",
                        C = "C",
                        D = "D",
                        E = "E",
                        F = "F"
                    }
                }
            };

            await Arango.Collection.CreateAsync("test", nameof(OutterChain), ArangoCollectionType.Document);
            await Arango.Document.CreateAsync("test", nameof(OutterChain), chainTest);

            var q = Arango.Query<OutterChain>("test").SelectMany(x => x.innerChains).Where(x => x.A == "A").Where(x => x.B == "B").Where(x => x.C == "C").Where(x => x.D == "D").Where(x => x.E == "E").Where(x => x.F == "F");
            var c = await q.FirstOrDefaultAsync();

            _output.WriteLine(q.ToAql().aql);
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
