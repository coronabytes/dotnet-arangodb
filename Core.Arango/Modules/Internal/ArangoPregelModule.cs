using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Core.Arango.Protocol;
using Core.Arango.Protocol.Internal;

namespace Core.Arango.Modules.Internal
{
    internal class ArangoPregelModule : ArangoModule, IArangoPregelModule
    {
        internal ArangoPregelModule(IArangoContext context) : base(context)
        {
        }

        public Task<long> StartJobAsync(ArangoHandle database, ArangoPregel job)
        {
            throw new System.NotImplementedException();
        }

        public Task<ArangoPregelStatus> GetJobStatusAsync(ArangoHandle database, long id)
        {
            throw new System.NotImplementedException();
        }

        public Task DeleteJobAsync(ArangoHandle database, long id)
        {
            throw new System.NotImplementedException();
        }
    }
}