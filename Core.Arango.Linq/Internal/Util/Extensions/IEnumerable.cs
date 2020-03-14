using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Core.Arango.Linq.Internal.Util.Extensions
{
    internal static class IEnumerableExtensions
    {
        public static bool Any(this IEnumerable src)
        {
            if (src == null) return false;
            foreach (var item in src) return true;
            return false;
        }

        public static bool None(this IEnumerable src)
        {
            return !src.Any();
        }

        public static List<object> ToObjectList(this IEnumerable src)
        {
            return src.Cast<object>().ToList();
        }
    }
}