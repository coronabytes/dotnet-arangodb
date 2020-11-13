using System;
using System.Runtime.InteropServices.ComTypes;
using System.Text.Json;

namespace Core.Arango.Serialization
{
    public class ArangoSystemTextJsonSerializer : IArangoSerializer
    {
        protected JsonSerializerOptions Options;

        public ArangoSystemTextJsonSerializer()
        {
            Options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
            Options.PropertyNamingPolicy = new ArangoSystemTextJsonNamingPolicy();
        }

        public string Serialize(object value)
        {
            return System.Text.Json.JsonSerializer.Serialize(value, Options);
        }

        public T Deserialize<T>(string value)
        {
            return System.Text.Json.JsonSerializer.Deserialize<T>(value, Options);
        }

        public object Deserialize(string v, Type t)
        {
            return System.Text.Json.JsonSerializer.Deserialize(v, t, Options);
        }
    }
}