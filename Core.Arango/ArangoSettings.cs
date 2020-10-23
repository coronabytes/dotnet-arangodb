using Core.Arango.Serialization;
using Core.Arango.Transport;

namespace Core.Arango
{
    public interface IArangoConfiguration
    {
        int BatchSize { get; set; }
        string Realm { get; set; }
        string Server { get; set; }
        string User { get; set; }
        string Password { get; set; }

        public IArangoSerializer Serializer { get; set; }
        public IArangoTransport Transport { get; set; }
    }

    public class ArangoConfiguration : IArangoConfiguration
    {
        public int BatchSize { get; set; }
        public string Realm { get; set; }
        public string Server { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public IArangoSerializer Serializer { get; set; }
        public IArangoTransport Transport { get; set; }
    }
}