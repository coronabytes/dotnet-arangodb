using System;
using System.Collections.Generic;
using Core.Arango.Serialization;
using Core.Arango.Transport;
using Newtonsoft.Json.Linq;

namespace Core.Arango
{
    public class ArangoConfiguration : IArangoConfiguration
    {
        public ArangoConfiguration()
        {
            BatchSize = 500;
            Serializer = new ArangoJsonNetSerializer(new ArangoDefaultContractResolver());
            Transport = new ArangoHttpTransport(this);
        }

        public string Realm { get; set; }
        public string Server { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public int BatchSize { get; set; }
        public IArangoSerializer Serializer { get; set; }
        public IArangoTransport Transport { get; set; }
        public Action<string, IDictionary<string, object>, JToken> QueryProfile { get; set; }
    }
}