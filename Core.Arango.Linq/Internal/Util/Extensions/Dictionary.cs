using System.Collections.Generic;

namespace Core.Arango.Linq.Internal.Util.Extensions
{
    internal static class Dictionary
    {
        public static void AddRange<TKey, TValue>(this IDictionary<TKey, TValue> dict, IEnumerable<(TKey, TValue)> src)
        {
            src.ForEach(t => dict.Add(t.Item1, t.Item2));
        }
    }
}