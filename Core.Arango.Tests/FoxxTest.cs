using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Arango.Protocol;
using Core.Arango.Tests.Core;
using Xunit;

namespace Core.Arango.Tests
{
    public class FoxxTest : TestBase
    {
        [Fact]
        public async Task InstallScript()
        {
            await SetupAsync("system-camel");

            await Arango.Foxx.InstallServiceAsync("test", "/sample/service", ArangoFoxxSource.FromJavaScript(@"
'use strict';
const createRouter = require('@arangodb/foxx/router');
const router = createRouter();

module.context.use(router);

router.get('/hello-world', function (req, res) {
  res.send('Hello World!');
})
.response(['text/plain'], 'A generic greeting.')
.summary('Generic greeting')
.description('Prints a generic greeting.');
            "));

            var services = await Arango.Foxx.ListServicesAsync("test", true);
            Assert.Single(services);
            Assert.Equal("/sample/service", services.First().Mount);
        }

        [Fact]
        public async Task InstallZip()
        {
            await SetupAsync("system-camel");

            await using var ms = new MemoryStream();
            using (var zip = new ZipArchive(ms, ZipArchiveMode.Create, true, Encoding.UTF8))
            {
                await using (var manifest = zip.CreateEntry("manifest.json").Open())
                {
                    await manifest.WriteAsync(Encoding.UTF8.GetBytes(@"
{
  ""$schema"": ""http://json.schemastore.org/foxx-manifest"",
  ""name"": ""SampleService"",
  ""description"": ""test"",
  ""version"": ""1.0.0"",
  ""license"": ""MIT"",
  ""engines"": {
    ""arangodb"": ""^3.0.0""
  },
  ""main"": ""index.js"",
  ""configuration"": {
    ""currency"": {
      ""description"": ""Currency symbol to use for prices in the shop."",
      ""default"": ""$"",
      ""type"": ""string""
    },
      ""secretKey"": {
      ""description"": ""Secret key to use for signing session tokens."",
      ""type"": ""password""
    }
  }
}
"));
                }

                await using (var readme = zip.CreateEntry("README").Open())
                {
                    await readme.WriteAsync(Encoding.UTF8.GetBytes(@"
TEST
"));
                }

                await using (var index = zip.CreateEntry("index.js").Open())
                {
                    await index.WriteAsync(Encoding.UTF8.GetBytes(@"
'use strict';
const createRouter = require('@arangodb/foxx/router');
const router = createRouter();

module.context.use(router);
router.get('/hello-world', function (req, res) {
  res.send('Hello World!');
})
.response(['text/plain'], 'A generic greeting.')
.summary('Generic greeting')
.description('Prints a generic greeting.');
"));
                }
            }

            ms.Position = 0;

            await Arango.Foxx.InstallServiceAsync("test", "/sample/service", ArangoFoxxSource.FromZip(ms));

            var services = await Arango.Foxx.ListServicesAsync("test", true);

            Assert.Single(services);
            Assert.Equal("/sample/service", services.First().Mount);
        }
    }
}