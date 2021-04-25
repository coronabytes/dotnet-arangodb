using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Core.Arango.Linq.Query
{
    internal class Grouping<TKey, TElement> : IGrouping<TKey, TElement>
    {
        public IEnumerable<TElement> Elements;

        public Grouping(TKey Key)
        {
            this.Key = Key;
            Elements = new List<TElement>();
        }

        public TKey Key { get; }

        public IEnumerator<TElement> GetEnumerator()
        {
            return Elements.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}