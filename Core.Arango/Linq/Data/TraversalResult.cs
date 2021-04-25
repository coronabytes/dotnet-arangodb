using System.Collections.Generic;
using Core.Arango.Linq.Attributes;
using Core.Arango.Linq.Interface;

namespace Core.Arango.Linq.Data
{
    public class TraversalContainerResult<TVertex, TEdge> : BaseResult
    {
        public TraversalResult<TVertex, TEdge> Result { get; set; }
    }

    public class TraversalResult<TVertex, TEdge> : BaseResult
    {
        public TraversalVisitedResult<TVertex, TEdge> Visited { get; set; }
    }

    public class TraversalVisitedResult<TVertex, TEdge>
    {
        public List<TVertex> Vertices { get; set; }

        public List<TraversalVisitedPathResult<TVertex, TEdge>> Paths { get; set; }
    }

    [CollectionProperty(Naming = NamingConvention.ToCamelCase)]
    public class TraversalVisitedPathResult<TVertex, TEdge>
    {
        public List<TVertex> Vertices { get; set; }

        public List<TEdge> Edges { get; set; }
    }
}