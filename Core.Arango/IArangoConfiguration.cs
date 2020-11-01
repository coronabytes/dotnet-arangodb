using System;
using System.Collections.Generic;
using Core.Arango.Protocol;
using Core.Arango.Serialization;
using Core.Arango.Transport;

namespace Core.Arango
{
    public interface IArangoConfiguration
    {
        string Realm { get; set; }
        string Server { get; set; }
        string User { get; set; }
        string Password { get; set; }

        int BatchSize { get; set; }
        IArangoSerializer Serializer { get; set; }
        IArangoTransport Transport { get; set; }
        Action<string, IDictionary<string, object>, ArangoQueryStatistic> QueryProfile { get; set; }
    }
}