using System.Diagnostics.CodeAnalysis;
using Core.Arango.Linq.Attributes;

namespace Core.Arango.Linq
{
    [SuppressMessage("CodeQuality", "IDE0060")]
    public partial class Aql
    {
        /// <summary>
        ///     Calculate the distance between two arbitrary coordinates in meters (as birds would fly). The value is computed
        ///     using the haversine formula, which is based on a spherical Earth model. It’s fast to compute and is accurate to
        ///     around 0.3%, which is sufficient for most use cases such as location-aware services.
        /// </summary>
        [AqlFunction("DISTANCE")]
        public static double Distance(double latitude1, double longitude1, double latitude2, double longitude2)
        {
            throw E;
        }

        /// <summary>
        ///     Checks whether the GeoJSON object geoJsonA fully contains geoJsonB (Every point in B is also in A). The object
        ///     geoJsonA has to be of type Polygon or MultiPolygon, other types are not supported because containment is ill
        ///     defined. This function can be optimized by a S2 based geospatial index.
        /// </summary>
        [AqlFunction("GEO_CONTAINS")]
        public static bool GeoContains(object geoJsonA, object geoJsonB)
        {
            throw E;
        }

        /// <summary>
        ///     Return the distance between two GeoJSON objects, measured from the centroid of each shape.
        /// </summary>
        [AqlFunction("GEO_DISTANCE")]
        public static double GeoDistance(object geoJsonA, object geoJsonB)
        {
            throw E;
        }

        /// <summary>
        ///     Return the distance between two GeoJSON objects, measured from the centroid of each shape.
        /// </summary>
        [AqlFunction("GEO_DISTANCE")]
        public static double GeoDistance(object geoJsonA, object geoJsonB, string ellipsoid)
        {
            throw E;
        }

        /// <summary>
        ///     Return the area for a polygon or multi-polygon on a sphere with the average Earth radius, or an ellipsoid.
        /// </summary>
        [AqlFunction("GEO_AREA")]
        public static double GeoArea(object geoJson)
        {
            throw E;
        }

        /// <summary>
        ///     Return the area for a polygon or multi-polygon on a sphere with the average Earth radius, or an ellipsoid.
        /// </summary>
        [AqlFunction("GEO_AREA")]
        public static double GeoArea(object geoJson, string ellipsoid)
        {
            throw E;
        }

        /// <summary>
        ///     Checks whether two GeoJSON objects are equal or not.
        /// </summary>
        [AqlFunction("GEO_EQUALS")]
        public static bool GeoEquals(object geoJsonA, object geoJsonB)
        {
            throw E;
        }

        /// <summary>
        ///     Checks whether the GeoJSON object geoJsonA intersects with geoJsonB (i.e. at least one point in B is also A or
        ///     vice-versa). This function can be optimized by a S2 based geospatial index.
        /// </summary>
        [AqlFunction("GEO_INTERSECTS")]
        public static bool GeoIntersects(object geoJsonA, object geoJsonB)
        {
            throw E;
        }

        /// <summary>
        ///     Checks whether the distance between two GeoJSON objects lies within a given interval. The distance is measured from
        ///     the centroid of each shape.
        /// </summary>
        [AqlFunction("GEO_IN_RANGE")]
        public static bool GeoInRange(object geoJsonA, object geoJsonB, double low, double high)
        {
            throw E;
        }

        /// <summary>
        ///     Checks whether the distance between two GeoJSON objects lies within a given interval. The distance is measured from
        ///     the centroid of each shape.
        /// </summary>
        [AqlFunction("GEO_IN_RANGE")]
        public static bool GeoInRange(object geoJsonA, object geoJsonB, double low, double high, bool includeLow,
            bool includeHigh)
        {
            throw E;
        }

        /// <summary>
        ///     Construct a GeoJSON LineString. Needs at least two longitude/latitude pairs.
        /// </summary>
        [AqlFunction("GEO_LINESTRING")]
        public static object GeoLineString(object[] value)
        {
            throw E;
        }

        /// <summary>
        ///     Construct a GeoJSON MultiLineString. Needs at least two elements consisting valid LineStrings coordinate arrays.
        /// </summary>
        [AqlFunction("GEO_MULTILINESTRING")]
        public static object GeoMultiLineString(object[] value)
        {
            throw E;
        }

        /// <summary>
        ///     Construct a GeoJSON LineString. Needs at least two longitude/latitude pairs.
        /// </summary>
        [AqlFunction("GEO_MULTIPOINT")]
        public static object GeoMultiPoint(object[] value)
        {
            throw E;
        }

        /// <summary>
        ///     Construct a valid GeoJSON Point.
        /// </summary>
        [AqlFunction("GEO_POINT")]
        public static object GeoPoint(double longitude, double latitude)
        {
            throw E;
        }

        /// <summary>
        ///     Construct a GeoJSON Polygon. Needs at least one array representing a loop. Each loop consists of an array with at
        ///     least three longitude/latitude pairs. The first loop must be the outermost, while any subsequent loops will be
        ///     interpreted as holes.
        /// </summary>
        [AqlFunction("GEO_POLYGON")]
        public static object GeoPolygon(object[] value)
        {
            throw E;
        }

        /// <summary>
        ///     Construct a GeoJSON MultiPolygon. Needs at least two Polygons inside.
        /// </summary>
        [AqlFunction("GEO_MULTIPOLYGON")]
        public static object GeoMultiPolygon(object[] value)
        {
            throw E;
        }
    }
}