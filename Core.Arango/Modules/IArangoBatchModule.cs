using System.Threading.Tasks;

namespace Core.Arango.Modules
{
    public interface IArangoBatchModule
    {
        public ArangoHandle Create(ArangoHandle handle);
        public Task ExecuteAsync(ArangoHandle handle);
    }
}