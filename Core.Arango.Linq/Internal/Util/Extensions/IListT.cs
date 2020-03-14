using System.Collections.Generic;

namespace Core.Arango.Linq.Internal.Util.Extensions
{
    internal static class IListTExtensions
    {
        public static void RemoveLast<T>(this List<T> lst, int count = 1)
        {
            lst.RemoveRange(lst.Count - count, count);
        }
    }
}