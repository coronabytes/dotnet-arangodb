using System.Collections.Generic;

namespace Core.Arango
{
    /// <summary>
    ///     Generic List with fullcount extension
    /// </summary>
    public class ArangoList<T> : List<T>
    {
        /// <summary>
        ///     Actual result count of query ignoring LIMIT clause
        /// </summary>
        public long? FullCount { get; set; }
    }
}