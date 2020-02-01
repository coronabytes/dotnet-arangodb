using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Core.Arango.Protocol;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Core.Arango
{
    public partial class ArangoContext
    {
        public async Task<bool> CreateDatabaseAsync(ArangoHandle name)
        {
            var res = await SendAsync<JObject>(HttpMethod.Post,
                $"{_server}/_db/_system/_api/database",
                JsonConvert.SerializeObject(new
                {
                    name = _realm + name
                }), throwOnError: false);

            return res != null;
        }

        public async Task<List<string>> ListDatabasesAsync()
        {
            var res = await SendAsync<QueryResponse<string>>(HttpMethod.Get,
                $"{_server}/_db/_system/_api/database");

            return res.Result
                .Where(x => x.StartsWith(_realm))
                .Select(x => x.Substring(_realm.Length))
                .ToList();
        }

        public async Task<bool> ExistDatabaseAsync(ArangoHandle name)
        {
            var dbs = await ListDatabasesAsync();

            return dbs.Contains(name);
        }

        public async Task DropDatabaseAsync(ArangoHandle name)
        {
            await SendAsync<JObject>(HttpMethod.Delete,
                $"{_server}/_db/_system/_api/database/{DbName(name)}", null, throwOnError: false);
        }

        public async Task EnsureIndexAsync(ArangoHandle database, string collection, ArangoIndex request)
        {
            var res = await SendAsync<JObject>(HttpMethod.Post,
                $"{_server}/_db/{DbName(database)}/_api/index?collection={collection}",
                JsonConvert.SerializeObject(request));
        }

        public async Task CreateViewAsync(ArangoHandle database, ArangoView view)
        {
            var res = await SendAsync<JObject>(HttpMethod.Post,
                $"{_server}/_db/{DbName(database)}/_api/view",
                JsonConvert.SerializeObject(view));
        }

        public async Task<List<string>> ListGraphAsync(ArangoHandle database)
        {
            var res = await SendAsync<GraphResponse<JObject>>(HttpMethod.Get,
                $"{_server}/_db/{DbName(database)}/_api/gharial");
            return res.Graphs.Select(x => x.Value<string>("_key")).ToList();
        }

        public async Task CreateGraphAsync(ArangoHandle database, ArangoGraph request)
        {
            var res = await SendAsync<JObject>(HttpMethod.Post,
                $"{_server}/_db/{DbName(database)}/_api/gharial",
                JsonConvert.SerializeObject(request));
        }

        public async Task DropGraphAsync(ArangoHandle database, string name)
        {
            var res = await SendAsync<JObject>(HttpMethod.Delete,
                $"{_server}/_db/{DbName(database)}/_api/gharial/{UrlEncoder.Default.Encode(name)}");
        }

        public async Task CreateCollectionAsync(ArangoHandle database, string collection, ArangoCollectionType type)
        {
            await SendAsync<JObject>(HttpMethod.Post,
                $"{_server}/_db/{DbName(database)}/_api/collection",
                JsonConvert.SerializeObject(new CollectionCreateRequest
                {
                    Name = collection,
                    Type = (int) type
                }, JsonSerializerSettings));
        }

        public async Task TruncateCollectionAsync(ArangoHandle database, string collection)
        {
            await SendAsync<JObject>(HttpMethod.Put,
                $"{_server}/_db/{DbName(database)}/_api/collection/{UrlEncoder.Default.Encode(collection)}/truncate");
        }

        public async Task<List<string>> ListCollectionsAsync(ArangoHandle database)
        {
            var res = await SendAsync<QueryResponse<JObject>>(HttpMethod.Get,
                $"{_server}/_db/{DbName(database)}/_api/collection?excludeSystem=true");
            return res.Result.Select(x => x.Value<string>("name")).ToList();
        }

        /// <summary>
        ///     Ignores primary and edge indices
        /// </summary>
        /// <returns></returns>
        public async Task<List<string>> ListIndicesAsync(ArangoHandle database, string collection)
        {
            var res = await SendAsync<JObject>(HttpMethod.Get,
                $"{_server}/_db/{DbName(database)}/_api/index?collection={UrlEncoder.Default.Encode(collection)}");
            return res.GetValue("indexes")
                .Where(x =>
                {
                    var type = x.Value<string>("type");
                    return type != "primary" && type != "edge";
                })
                .Select(x => x.Value<string>("id")).ToList();
        }

        public async Task DropIndexAsync(ArangoHandle database, string index)
        {
            await SendAsync<JObject>(HttpMethod.Delete,
                $"{_server}/_db/{DbName(database)}/_api/index/{index}");
        }

        public async Task<Version> GetVersionAsync()
        {
            var res = await SendAsync<JObject>(HttpMethod.Get, $"{_server}/_db/_system/_api/version");
            return Version.Parse(res.Value<string>("version"));
        }

        public async Task DropCollectionAsync(ArangoHandle database, string collection)
        {
            await SendAsync<JObject>(HttpMethod.Delete,
                $"{_server}/_db/{DbName(database)}/_api/collection/{UrlEncoder.Default.Encode(collection)}");
        }

        public async Task DropIndices(ArangoHandle database)
        {
            var collections = await ListCollectionsAsync(database);

            foreach (var col in collections)
            {
                var indices = await ListIndicesAsync(database, col);

                foreach (var idx in indices) await DropIndexAsync(database, idx);
            }
        }
    }
}