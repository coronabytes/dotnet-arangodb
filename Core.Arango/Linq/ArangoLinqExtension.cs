using System.Linq;
using Core.Arango.Linq.Query;

namespace Core.Arango.Linq
{
    public static class ArangoLinqExtension
    {
        public static IQueryable<T> Query<T>(this IArangoContext context)
        {
            var queryParser = new AqlParser(new ArangoLinq(context, null));
            return queryParser.CreateQueryable<T>();
        }

        public static IQueryable<T> Query<T>(this IArangoContext context, ArangoHandle handle)
        {
            var queryParser = new AqlParser(new ArangoLinq(context, handle));
            return queryParser.CreateQueryable<T>();
        }

        public static IQueryable<Aql> Query(this IArangoContext context)
        {
            var queryParser = new AqlParser(new ArangoLinq(context, null));
            return queryParser.CreateQueryable<Aql>();
        }

        public static IQueryable<Aql> Query(this IArangoContext context, ArangoHandle handle)
        {
            var queryParser = new AqlParser(new ArangoLinq(context, handle));
            return queryParser.CreateQueryable<Aql>();
        }
    }
}