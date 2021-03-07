using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Core.Arango.Serialization;
using Core.Arango.Serialization.Json;
using Core.Arango.Serialization.Newtonsoft;
using Core.Arango.Tests.Core;
using Xunit;
using Xunit.Abstractions;

namespace Core.Arango.Tests
{
    public class SerializerTest
    {
        private readonly ITestOutputHelper _output;

        public SerializerTest(ITestOutputHelper output)
        {
            _output = output;
        }

        public static TheoryData<IArangoSerializer, string> SerializerData =>
            new TheoryData<IArangoSerializer, string>
            {
                {new ArangoNewtonsoftSerializer(new ArangoNewtonsoftDefaultContractResolver()), "Newtonsoft(Default)"},
                {new ArangoNewtonsoftSerializer(new ArangoNewtonsoftCamelCaseContractResolver()), "Newtonsoft(Camel)"},
                {new ArangoJsonSerializer(new ArangoJsonCamelCasePolicy()), "System.Json.Text(Camel)"},
                {new ArangoJsonSerializer(new ArangoJsonDefaultPolicy()), "System.Json.Text(Default)"}
            };

        [Theory]
        [MemberData(nameof(SerializerData))]
        public void Equality(IArangoSerializer serializer, string name)
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

            _output.WriteLine(name);
            Assert.Contains("_key", j1);
            Assert.DoesNotContain("_key", j2);

            var o3 = serializer.Deserialize<Entity>(
                @"{""_key"":""00000000-0000-0000-0000-000000000001"",""Name"":null,""Value"":0}");

            Assert.Equal("00000000-0000-0000-0000-000000000001", o3.Key);
        }

        [Theory]
        [MemberData(nameof(SerializerData))]
        public void Performance(IArangoSerializer serializer, string name)
        {
            var docs = Enumerable.Range(1, 100000)
                .Select(x => new Entity {Value = x});

            var sw = new Stopwatch();
            sw.Start();

            for (var i = 0; i < 10; i++)
            {
                var json = serializer.Serialize(docs);
                serializer.Deserialize<List<Entity>>(json);
            }

            sw.Stop();

            _output.WriteLine($"{name}: {sw.Elapsed.TotalMilliseconds}");
        }

        [Fact]
        public void Bla()
        {
            var m = new MultipartFormDataContent("SomeBoundaryValue");
            var s1 = new StringContent("GET /_api/collection/products/figures HTTP/1.1}\r\n");
            s1.Headers.ContentType = new MediaTypeHeaderValue("application/x-arango-batchpart");
            s1.Headers.Add("Content-Id", Guid.NewGuid().ToString("D"));
            m.Add(s1, "a");
            s1.Headers.Remove("Content-Disposition");

            var s2 = new StringContent("POST /_api/collection/products HTTP/1.1\r\n\r\n{\"name\": \"products\" }\r\n");
            s2.Headers.ContentType = new MediaTypeHeaderValue("application/x-arango-batchpart");
            s2.Headers.Add("Content-Id", Guid.NewGuid().ToString("D"));
            m.Add(s2, "b");
            s2.Headers.Remove("Content-Disposition");

            var res = m.ReadAsStringAsync().Result;

            _output.WriteLine(res);
        }
    }
}