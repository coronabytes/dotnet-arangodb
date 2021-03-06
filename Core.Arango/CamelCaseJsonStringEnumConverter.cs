using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Core.Arango
{
    /// <summary>
    ///   System.Text.Json camelCase enums
    /// </summary>
    public class CamelCaseJsonStringEnumConverter : JsonConverterFactory
    {
        private readonly JsonStringEnumConverter _converter = new JsonStringEnumConverter(JsonNamingPolicy.CamelCase);

        /// <inheritdoc/>
        public override bool CanConvert(Type typeToConvert)
        {
            return _converter.CanConvert(typeToConvert);
        }

        /// <inheritdoc/>
        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            return _converter.CreateConverter(typeToConvert, options);
        }
    }
}