using System.Linq;

namespace Core.Arango.Linq.Interface
{
    public interface ITraversalQueryable : IOrderedQueryable
    {
    }

    public interface ITraversalQueryable<T> : ITraversalQueryable, IOrderedQueryable<T>
    {
    }
}