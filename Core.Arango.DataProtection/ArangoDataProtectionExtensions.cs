using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace Core.Arango.DataProtection
{
    public static class ArangoDataProtectionExtensions
    {
        /// <summary>
        /// Stores DataProtection keys in ArangoDB
        /// </summary>
        /// <param name="database">Name of the database suffix</param>
        /// <param name="collection">Name of the collection (will only be created if database didn't exist)</param>
        /// <param name="context">Arango context (if not supplied it will be tried via dependency injection)</param>
        public static IDataProtectionBuilder PersistKeysToArangoDB(this IDataProtectionBuilder builder,
            string database = "dataprotection", string collection = "keys", ArangoContext context = null)
        {
            builder.Services.AddSingleton<IConfigureOptions<KeyManagementOptions>>(services =>
            {
                var loggerFactory = services.GetService<ILoggerFactory>() ?? NullLoggerFactory.Instance;
                return new ConfigureOptions<KeyManagementOptions>(options =>
                {
                    options.XmlRepository = new ArangoXmlRepository(services, loggerFactory, database, collection, context);
                });
            });

            return builder;
        }
    }
}