using System;
using System.Text.Json;

namespace Core.Arango.Serialization.System
{
    public class ArangoJsonSerializer : IArangoSerializer
    {
        protected JsonSerializerOptions Options;

        public ArangoJsonSerializer(JsonNamingPolicy policy)
        {
            Options = new JsonSerializerOptions(JsonSerializerDefaults.Web)
            {
                PropertyNamingPolicy = policy,
                //DictionaryKeyPolicy = policy,
                IgnoreNullValues = false
            };
        }

        public string Serialize(object value)
        {
            return JsonSerializer.Serialize(value, Options);
        }

        public T Deserialize<T>(string value)
        {
            return JsonSerializer.Deserialize<T>(value, Options);
        }

        public object Deserialize(string v, Type t)
        {
            return JsonSerializer.Deserialize(v, t, Options);
        }
    }
}