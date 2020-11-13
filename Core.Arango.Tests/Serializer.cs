using System;
using System.Collections.Generic;
using Core.Arango.Serialization;
using Core.Arango.Tests.Core;
using Xunit;

namespace Core.Arango.Tests
{
    public class SerializerTest
    {
        public static TheoryData<IArangoSerializer> SerializerData =>
            new TheoryData<IArangoSerializer>
            {
                new ArangoJsonNetSerializer(new ArangoDefaultContractResolver()),
                new ArangoJsonNetSerializer(new ArangoCamelCaseContractResolver()),
                new ArangoSystemTextJsonSerializer(new ArangoSystemTextJsonNamingPolicy())
            };

        [Theory]
        [MemberData(nameof(SerializerData))]
        public void SystemTextJson(IArangoSerializer serializer)
        {
            var o1 = new Entity
            {
                Key = Guid.Empty.ToString("D")
            };

            var o2 = new Entity
            {
                Key = null
            };

            var j1 = serializer.Serialize(o1);
            var j2 = serializer.Serialize(o2);

            Assert.Contains("_key", j1);
            Assert.DoesNotContain("_key", j2);

            var o3 = serializer.Deserialize<Entity>(
                @"{""_key"":""00000000-0000-0000-0000-000000000001"",""Name"":null,""Value"":0}");

            Assert.Equal("00000000-0000-0000-0000-000000000001", o3.Key);
        }
    }
}