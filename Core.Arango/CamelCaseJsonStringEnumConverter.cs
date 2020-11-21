using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Core.Arango
{
    public class CamelCaseJsonStringEnumConverter : JsonConverterFactory
    {
        private readonly JsonStringEnumConverter _converter = new JsonStringEnumConverter(JsonNamingPolicy.CamelCase);

        public override bool CanConvert(Type typeToConvert)
        {
            return _converter.CanConvert(typeToConvert);
        }

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            return _converter.CreateConverter(typeToConvert, options);
        }
    }
}