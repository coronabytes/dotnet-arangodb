using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Core.Arango.Linq.Attributes;

namespace Core.Arango.Linq
{
    [SuppressMessage("CodeQuality", "IDE0060")]
    public partial class Aql
    {
        /// <summary>
        ///     Take an input value of any type and convert it into the appropriate boolean value.
        /// </summary>
        [AqlFunction("TO_BOOL")]
        public static bool ToBool(object value)
        {
            throw E;
        }

        /// <summary>
        ///     Take an input value of any type and convert it into a numeric value.
        /// </summary>
        [AqlFunction("TO_NUMBER")]
        public static double ToNumber(object value)
        {
            throw E;
        }

        /// <summary>
        ///     Take an input value of any type and convert it into a string value.
        /// </summary>
        [AqlFunction("TO_STRING")]
        public static string ToString(object value)
        {
            throw E;
        }

        /// <summary>
        ///     Take an input value of any type and convert it into an array value.
        /// </summary>
        [AqlFunction("TO_ARRAY")]
        public static IList<T> ToArray<T>(T value)
        {
            throw E;
        }

        /// <summary>
        ///     This is an alias for TO_ARRAY().
        /// </summary>
        [AqlFunction("TO_LIST")]
        public static IList<T> ToList<T>(T value)
        {
            throw E;
        }

        /// <summary>
        ///     Check whether value is null.
        /// </summary>
        [AqlFunction("IS_NULL")]
        public static bool IsNull(object value)
        {
            throw E;
        }

        /// <summary>
        ///     Check whether value is a boolean value
        /// </summary>
        [AqlFunction("IS_BOOL")]
        public static bool IsBool(object value)
        {
            throw E;
        }

        /// <summary>
        ///     Check whether value is a number
        /// </summary>
        [AqlFunction("IS_NUMBER")]
        public static bool IsNumber(object value)
        {
            throw E;
        }

        /// <summary>
        ///     Check whether value is a string
        /// </summary>
        [AqlFunction("IS_STRING")]
        public static bool IsString(object value)
        {
            throw E;
        }

        /// <summary>
        ///     Check whether value is an array / list
        /// </summary>
        [AqlFunction("IS_ARRAY")]
        public static bool IsArray(object value)
        {
            throw E;
        }

        /// <summary>
        ///     This is an alias for IS_ARRAY()
        /// </summary>
        [AqlFunction("IS_LIST")]
        public static bool IsList(object value)
        {
            throw E;
        }

        /// <summary>
        ///     Check whether value is an object / document
        /// </summary>
        [AqlFunction("IS_OBJECT")]
        public static bool IsObject(object value)
        {
            throw E;
        }

        /// <summary>
        ///     This is an alias for IS_OBJECT()
        /// </summary>
        [AqlFunction("IS_DOCUMENT")]
        public static bool IsDocument(object value)
        {
            throw E;
        }

        /// <summary>
        ///     Check whether value is a string that can be used in a date function. This includes partial dates such as “2015” or
        ///     “2015-10” and strings containing properly formatted but invalid dates such as “2015-02-31”.
        /// </summary>
        [AqlFunction("IS_DATESTRING")]
        public static bool IsDateString(object value)
        {
            throw E;
        }

        /// <summary>
        ///     Check whether value is a string that can be used as a document key.
        /// </summary>
        [AqlFunction("IS_KEY")]
        public static bool IsKey(object value)
        {
            throw E;
        }

        /// <summary>
        ///     Check if an arbitrary string is suitable for interpretation as an IPv4 address.
        /// </summary>
        [AqlFunction("IS_IPV4")]
        public static bool IsIpV4(object value)
        {
            throw E;
        }

        /// <summary>
        ///     Return the data type name of value.
        /// </summary>
        [AqlFunction("TYPENAME")]
        public static string Typename(object value)
        {
            throw E;
        }
    }
}