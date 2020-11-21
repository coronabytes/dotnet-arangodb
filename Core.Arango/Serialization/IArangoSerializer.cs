using System;

namespace Core.Arango.Serialization
{
    public interface IArangoSerializer
    {
        public string Serialize(object value);
        public T Deserialize<T>(string value);
        public object Deserialize(string value, Type type);
    }
}