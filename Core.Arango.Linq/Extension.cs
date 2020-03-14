using System.Linq;

namespace Core.Arango.Linq
{
    public static class ArangoLinqExtension
    {
        public static IQueryable<T> AsQueryable<T>(this ArangoContext arango, ArangoHandle db, string collection = null)
        {
            return new ArangoQueryableContext<T>(arango, db, collection ?? typeof(T).Name);
        }
    }
}
