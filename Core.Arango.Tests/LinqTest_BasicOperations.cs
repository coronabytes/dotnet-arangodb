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
        public string Id { get; set; }
        public string Key { get; set; }
        public string Revision { get; set; }
    }

    class Pet
    {
        public string Name { get; set; }
        public Person Owner { get; set; }
        public string Id { get; set; }
        public string Key { get; set; }
        public string Revision { get; set; }
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
        public async ValueTask GroupBy()
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

        /*[Fact]
        public async ValueTask GroupJoin()
        {
            var magnus = new Person { Name = "Hedlund, Magnus", Key = "Per1" };
            var terry = new Person { Name = "Adams, Terry", Key = "Per2" };
            var charlotte = new Person { Name = "Weiss, Charlotte", Key = "Per3" };

            var barley = new Pet { Name = "Barley", Owner = terry, Key= "Pet1" };
            var boots = new Pet { Name = "Boots", Owner = terry, Key = "Pet2" };
            var whiskers = new Pet { Name = "Whiskers", Owner = charlotte, Key = "Pet3" };
            var daisy = new Pet { Name = "Daisy", Owner = magnus, Key = "Pet4" };

            var people = new List<Person> { magnus, terry, charlotte };
            var pets = new List<Pet> { barley, boots, whiskers, daisy };

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
        }*/

        /*//Check the AQL being generated
        [Fact]
        public async ValueTask Join()
        {
            var magnus = new Person { Name = "Hedlund, Magnus", Key = "Per1" };
            var terry = new Person { Name = "Adams, Terry", Key = "Per2" };
            var charlotte = new Person { Name = "Weiss, Charlotte", Key = "Per3" };

            var barley = new Pet { Name = "Barley", Owner = terry, Key = "Pet1" };
            var boots = new Pet { Name = "Boots", Owner = terry, Key = "Pet2" };
            var whiskers = new Pet { Name = "Whiskers", Owner = charlotte, Key = "Pet3" };
            var daisy = new Pet { Name = "Daisy", Owner = magnus, Key = "Pet4" };

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
        }*/

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
        public async ValueTask Count_In_Filter()
        {
            var q = Arango.Query<Activity>("test")
                .Where(x => x.Notes.Count() == 3);

            _output.WriteLine(q.ToAql().aql);

            var activities = await q.ToListAsync();

            Assert.Single(activities);
        }

        [Fact]
        public async ValueTask Count_In_Select()
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
        public async ValueTask Contains()
        {
            var p = await Arango.Query<Activity>("test").FirstOrDefaultAsync();

            _output.WriteLine(JsonConvert.SerializeObject(p));

            var boolean = Arango.Query<Activity>("test").Contains(p); // This should work: does `p` not get serialized the same way is it gets de-serialized? This operations should be inverse of each other.

            Assert.True(boolean);
        }

        /*[Fact]
        public async ValueTask Distinct()
        {
            var per1 = new Person { Name = "Person1", Key = "Per1" };
            var per2 = new Person { Name = "Person1", Key = "Per2" };
            var per3 = new Person { Name = "Person2", Key = "Per3" };

            var people = new List<Person> { per1, per2, per3 };

            await Arango.Collection.CreateAsync(D, nameof(Person), ArangoCollectionType.Document);
            await Arango.Document.CreateManyAsync(D, nameof(Person), people);
            
            var p = await Arango.Query<Person>("test")
                .Select(x => x.Name)
                .Distinct()
                .ToListAsync();

            Assert.Equal(2, p.Count);
        }*/

        /*[Fact]
        public async ValueTask Except_Compare_Objects()
        {
            var list = await Arango.Query<Activity>("test")
                .Take(2)
                .ToListAsync();

            var q = Arango.Query<Activity>("test")
                .Except(list);

            PrintQuery(q, _output);

            var p = await q.ToListAsync();

            Assert.Equal(3, p.Count); // TODO
        }*/

        [Fact]
        public async ValueTask Except_Compare_Keys()
        {
            var list = await Arango.Query<Activity>("test")
                .Take(2)
                .Select(x => x.Key)
                .ToListAsync();

            var q = Arango.Query<Activity>("test")
                .Select(x => x.Key)
                .Except(list);

            PrintQuery(q, _output);

            var p = await q.ToListAsync();

            Assert.Equal(3, p.Count);
        }

        [Fact]
        public async ValueTask Combine_Skip_Take()
        {
            var q = Arango.Query<Activity>("test")
                .Skip(1)
                .Take(1)
                .Select(x => x.Key);

            PrintQuery(q, _output);

            var p = await q.ToListAsync();

            Assert.Equal(new List<string> { "AB" }, p); // TODO : This fails but should pass. Another instance of object not serialized correctly so arango can't compare?
        }

        /*[Fact]
        public async ValueTask Intersect()
        {
            var list = await Arango.Query<Activity>("test").Take(1).ToListAsync();

            var q = Arango.Query<Activity>("test").Intersect(list);

            _output.WriteLine(q.ToAql().aql);

            var p = await q.ToListAsync();

            Assert.Single(p); // TODO : This fails but should pass. Another instance of object not serialized correctly so arango can't compare?
        }

        [Fact]
        public async ValueTask Intersect_With_Count()
        {
            var list = await Arango.Query<Activity>("test").Take(1).ToListAsync();

            var q = Arango.Query<Activity>("test").Intersect(list).Count(); // TODO : Result operators are called in the wrong order

            Assert.Equal(1, q);
        }*/

        [Fact]
        public async ValueTask Union()
        {
            var personList1 = new List<Person>
            {
                new Person { Name = "Person1", Key = "Per1" },
                new Person { Name = "Person2", Key = "Per2" },
                new Person { Name = "Person3", Key = "Per3" },
                new Person { Name = "Person4", Key = "Per4" }
            };
            var personList2 = new List<Person>
            {
                new Person { Name = "Person3", Key = "Per3" },
                new Person { Name = "Person4", Key = "Per4" },
                new Person { Name = "Person5", Key = "Per5" },
                new Person { Name = "Person6", Key = "Per6" }
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

        /*//This test has precision issues.
        [Fact]
        public async ValueTask Average()
        {
            var expectedAverage = (new List<decimal> { 3.4m, 4.4m }).AsQueryable().Average();
            var average = Arango.Query<Activity>("test").Where(x => x.Key == "AA" || x.Key == "AB").Select(x => x.Revenue).Average();

            Assert.Equal(expectedAverage, average);
        }*/

        [Fact]
        public async ValueTask Min()
        {
            var min = Arango.Query<Activity>("test").Select(x => x.Revenue).Min();

            Assert.Equal(3.4m, min);
        }

        [Fact]
        public async ValueTask Max()
        {
            var max = Arango.Query<Activity>("test").Select(x => x.Revenue).Max();

            Assert.Equal(4.4m, max);
        }

        [Fact]
        public async ValueTask Sum()
        {
            var expectedSum = (4.4m * 4) + 3.4m;
            var sum = Arango.Query<Activity>("test").Select(x => x.Revenue).Sum();

            Assert.Equal(expectedSum, sum);
        }

        /*[Fact]
        public async ValueTask SameVariableChained()
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
                .SelectMany(y => y.innerChains)
                .Where(y => y.A == "A")
                .Where(y => y.B == "B")
                .Where(y => y.C == "C");

            _output.WriteLine(q.ToAql().aql);

            var c = await q.FirstOrDefaultAsync();
        }*/

        /*[Fact]
        public async ValueTask Cast()
        {
            Person per1 = new Person { Name = "Person1", Key = "Per1" };
            Person per2 = new Person { Name = "Person1", Key = "Per2" };
            Person per3 = new Person { Name = "Person2", Key = "Per3" };

            List<Person> people = new List<Person> { per1, per2, per3 };

            await Arango.Collection.CreateAsync(D, nameof(Person), ArangoCollectionType.Document);
            await Arango.Document.CreateManyAsync(D, nameof(Person), people);

            IEnumerable<int> i = Arango.Query<Activity>("test").Select(x => x.Revenue).Cast<int>();

            Assert.IsType<int>(i.First());
            _output.WriteLine("");
        }*/

        /*[Fact]
        public async ValueTask Last()
        {
            var a = Arango.Query<Activity>("test").LastOrDefault();
            Assert.Equal("AE", a.Key);
        }

        [Fact]
        public async ValueTask Reverse()
        {
            var a = await Arango.Query<Activity>("test").Reverse().FirstOrDefaultAsync();
            Assert.Equal("AE", a.Key);
        }*/

        [Fact]
        public async ValueTask Single()
        {
            var a = await Arango.Query<Activity>("test").Where(x => x.Key == "AA").SingleOrDefaultAsync();
            Assert.Equal("AA", a.Key);
        }

        [Fact]
        public async ValueTask Take()
        {
            var a = await Arango.Query<Activity>("test").Take(2).ToListAsync();
            Assert.Equal("AA", a[0].Key);
            Assert.Equal("AB", a[1].Key);
        }

        public override async ValueTask InitializeAsync()
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
