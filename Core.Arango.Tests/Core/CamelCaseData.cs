using System;
using Core.Arango.Serialization;
using Core.Arango.Serialization.JsonNet;
using Core.Arango.Serialization.Newtonsoft;
using Core.Arango.Serialization.System;
using Core.Arango.Serialization.SystemTextJson;
using Xunit;

namespace Core.Arango.Tests
{
    public class CamelCaseData : TheoryData<IArangoContext>
    {
        private string UniqueTestRealm()
        {
            var cs = Environment.GetEnvironmentVariable("ARANGODB_CONNECTION");

            if (string.IsNullOrWhiteSpace(cs))
                cs = "Server=http://localhost:8529;Realm=CI-{UUID};User=root;Password=;";

            return cs.Replace("{UUID}", Guid.NewGuid().ToString("D"));
        }

        public CamelCaseData()
        {
            Add(new ArangoContext(UniqueTestRealm(), new ArangoConfiguration
            {
                Serializer = new ArangoSystemTextJsonSerializer(new ArangoJsonCamelCasePolicy())
            }));
            Add(new ArangoContext(UniqueTestRealm(), new ArangoConfiguration
            {
                Serializer = new ArangoJsonNetSerializer(new ArangoCamelCaseContractResolver())
            }));
        }
    }
}