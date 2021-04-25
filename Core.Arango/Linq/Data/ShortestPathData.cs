using Core.Arango.Linq.Attributes;
using Core.Arango.Linq.Interface;

namespace Core.Arango.Linq.Data
{
    [CollectionProperty(Naming = NamingConvention.ToCamelCase)]
    public class ShortestPathData<TVertex, TEdge>
    {
        public TVertex Vertex { get; set; }

        public TEdge Edge { get; set; }
    }
}