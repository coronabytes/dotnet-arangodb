using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Arango.Protocol;
using Xunit;

namespace Core.Arango.Linq.Tests
{
    public class Project
    {
        public Guid Key { get; set; }
        public string Name { get; set; }
        public int Value { get; set; }

        public DateTime StartDate { get; set; }
    }

    public class UnitTest1 : IAsyncLifetime
    {
        protected readonly ArangoContext Arango =
            new ArangoContext($"Server=http://localhost:8529;Realm=CI-{Guid.NewGuid():D};User=root;Password=;");

        [Fact]
        public async void TestToList()
        {
            var test = Arango.AsQueryable<Project>("test").ToList();
            Assert.True(test.Count > 0);
        }

        /// <summary>
        /// Überprüft die Funktionalität eines SingleOrDefault-Querys mit Einschränkung des Projektnamens
        /// expected query: FOR x IN Project FILTER x.Name == "A" return x
        /// </summary>
        [Fact]
        public async void TestSingleOrDefault()
        {
            var test = Arango.AsQueryable<Project>("test").SingleOrDefault(x => x.Name == "A");
            Assert.True(test.Name == "A");
        }

        /// <summary>
        /// expected query: FOR x IN Project FILTER x.Name == "A" return x.Name
        /// </summary>
        [Fact]
        public void TestWhereSelect()
        {
            var test = Arango.AsQueryable<Project>("test").Where(x => x.Name == "A").Select(x => x.Name).ToList();
            foreach (var t in test)
            {
                Assert.True(t == "A");
            }
        }

        /// <summary>
        /// expected query: FOR x IN Project FILTER x.Value IN @list RETURN x
        /// </summary>
        [Fact]
        public void TestListContains()
        {
            var list = new List<int> { 1, 2, 3 };
            var test = Arango.AsQueryable<Project>("test").Where(x => list.Contains(x.Value)).ToList();
            foreach (var t in test)
            {
                Assert.Contains(t.Value, list);
            }
        }

        /// <summary>
        /// expected query: FOR x IN Project FILTER x.Value == 1 || x.Value == 2 RETURN x
        /// </summary>
        [Fact]
        public async void TestOr()
        {
            var test = Arango.AsQueryable<Project>("test").Where(x => x.Value == 1 || x.Value == 2).ToList();
            foreach (var t in test)
            {
                Assert.True(t.Value == 1 || t.Value == 2);
            }
        }

        /// <summary>
        /// expected query: FOR x IN Project FILTER x.Name LIKE "A%" RETURN x
        /// </summary>
        [Fact]
        public void TestStringBeginsWith()
        {
            var test = Arango.AsQueryable<Project>("test").Where(x => x.Name.StartsWith("A")).ToList();
            foreach (var t in test)
            {
                Assert.StartsWith("A", t.Name);
                Assert.True(test.Count > 0);
            }
        }

        /// <summary>
        /// expected query: FOR x IN Project FILTER x.Name == @p || x.Name == @pUnique RETURN x.Name
        /// </summary>
        [Fact]
        public async void TestInjection()
        {
            var test = Arango.AsQueryable<Project>("test").Where(x => x.Name == "A" || x.Name == "B \" RETURN 42").Select(x => x.Name).ToList();
            Assert.True(test.Count == 1);
        }

        /// <summary>
        /// Überprüft die Funktionalität eines SingleOrDefault-Querys mit Einschränkung der Guid
        /// expected query: FOR x IN Project FILTER x.Key == @testGuid return x
        /// </summary>
        [Fact]
        public async void TestSingleOrDefaultGuid()
        {
            var testGuid = Guid.NewGuid();

            await Arango.CreateDocumentAsync("test", nameof(Project), new Project
            {
                Key = testGuid,
                Name = "TestSingleOrDefault",
                Value = 2
            });

            var test = Arango.AsQueryable<Project>("test").SingleOrDefault(x => x.Key == testGuid);

            Assert.True(test.Key == testGuid);
        }

        /// <summary>
        /// Checks whether the OrderBy is correctly applied.
        /// Expected query: FOR x IN Project SORT x.Value RETURN x
        /// </summary>
        [Fact]
        public void TestOrderBy()
        {
            var test = Arango.AsQueryable<Project>("test").OrderBy(x => x.Value).ToList();

            Assert.True(test[0].Value == 1);
        }

        /// <summary>
        /// Checks if Take is correctly applied
        /// Expected query: FOR x IN Project LIMIT 2 RETURN x
        /// </summary>
        [Fact]
        public void TestTake()
        {
            var test = Arango.AsQueryable<Project>("test").Take(2).ToList();

            Assert.True(test.Count == 2);
        }


        public static class AQL
        {
            public static double DateDiff(DateTime a, DateTime b, string format)
            {
                return 0;
            }
        }
        //todo: AQL.DateDiff(x.StartDate, x.StartDate, "h") > 0).ToList();

        [Fact]
        public void TestDateTimeNow()
        {
            var test = Arango.AsQueryable<Project>("test").Where(x =>  x.StartDate <= DateTime.UtcNow).Select(x => x.StartDate).ToList();

            Assert.True(test.Count == 2);
            foreach (var t in test)
            {
                Assert.True(t <= DateTime.UtcNow);
            }
        }

        // todo
        [Fact]
        public async Task TestToListAsync()
        {
            var test = await Arango.AsQueryable<Project>("test").ToListAsync();
            Assert.True(test.Count == 3);
        }

        // todo
        //[Fact]
        //public async void TestSingleOrDefaultAsync()
        //{
        //    var test = await Arango.AsQueryable<Project>("test").Single
        //}

        /// <summary>
        /// Initialisiert eine Datenbank und eine Collection für die Tests
        /// </summary>
        /// <returns></returns>
        public async Task InitializeAsync()
        {
            await Arango.CreateDatabaseAsync("test");
            await Arango.CreateCollectionAsync("test", nameof(Project), ArangoCollectionType.Document);

            await Arango.CreateDocumentAsync("test", nameof(Project), new Project
            {
                Key = Guid.NewGuid(),
                Name = "A",
                Value = 1,
                StartDate = new DateTime(2020, 04, 03).ToUniversalTime()
            });
            await Arango.CreateDocumentAsync("test", nameof(Project), new Project
            {
                Key = Guid.NewGuid(),
                Name = "B",
                Value = 2,
                StartDate = DateTime.Now.AddDays(-1).ToUniversalTime()
            });
            await Arango.CreateDocumentAsync("test", nameof(Project), new Project
            {
                Key = Guid.NewGuid(),
                Name = "C",
                Value = 3,
                StartDate = new DateTime(2021, 1, 5).ToUniversalTime()
            });
        }

        /// <summary>
        /// Löscht die angelegten Datenbanken
        /// </summary>
        /// <returns></returns>
        public async Task DisposeAsync()
        {
            try
            {
                foreach (var db in await Arango.ListDatabasesAsync())
                    await Arango.DropDatabaseAsync(db);
            }
            catch
            {
                //
            }
        }
    }
}