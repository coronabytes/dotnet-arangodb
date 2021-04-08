using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Core.Arango.Serialization.Json;
using Newtonsoft.Json.Converters;

namespace Core.Arango.Protocol
{
    /// <summary>
    ///     ArangoGeoJsonType
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    [Newtonsoft.Json.JsonConverter(typeof(StringEnumConverter))]
    public enum ArangoAnalyzerGeoJsonType
    {
        /// <summary>
        ///     index all GeoJSON geometry types (Point, Polygon etc.)
        /// </summary>
        [EnumMember(Value = "shape")] Shape,

        /// <summary>
        ///     compute and only index the centroid of the input geometry
        /// </summary>
        [EnumMember(Value = "centroid")] Centroid,

        /// <summary>
        ///     only index GeoJSON objects of type Point, ignore all other geometry types
        /// </summary>
        [EnumMember(Value = "point")] Point
    }
}