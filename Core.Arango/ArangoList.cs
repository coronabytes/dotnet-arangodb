using System.Collections.Generic;

namespace Core.Arango
{
    public class ArangoList<T> : List<T>
    {
        public int? FullCount { get; set; }
    }
}