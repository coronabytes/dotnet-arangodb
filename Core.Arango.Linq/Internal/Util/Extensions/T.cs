using System.Collections.Generic;
using System.Linq;

namespace Core.Arango.Linq.Internal.Util.Extensions
{
    internal static class TExtensions
    {
        public static bool In<T>(this T val, IEnumerable<T> vals)
        {
            return vals.Contains(val);
        }

        public static bool In<T>(this T val, params T[] vals)
        {
            return vals.Contains(val);
        }

        public static bool In(this char c, string s)
        {
            return s.IndexOf(c) > -1;
        }

        public static bool In<T>(this T val, HashSet<T> vals)
        {
            return vals.Contains(val);
        }

        public static bool NotIn<T>(this T val, IEnumerable<T> vals)
        {
            return !vals.Contains(val);
        }

        public static bool NotIn<T>(this T val, params T[] vals)
        {
            return !vals.Contains(val);
        }

        public static bool NotIn(this char c, string s)
        {
            return s.IndexOf(c) == -1;
        }

        public static bool NotIn<T>(this T val, HashSet<T> vals)
        {
            return !vals.Contains(val);
        }
    }
}