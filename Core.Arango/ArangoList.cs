using System.Collections.Generic;

namespace Core.Arango
{
    public class ArangoList<T> : List<T>
    {
        public long? FullCount { get; set; }
    }
}