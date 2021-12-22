using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Core.Arango.Serialization.Json
{
    internal class ArangoJsonNullableUnixTimeConverter : JsonConverter<DateTime?>
    {
        public override bool HandleNull => true;

        public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TryGetInt64(out var time))
                return DateTimeOffset.FromUnixTimeMilliseconds(time).UtcDateTime;

            return null;
        }

        public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
        {
            if (value.HasValue)
                writer.WriteNumberValue(((DateTimeOffset)value.Value).ToUnixTimeMilliseconds());
            else
                writer.WriteNullValue();
        }
    }

    internal class ArangoJsonUnixTimeConverter : JsonConverter<DateTime>
    {
        public override bool HandleNull => false;

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TryGetInt64(out var time))
                return DateTimeOffset.FromUnixTimeMilliseconds(time).UtcDateTime;

            return DateTime.MinValue;
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(((DateTimeOffset)value).ToUnixTimeMilliseconds());
        }
    }

    /// <summary>
    ///     Arango Json Serializer with System.Json.Text
    /// </summary>
    public class ArangoJsonSerializer : IArangoSerializer
    {
        /// <summary>
        ///  Use unix timestamps for DateTime
        /// </summary>
        public bool UseTimestamps
        {
            get => _useTimestamps;
            set
            {
                _useTimestamps = value;
                if (value && _options.Converters.All(x => x.GetType() != typeof(ArangoJsonUnixTimeConverter)))
                {
                    _options.Converters.Add(new ArangoJsonUnixTimeConverter());
                    _options.Converters.Add(new ArangoJsonNullableUnixTimeConverter());
                }
            }
        }

        private readonly JsonSerializerOptions _options;
        private bool _useTimestamps;

        /// <summary>
        /// </summary>
        /// <param name="policy">PascalCase or camelCase policy</param>
        public ArangoJsonSerializer(JsonNamingPolicy policy)
        {
            _options = new JsonSerializerOptions(JsonSerializerDefaults.Web)
            {
                PropertyNamingPolicy = policy,
                //DictionaryKeyPolicy = policy,
#if NET6_0_OR_GREATER
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
#else
                IgnoreNullValues = false
#endif
            };

            
        }

        /// <inheritdoc />
        public string Serialize(object value)
        {
            return JsonSerializer.Serialize(value, _options);
        }

        /// <inheritdoc />
        public T Deserialize<T>(string value)
        {
            return JsonSerializer.Deserialize<T>(value, _options);
        }

        /// <inheritdoc />
        public object Deserialize(string v, Type t)
        {
            return JsonSerializer.Deserialize(v, t, _options);
        }
    }
}