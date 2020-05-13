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
    }

    public class UnitTest1 : IAsyncLifetime
    {
        protected readonly ArangoContext Arango =
            new ArangoContext($"Server=http://localhost:8529;Realm=CI-{Guid.NewGuid():D};User=root;Password=;");

        /// <summary>
        /// Überprüft die Funktionalität eines SingleOrDefault-Querys mit Einschränkung des Projektnamens
        /// expected query: FOR x IN Project FILTER x.Name == "A" return x
        /// </summary>
        [Fact]
        public async void Test1()
        {
            var test = Arango.AsQueryable<Project>("test").SingleOrDefault(x => x.Name == "A");
            Assert.True(test.Name == "A");
        }

        /// <summary>
        /// expected query: FOR x IN Project FILTER x.Name == "A" return x.Name
        /// </summary>
        [Fact]
        public void Test2()
        {
            var test = Arango.AsQueryable<Project>("test").Where(x => x.Name == "A").Select(x => x.Name).ToList();
            foreach (var t in test)
            {
                Assert.True(t  == "A");
            }
        }

        /// <summary>
        /// expected query: FOR x IN Project FILTER x.Value IN @list RETURN x
        /// </summary>
        [Fact]
        public void Test3()
        {
            var list = new List<int> { 1, 2, 3 };
            var test = Arango.AsQueryable<Project>("test").Where(x => list.Contains(x.Value)).ToList();
        }

        /*[Fact]
        public void Test5()
        {
            var test = Arango.AsQueryable<Project>("test").Where( x=> x.Value == 1 || x.Value == 2).ToList();
        }*/

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
                Value = 1
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