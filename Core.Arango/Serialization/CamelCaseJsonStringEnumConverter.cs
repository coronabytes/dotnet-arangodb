using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Core.Arango.Serialization
{
    public class CamelCaseJsonStringEnumConverter : JsonConverterFactory
    {
        JsonStringEnumConverter bla = new JsonStringEnumConverter(JsonNamingPolicy.CamelCase);

        public override bool CanConvert(Type typeToConvert)
        {
            return bla.CanConvert(typeToConvert);
        }

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            return bla.CreateConverter(typeToConvert, options);
        }
    }
}