using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Arango.Protocol;

namespace Core.Arango.Modules
{
    public interface IArangoCollectionModule
    {
        Task CreateAsync(ArangoHandle database, ArangoCollection collection,
            CancellationToken cancellationToken = default);

        Task CreateAsync(ArangoHandle database, string collection, ArangoCollectionType type,
            CancellationToken cancellationToken = default);

        Task<List<string>> ListAsync(ArangoHandle database, CancellationToken cancellationToken = default);

        Task RenameAsync(ArangoHandle database, string oldname, string newname,
            CancellationToken cancellationToken = default);

        Task TruncateAsync(ArangoHandle database, string collection, CancellationToken cancellationToken = default);

        Task UpdateAsync(ArangoHandle database, string collection, ArangoCollectionUpdate update,
            CancellationToken cancellationToken = default);
    }
}