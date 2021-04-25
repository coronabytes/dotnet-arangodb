using Core.Arango.Linq.Collection;

namespace Core.Arango.Linq.Data
{
    public class TraversalEdgeDefinition
    {
        public string CollectionName { get; set; }

        public EdgeDirection? Direction { get; set; }
    }
}