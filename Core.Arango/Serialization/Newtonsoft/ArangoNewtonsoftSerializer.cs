using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Core.Arango.Serialization.Newtonsoft
{
    public class ArangoNewtonsoftSerializer : IArangoSerializer
    {
        protected JsonSerializerSettings Settings;

        public ArangoNewtonsoftSerializer(IContractResolver resolver)
        {
            Settings = new JsonSerializerSettings
            {
                ContractResolver = resolver,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Formatting = Formatting.None,
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            };
        }

        public string Serialize(object value)
        {
            return JsonConvert.SerializeObject(value, Settings);
        }

        public T Deserialize<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value, Settings);
        }

        public object Deserialize(string v, Type t)
        {
            return JsonConvert.DeserializeObject(v, t, Settings);
        }
    }
}