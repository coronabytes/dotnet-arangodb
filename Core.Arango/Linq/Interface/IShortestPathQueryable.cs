using System.Linq;

namespace Core.Arango.Linq.Interface
{
    public interface IShortestPathQueryable : IOrderedQueryable
    {
    }

    public interface IShortestPathQueryable<T> : IShortestPathQueryable, IOrderedQueryable<T>
    {
    }
}