using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Core.Arango.Protocol;
using Core.Arango.Tests.Core;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Core.Arango.Tests
{
    public class GraphTest : TestBase
    {
        private class Vertex
        {
            public string Key { get; set; }
            public string Name { get; set; }
        }

        [Theory]
        [ClassData(typeof(CamelCaseData))]
        public async Task Get(string serializer)
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

            var friends = await Arango.Query.ExecuteAsync<Vertex>("test",$@"
FOR v, e IN 1..1 OUTBOUND {start} GRAPH 'graph' 
FILTER e.label == 'friend'
RETURN v");

            Assert.Single(friends);

            await Arango.Graph.Edge.UpdateAsync("test", "graph", "edges", new
            {
                Label = "friend"
            }, "ac");

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
    }
}