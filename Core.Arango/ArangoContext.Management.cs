using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Core.Arango.Protocol;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Core.Arango
{
    public partial class ArangoContext
    {
        public async Task<bool> CreateDatabaseAsync(ArangoHandle name, CancellationToken cancellationToken = default)
        {
            var res = await SendAsync<JObject>(HttpMethod.Post,
                $"{Server}/_db/_system/_api/database",
                JsonConvert.SerializeObject(new
                {
                    name = Realm + name
                }), throwOnError: false, cancellationToken: cancellationToken);

            return res != null;
        }

        public async Task<List<string>> ListDatabasesAsync(CancellationToken cancellationToken = default)
        {
            var res = await SendAsync<QueryResponse<string>>(HttpMethod.Get,
                $"{Server}/_db/_system/_api/database", cancellationToken: cancellationToken);

            return res.Result
                .Where(x => x.StartsWith(Realm))
                .Select(x => x.Substring(Realm.Length))
                .ToList();
        }

        public async Task<bool> ExistDatabaseAsync(ArangoHandle name, CancellationToken cancellationToken = default)
        {
            var dbs = await ListDatabasesAsync(cancellationToken);

            return dbs.Contains(name);
        }

        public async Task DropDatabaseAsync(ArangoHandle name, CancellationToken cancellationToken = default)
        {
            await SendAsync<JObject>(HttpMethod.Delete,
                $"{Server}/_db/_system/_api/database/{DbName(name)}", null,
                throwOnError: false, cancellationToken: cancellationToken);
        }

        public async Task EnsureIndexAsync(ArangoHandle database, string collection, ArangoIndex request,
            CancellationToken cancellationToken = default)
        {
            await SendAsync<JObject>(HttpMethod.Post,
                $"{Server}/_db/{DbName(database)}/_api/index?collection={collection}",
                JsonConvert.SerializeObject(request), cancellationToken: cancellationToken);
        }

        public async Task CreateViewAsync(ArangoHandle database, ArangoView view,
            CancellationToken cancellationToken = default)
        {
            await SendAsync<JObject>(HttpMethod.Post,
                $"{Server}/_db/{DbName(database)}/_api/view",
                JsonConvert.SerializeObject(view),
                cancellationToken: cancellationToken);
        }

        public async Task<List<string>> ListViewsAsync(ArangoHandle database,
            CancellationToken cancellationToken = default)
        {
            var res = await SendAsync<JObject>(HttpMethod.Get,
                $"{Server}/_db/{DbName(database)}/_api/view",
                cancellationToken: cancellationToken);
            return res.GetValue("result")
                .Select(x => x.Value<string>("name")).ToList();
        }

        public async Task DropViewAsync(ArangoHandle database,
            string name,
            CancellationToken cancellationToken = default)
        {
            await SendAsync<JObject>(HttpMethod.Delete,
                $"{Server}/_db/{DbName(database)}/_api/view/{UrlEncoder.Default.Encode(name)}",
                cancellationToken: cancellationToken);
        }

        public async Task DropViewsAsync(ArangoHandle database, CancellationToken cancellationToken = default)
        {
            var views = await ListViewsAsync(database, cancellationToken);

            foreach (var view in views)
                await DropViewAsync(database, view, cancellationToken);
        }

        [Obsolete("use ArangoContext.Graph.ListAsync")]
        public async Task<List<string>> ListGraphAsync(ArangoHandle database,
            CancellationToken cancellationToken = default)
        {
            return await Graph.ListAsync(database, cancellationToken);
        }

        [Obsolete("use ArangoContext.Graph.CreateAsync")]
        public async Task CreateGraphAsync(ArangoHandle database, ArangoGraph request,
            CancellationToken cancellationToken = default)
        {
            await Graph.CreateAsync(database, request, cancellationToken);
        }

        [Obsolete("use ArangoContext.Graph.DropAsync")]
        public async Task DropGraphAsync(ArangoHandle database, string name,
            CancellationToken cancellationToken = default)
        {
            await Graph.DropAsync(database, name, cancellationToken);
        }

        [Obsolete("use ArangoContext.Collection.CreateAsync")]
        public async Task CreateCollectionAsync(ArangoHandle database, string collection, ArangoCollectionType type,
            CancellationToken cancellationToken = default)
        {
            await Collection.CreateAsync(database, collection, type, cancellationToken);
        }

        [Obsolete("use ArangoContext.Collection.CreateAsync")]
        public async Task CreateCollectionAsync(ArangoHandle database, ArangoCollection collection,
            CancellationToken cancellationToken = default)
        {
            await Collection.CreateAsync(database, collection, cancellationToken);
        }

        [Obsolete("use ArangoContext.Collection.TruncateAsync")]
        public async Task TruncateCollectionAsync(ArangoHandle database, string collection,
            CancellationToken cancellationToken = default)
        {
            await Collection.TruncateAsync(database, collection, cancellationToken);
        }

        [Obsolete("use ArangoContext.Collection.ListAsync")]
        public async Task<List<string>> ListCollectionsAsync(ArangoHandle database,
            CancellationToken cancellationToken = default)
        {
            return await Collection.ListAsync(database, cancellationToken);
        }

        /// <summary>
        ///     Ignores primary and edge indices
        /// </summary>
        public async Task<List<string>> ListIndicesAsync(ArangoHandle database, string collection,
            CancellationToken cancellationToken = default)
        {
            var res = await SendAsync<JObject>(HttpMethod.Get,
                $"{Server}/_db/{DbName(database)}/_api/index?collection={UrlEncoder.Default.Encode(collection)}",
                cancellationToken: cancellationToken);
            return res.GetValue("indexes")
                .Where(x =>
                {
                    var type = x.Value<string>("type");
                    return type != "primary" && type != "edge";
                })
                .Select(x => x.Value<string>("id")).ToList();
        }

        public async Task DropIndexAsync(ArangoHandle database, string index,
            CancellationToken cancellationToken = default)
        {
            await SendAsync<JObject>(HttpMethod.Delete,
                $"{Server}/_db/{DbName(database)}/_api/index/{index}", cancellationToken: cancellationToken);
        }

        public async Task<Version> GetVersionAsync(CancellationToken cancellationToken = default)
        {
            var res = await SendAsync<JObject>(HttpMethod.Get, $"{Server}/_db/_system/_api/version",
                cancellationToken: cancellationToken);
            return Version.Parse(res.Value<string>("version"));
        }

        public async Task DropCollectionAsync(ArangoHandle database, string collection,
            CancellationToken cancellationToken = default)
        {
            await SendAsync<JObject>(HttpMethod.Delete,
                $"{Server}/_db/{DbName(database)}/_api/collection/{UrlEncoder.Default.Encode(collection)}",
                cancellationToken: cancellationToken);
        }

        /// <summary>
        ///     Drops all user created indices over all collections in database
        /// </summary>
        public async Task DropIndicesAsync(ArangoHandle database, CancellationToken cancellationToken = default)
        {
            var collections = await Collection.ListAsync(database, cancellationToken);

            foreach (var col in collections)
            {
                var indices = await ListIndicesAsync(database, col, cancellationToken);

                foreach (var idx in indices) await DropIndexAsync(database, idx, cancellationToken);
            }
        }

        public async Task<List<ArangoAnalyzer>> ListAnalyzersAsync(ArangoHandle database,
            CancellationToken cancellationToken = default)
        {
            return (await SendAsync<QueryResponse<ArangoAnalyzer>>(HttpMethod.Get,
                $"{Server}/_db/{DbName(database)}/_api/analyzer",
                cancellationToken: cancellationToken)).Result;
        }

        public async Task CreateAnalyzerAsync(ArangoHandle database,
            ArangoAnalyzer analyzer,
            CancellationToken cancellationToken = default)
        {
            await SendAsync<QueryResponse<JObject>>(HttpMethod.Post,
                $"{Server}/_db/{DbName(database)}/_api/analyzer",
                JsonConvert.SerializeObject(analyzer, JsonSerializerSettings),
                cancellationToken: cancellationToken);
        }

        public async Task DeleteAnalyzerAsync(ArangoHandle database,
            string analyzer, bool force = false,
            CancellationToken cancellationToken = default)
        {
            await SendAsync<QueryResponse<JObject>>(HttpMethod.Delete,
                $"{Server}/_db/{DbName(database)}/_api/analyzer/{UrlEncoder.Default.Encode(analyzer)}?force={(force ? "true" : "false")}",
                cancellationToken: cancellationToken);
        }
    }
}