using System.Linq;

namespace Core.Arango.Linq.Interface
{
    public interface IAqlModifiable : IQueryable, IOrderedQueryable
    {
    }

    public interface IAqlModifiable<T> : IAqlModifiable, IQueryable<T>, IOrderedQueryable<T>
    {
    }
}