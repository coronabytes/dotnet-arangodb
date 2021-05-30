using System;
using System.Linq;
using Core.Arango.Linq.Query;

namespace Core.Arango.Linq
{
    /// <summary>
    /// LINQ Extensions
    /// </summary>
    public static class ArangoContextExtension
    {
        /// <summary>
        ///  LINQ in sub-expression
        /// </summary>
        [Obsolete("Experimental")]
        public static IQueryable<T> Query<T>(this IArangoContext context)
        {
            var queryParser = new AqlParser(new ArangoLinq(context, null));
            return queryParser.CreateQueryable<T>();
        }

        /// <summary>
        ///  LINQ on database
        /// </summary>
        [Obsolete("Experimental")]
        public static IQueryable<T> Query<T>(this IArangoContext context, ArangoHandle handle)
        {
            var queryParser = new AqlParser(new ArangoLinq(context, handle));
            return queryParser.CreateQueryable<T>();
        }

        /// <summary>
        ///  LINQ in sub-expression
        /// </summary>
        [Obsolete("Experimental")]
        public static IQueryable<Aql> Query(this IArangoContext context)
        {
            var queryParser = new AqlParser(new ArangoLinq(context, null));
            return queryParser.CreateQueryable<Aql>();
        }

        /// <summary>
        ///  LINQ on database
        /// </summary>
        [Obsolete("Experimental")]
        public static IQueryable<Aql> Query(this IArangoContext context, ArangoHandle handle)
        {
            var queryParser = new AqlParser(new ArangoLinq(context, handle));
            return queryParser.CreateQueryable<Aql>();
        }
    }
}