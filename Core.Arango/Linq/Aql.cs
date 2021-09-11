using System;
using System.Diagnostics.CodeAnalysis;
using Core.Arango.Linq.Attributes;

namespace Core.Arango.Linq
{
    /// <summary>
    ///     Built-on AQL functions
    /// </summary>
    [SuppressMessage("CodeQuality", "IDE0060")]
    public partial class Aql
    {
        private static Exception E => new NotImplementedException();

        /// <summary>
        ///     Force return type (not an actual function)
        /// </summary>
        public static T As<T>(object v)
        {
            throw E;
        }

        /// <summary>
        ///     Access to new document version
        /// </summary>
        [AqlFunction("NEW", true)]
        public static T New<T>()
        {
            throw E;
        }

        /// <summary>
        ///     Access to old document version
        /// </summary>
        [AqlFunction("OLD", true)]
        public static T Old<T>()
        {
            throw E;
        }

        /// <summary>
        ///     Calculate the FNV-1A 64 bit hash for text and return it in a hexadecimal string representation.
        /// </summary>
        [AqlFunction("FNV64")]
        public static string Fnv64(object value)
        {
            throw E;
        }
    }
}