using Core.Arango.Protocol;
using Core.Arango.Tests.Core;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Core.Arango.Tests
{
    public class GraphTest : TestBase
    {
        [Theory]
        [ClassData(typeof(CamelCaseData))]
        public async ValueTask Get(string serializer)
        {
            await SetupAsync(serializer);
            await Arango.Collection.CreateAsync("test", "vertices", ArangoCollectionType.Document);
            await Arango.Collection.CreateAsync("test", "edges", ArangoCollectionType.Edge);

            await Arango.Graph.CreateAsync("test", new ArangoGraph
            {
                Name = "graph",
                EdgeDefinitions = new List<ArangoEdgeDefinition>
                {
                    new()
                    {
                        Collection = "edges",
                        From = new List<string> {"vertices"},
                        To = new List<string> {"vertices"}
                    }
                }
            });

            var list = await Arango.Graph.ListAsync("test");

            foreach (var graph in list)
            {
                var def = await Arango.Graph.GetAsync("test", graph.Name);
            }

            await Arango.Graph.Vertex.CreateAsync("test", "graph", "vertices", new
            {
                Key = "alice",
                Name = "Alice"
            });

            await Arango.Graph.Vertex.CreateAsync("test", "graph", "vertices", new
            {
                Key = "bob",
                Name = "Bob"
            });

            await Arango.Graph.Vertex.CreateAsync("test", "graph", "vertices", new
            {
                Key = "cesar",
                Name = "Cesar"
            });

            await Arango.Graph.Edge.CreateAsync("test", "graph", "edges", new
            {
                Key = "ab",
                From = "vertices/alice",
                To = "vertices/bob",
                Label = "friend"
            });

            await Arango.Graph.Edge.CreateAsync("test", "graph", "edges", new
            {
                Key = "ac",
                From = "vertices/alice",
                To = "vertices/cesar",
                Label = "foe"
            });


            var start = "vertices/alice";

            var friends = await Arango.Query.ExecuteAsync<Vertex>("test", $@"
                FOR v, e IN 1..1 OUTBOUND {start} GRAPH 'graph' 
                FILTER e.label == 'friend'
                RETURN v");

            Assert.Single(friends);

            await Arango.Graph.Edge.UpdateAsync("test", "graph", "edges", "ac", new
            {
                Label = "friend"
            });

            friends = await Arango.Query.ExecuteAsync<Vertex>("test", $@"
                FOR v, e IN 1..1 OUTBOUND {start} GRAPH 'graph' 
                FILTER e.label == 'friend'
                RETURN v");

            Assert.Equal(2, friends.Count);

            await Arango.Graph.Vertex.RemoveAsync("test", "graph", "vertices", "bob");

            friends = await Arango.Query.ExecuteAsync<Vertex>("test", $@"
                FOR v, e IN 1..1 OUTBOUND {start} GRAPH 'graph' 
                FILTER e.label == 'friend'
                RETURN v");

            Assert.Single(friends);

            await Arango.Graph.DropAsync("test", "graph");
        }

        [Theory]
        [ClassData(typeof(CamelCaseData))]
        public async ValueTask TransactionalOperations(string serializer)
        {
            await SetupAsync(serializer);
            await Arango.Collection.CreateAsync("test", "vertices", ArangoCollectionType.Document);
            await Arango.Collection.CreateAsync("test", "edges", ArangoCollectionType.Edge);

            await Arango.Graph.CreateAsync("test", new ArangoGraph
            {
                Name = "graph",
                EdgeDefinitions = new List<ArangoEdgeDefinition>
                {
                    new()
                    {
                        Collection = "edges",
                        From = new List<string> {"vertices"},
                        To = new List<string> {"vertices"}
                    }
                }
            });

            var transaction0 = await Arango.Transaction.BeginAsync("test",
                new ArangoTransaction()
                { Collections = new ArangoTransactionScope { Write = [ "vertices", "edges" ] }});

            await Arango.Graph.Vertex.CreateAsync(transaction0, "graph", "vertices", new
            {
                Key = "alice",
                Name = "Alice"
            });

            await Arango.Graph.Vertex.CreateAsync("test", "graph", "vertices", new
            {
                Key = "bob",
                Name = "Bob"
            });

            await Arango.Graph.Vertex.CreateAsync("test", "graph", "vertices", new
            {
                Key = "cesar",
                Name = "Cesar"
            });

            var nodes = await Arango.Query.ExecuteAsync<Vertex>("test", $@"FOR v IN {"vertices":@} RETURN v");

            Assert.Equal(2, nodes.Count);

            nodes = await Arango.Query.ExecuteAsync<Vertex>(transaction0, $@"FOR v IN {"vertices":@} RETURN v");

            Assert.Collection(nodes, node => Assert.Equal("Alice", node.Name));

            await Arango.Transaction.CommitAsync(transaction0);

            nodes = await Arango.Query.ExecuteAsync<Vertex>("test", $@"FOR v IN {"vertices":@} RETURN v");

            Assert.Equal(3, nodes.Count);

            var transaction1 = await Arango.Transaction.BeginAsync("test",
                new ArangoTransaction()
                { Collections = new ArangoTransactionScope { Write = ["vertices", "edges" ] } });

            await Arango.Graph.Edge.CreateAsync(transaction1, "graph", "edges", new
            {
                Key = "ab",
                From = "vertices/alice",
                To = "vertices/bob",
                Label = "friend"
            });

            var transaction2 = await Arango.Transaction.BeginAsync("test",
                new ArangoTransaction()
                { Collections = new ArangoTransactionScope { Write = [ "vertices", "edges" ] } });

            await Arango.Graph.Edge.CreateAsync(transaction2, "graph", "edges", new
            {
                Key = "ac",
                From = "vertices/alice",
                To = "vertices/cesar",
                Label = "friend"
            });


            var start = "vertices/alice";

            var friends = await Arango.Query.ExecuteAsync<Vertex>("test", $@"
                FOR v, e IN 1..1 OUTBOUND {start} GRAPH 'graph'
                RETURN v");

            Assert.Empty(friends);

            friends = await Arango.Query.ExecuteAsync<Vertex>(transaction1, $@"
                FOR v, e IN 1..1 OUTBOUND {start} GRAPH 'graph'
                RETURN v");

            Assert.Collection(friends, friend => Assert.Equal("Bob", friend.Name));

            friends = await Arango.Query.ExecuteAsync<Vertex>(transaction2, $@"
                FOR v, e IN 1..1 OUTBOUND {start} GRAPH 'graph'
                RETURN v");

            Assert.Collection(friends, friend => Assert.Equal("Cesar", friend.Name));

            await Arango.Transaction.CommitAsync(transaction1);

            friends = await Arango.Query.ExecuteAsync<Vertex>("test", $@"
                FOR v, e IN 1..1 OUTBOUND {start} GRAPH 'graph'
                RETURN v");

            Assert.Collection(friends, friend => Assert.Equal("Bob", friend.Name));

            await Arango.Transaction.CommitAsync(transaction2);

            friends = await Arango.Query.ExecuteAsync<Vertex>("test", $@"
                FOR v, e IN 1..1 OUTBOUND {start} GRAPH 'graph'
                RETURN v");

            Assert.Equal(2, friends.Count);

            await Arango.Graph.DropAsync("test", "graph");
        }

        [Theory]
        [ClassData(typeof(CamelCaseData))]
        public async ValueTask ReplaceEdgeDefinition(string serializer)
        {
            await SetupAsync(serializer);
            await Arango.Collection.CreateAsync("test", "vertices1", ArangoCollectionType.Document);
            await Arango.Collection.CreateAsync("test", "vertices2", ArangoCollectionType.Document);
            await Arango.Collection.CreateAsync("test", "edges", ArangoCollectionType.Edge);

            await Arango.Graph.CreateAsync("test", new ArangoGraph
            {
                Name = "graph",
                EdgeDefinitions = new List<ArangoEdgeDefinition>
                {
                    new()
                    {
                        Collection = "edges",
                        From = new List<string> {"vertices1"},
                        To = new List<string> {"vertices1"}
                    }
                }
            });

            var newDef = new ArangoEdgeDefinition
            {
                Collection = "edges",
                From = new List<string> { "vertices1" },
                To = new List<string> { "vertices2" }
            };

            await Arango.Graph.ReplaceEdgeDefinitionAsync("test", "graph", newDef);

            var graphs = await Arango.Graph.ListAsync("test");
            foreach (var graph in graphs)
            {
                Assert.Single(graph.EdgeDefinitions);

                var def = graph.EdgeDefinitions.Single();

                Assert.Equal("edges", def.Collection);
                Assert.Single(def.From);
                Assert.Single(def.To);

                Assert.Contains("vertices1", def.From);
                Assert.Contains("vertices2", def.To);
            }
        }

        private class Vertex
        {
            public string Key { get; set; }
            public string Name { get; set; }
        }
    }
}