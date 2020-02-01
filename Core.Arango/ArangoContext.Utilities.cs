using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Core.Arango
{
    public partial class ArangoContext
    {
        private string DbName(string name)
        {
            return UrlEncoder.Default.Encode(_realm + name);
        }

        private static string AddQueryString(
            string uri,
            IEnumerable<KeyValuePair<string, string>> queryString)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            if (queryString == null)
                throw new ArgumentNullException(nameof(queryString));

            var anchorIndex = uri.IndexOf('#');
            var uriToBeAppended = uri;
            var anchorText = "";

            if (anchorIndex != -1)
            {
                anchorText = uri.Substring(anchorIndex);
                uriToBeAppended = uri.Substring(0, anchorIndex);
            }

            var queryIndex = uriToBeAppended.IndexOf('?');
            var hasQuery = queryIndex != -1;

            var sb = new StringBuilder();
            sb.Append(uriToBeAppended);
            foreach (var parameter in queryString)
            {
                sb.Append(hasQuery ? '&' : '?');
                sb.Append(UrlEncoder.Default.Encode(parameter.Key));
                sb.Append('=');
                sb.Append(UrlEncoder.Default.Encode(parameter.Value));
                hasQuery = true;
            }

            sb.Append(anchorText);
            return sb.ToString();
        }

        private async Task<T> SendAsync<T>(HttpMethod m, string url, string body = null, 
            string transaction = null, bool throwOnError = true, bool auth = true)
        {
            var msg = new HttpRequestMessage(m, url)
            {
                Version = HttpVersion.Version11
            };

            msg.Headers.Add(HttpRequestHeader.KeepAlive.ToString(), "true");

            if (auth)
                msg.Headers.Add(HttpRequestHeader.Authorization.ToString(), _auth);

            if (transaction != null)
                msg.Headers.Add("x-arango-trx-id", transaction);

            if (body != null)
                msg.Content = new StringContent(body, Encoding.UTF8, "application/json");
            else
                msg.Headers.Add(HttpRequestHeader.ContentLength.ToString(), "0");

            var res = await HttpClient.SendAsync(msg);

            if (!res.IsSuccessStatusCode)
                if (throwOnError)
                    throw new ArgumentException(await res.Content.ReadAsStringAsync());
                else return default;

            var content = await res.Content.ReadAsStringAsync();

            if (content == "{}")
                return default;

            return JsonConvert.DeserializeObject<T>(content, JsonSerializerSettings);
        }

        private static string Parameterize(FormattableString query, out Dictionary<string, object> parameter)
        {
            var i = 0;
            //var j = 0;

            var set = new Dictionary<object, string>();

            var args = query.GetArguments().Select(x =>
            {
                if (set.TryGetValue(x, out var p))
                    return (object) p;

                p = $"@P{++i}";

                set.Add(x, p);

                return (object) p;
            }).ToArray();

            var queryExp = string.Format(query.Format, args);

            parameter = set.ToDictionary(x => x.Value.Substring(1), x => x.Key);

            return queryExp;
        }
    }
}