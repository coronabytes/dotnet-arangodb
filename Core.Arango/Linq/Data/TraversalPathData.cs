using System.Collections.Generic;
using Core.Arango.Linq.Attributes;
using Core.Arango.Linq.Interface;

namespace Core.Arango.Linq.Data
{
    [CollectionProperty(Naming = NamingConvention.ToCamelCase)]
    public class TraversalPathData<TVertex, TEdge>
    {
        public IList<TVertex> Vertices { get; set; }

        public IList<TEdge> Edges { get; set; }
    }
}