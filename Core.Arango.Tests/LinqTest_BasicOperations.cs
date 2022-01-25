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
            var magnus = new Person { Name = "Hedlund, Magnus" };
            var terry = new Person { Name = "Adams, Terry" };
            var charlotte = new Person { Name = "Weiss, Charlotte" };

            var barley = new Pet { Name = "Barley", Owner = terry };
            var boots = new Pet { Name = "Boots", Owner = terry };
            var whiskers = new Pet { Name = "Whiskers", Owner = charlotte };
            var daisy = new Pet { Name = "Daisy", Owner = magnus };

            var people = new List<Person> { magnus, terry, charlotte };
            var pets = new List<Pet> { barley, boots, whiskers, daisy };

            await Arango.Collection.CreateAsync(D, nameof(Person), ArangoCollectionType.Document);
            await Arango.Collection.CreateAsync(D, nameof(Pet), ArangoCollectionType.Document);

            await Arango.Document.CreateManyAsync(D, nameof(Person), people);
            await Arango.Document.CreateManyAsync(D, nameof(Pet), pets);

            var q = Arango.Query<Person>("test")
                .Join(pets,
                    person => person.Name,
                    pet => pet.Owner.Name,
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

        [Fact]
        public void LongCount()
        {
            var longCount = Arango.Query<Activity>("test")
                .LongCount();

            Assert.Equal(5L, longCount);
        }

        [Fact]
        public void Count_Result()
        {
            var count = Arango.Query<Activity>("test")
                .Count();

            Assert.Equal(5, count);
        }

        [Fact]
        public async Task Count_In_Filter()
        {
            var q = Arango.Query<Activity>("test")
                .Where(x => x.Notes.Count() == 3);

            _output.WriteLine(q.ToAql().aql);

            var activities = await q.ToListAsync();

            Assert.Single(activities);
        }

        [Fact]
        public async Task Count_In_Select()
        {
            var q = Arango.Query<Activity>("test")
                .Select(x => x.Notes.Count());

            _output.WriteLine(q.ToAql().aql);

            var activitiesNotesCount = await q.ToListAsync();

            Assert.Equal(new List<int> { 1, 2, 3, 0, 0 }, activitiesNotesCount);
        }

        [Fact]
        public void All()
        {
            var shouldBeTrue = Arango.Query<Activity>("test").All(x => x.Key.Contains("A"));
            var shouldBeFalse = Arango.Query<Activity>("test").All(x => x.Key.Contains("X"));

            Assert.True(shouldBeTrue);
            Assert.False(shouldBeFalse);
        }

        [Fact]
        public async Task Contains()
        {
            var p = await Arango.Query<Activity>("test").FirstOrDefaultAsync();

            _output.WriteLine(JsonConvert.SerializeObject(p));

            var boolean = Arango.Query<Activity>("test").Contains(p); // This should work: does `p` not get serialized the same way is it gets de-serialized? This operations should be inverse of each other.

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
            
            var p = await Arango.Query<Person>("test")
                .Select(x => x.Name)
                .Distinct()
                .ToListAsync();

            Assert.Equal(2, p.Count);
        }

        [Fact]
        public async Task Except()
        {
            var list = await Arango.Query<Activity>("test")
                .Take(2)
                .ToListAsync();

            var q = Arango.Query<Activity>("test")
                .Except(list);

            _output.WriteLine(q.ToAql().aql);

            var p = await q.ToListAsync();

            Assert.Equal(2, p.Count); // TODO : This fails but should pass. Another instance of object not serialized correctly so arango can't compare?
        }

        [Fact]
        public async Task Intersect()
        {
            var list = await Arango.Query<Activity>("test").Take(1).ToListAsync();

            var q = Arango.Query<Activity>("test").Intersect(list);

            _output.WriteLine(q.ToAql().aql);

            var p = await q.ToListAsync();

            Assert.Single(p); // TODO : This fails but should pass. Another instance of object not serialized correctly so arango can't compare?
        }

        [Fact]
        public async Task Intersect_With_Count()
        {
            var list = await Arango.Query<Activity>("test").Take(1).ToListAsync();

            var q = Arango.Query<Activity>("test").Intersect(list).Count(); // TODO : Result operators are called in the wrong order

            Assert.Equal(1, q);
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

            var q = Arango.Query<Person>("test")
                .Select(x => x.Name)
                .Union(personList2.Select(x => x.Name));

            var aql = q.ToAql();

            _output.WriteLine(aql.aql);
            _output.WriteLine(JsonConvert.SerializeObject(aql.bindVars));

            var p = await q.ToListAsync();

            Assert.Equal(6, p.Count);
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
                        C = "C"
                    },
                    new InnerChain()
                    {
                        A = "A",
                        B = "B",
                        C = "C"
                    },
                    new InnerChain()
                    {
                        A = "A",
                        B = "B",
                        C = "C"
                    }
                }
            };

            await Arango.Collection.CreateAsync("test", nameof(OutterChain), ArangoCollectionType.Document);
            await Arango.Document.CreateAsync("test", nameof(OutterChain), chainTest);

            var q = Arango.Query<OutterChain>("test")
                .SelectMany(x => x.innerChains)
                .Where(x => x.A == "A")
                .Where(x => x.B == "B")
                .Where(x => x.C == "C");

            _output.WriteLine(q.ToAql().aql);

            var c = await q.FirstOrDefaultAsync();
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
                    Revenue = 3.4m,
                    Notes = new List<Note>
                    {
                        new Note { CreatedOn = new DateTime(2021, 1, 30), Text = "Note 1 (AA)" },
                    }
                },
                new()
                {
                    Key = "AB",
                    Start = new DateTime(2022, 5, 15),
                    End = new DateTime(2050, 10, 3),
                    Revenue = 4.4m,
                    Notes = new List<Note>
                    {
                        new Note { CreatedOn = new DateTime(2022, 5, 15), Text = "Note 1 (AB)" },
                        new Note { CreatedOn = new DateTime(2022, 5, 16), Text = "Note 2 (AB)" },
                    }
                },
                new()
                {
                    Key = "AC",
                    Start = new DateTime(2022, 5, 15),
                    End = new DateTime(2050, 10, 3),
                    Revenue = 4.4m,
                    Notes = new List<Note>
                    {
                        new Note { CreatedOn = new DateTime(2022, 5, 15), Text = "Note 1 (AC)" },
                        new Note { CreatedOn = new DateTime(2022, 5, 16), Text = "Note 2 (AC)" },
                        new Note { CreatedOn = new DateTime(2022, 5, 17), Text = "Note 3 (AC)" },
                    }
                },
                new()
                {
                    Key = "AD",
                    Start = new DateTime(2022, 5, 15),
                    End = new DateTime(2050, 10, 3),
                    Revenue = 4.4m,
                    Notes = new List<Note>()
                },
                new()
                {
                    Key = "AE",
                    Start = new DateTime(2026, 3, 15),
                    End = new DateTime(2058, 2, 3),
                    Revenue = 4.4m,
                    Notes = new List<Note>()
                }
            });
        }
    }
}
