using System;
using System.Diagnostics.CodeAnalysis;
using Core.Arango.Linq.Attributes;

namespace Core.Arango.Linq
{
    /// <summary>
    /// Built-on AQL functions
    /// </summary>
    [SuppressMessage("CodeQuality", "IDE0060")]
    public partial class Aql
    {
        private static Exception E => new NotImplementedException();

        [AqlFunction("NEW", true)]
        public static T New<T>()
        {
            throw E;
        }

        [AqlFunction("OLD", true)]
        public static T Old<T>()
        {
            throw E;
        }

        [AqlFunction("FNV64")]
        public static string Fnv64(object value)
        {
            throw E;
        }
    }
}