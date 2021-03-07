using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Core.Arango.Serialization.Newtonsoft
{
    /// <summary>
    ///   Arango Json Serializer with Newtonsoft
    /// </summary>
    public class ArangoNewtonsoftSerializer : IArangoSerializer
    {
        private readonly JsonSerializerSettings _settings;

        /// <summary>
        ///   Arango Json Serializer with Newtonsoft
        /// </summary>
        /// <param name="resolver">PascalCase or camelCaseResolver</param>
        public ArangoNewtonsoftSerializer(IContractResolver resolver)
        {
            _settings = new JsonSerializerSettings
            {
                ContractResolver = resolver,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Formatting = Formatting.None,
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            };
        }

        /// <inheritdoc/>
        public string Serialize(object value)
        {
            return JsonConvert.SerializeObject(value, _settings);
        }

        /// <inheritdoc/>
        public T Deserialize<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value, _settings);
        }

        /// <inheritdoc/>
        public object Deserialize(string v, Type t)
        {
            return JsonConvert.DeserializeObject(v, t, _settings);
        }
    }
}