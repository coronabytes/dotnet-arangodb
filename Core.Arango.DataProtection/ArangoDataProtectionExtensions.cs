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
        public static IDataProtectionBuilder PersistKeysToArangoDB(this IDataProtectionBuilder builder,
            string database = "dataprotection", string collection = "keys")
        {
            builder.Services.AddSingleton<IConfigureOptions<KeyManagementOptions>>(services =>
            {
                var loggerFactory = services.GetService<ILoggerFactory>() ?? NullLoggerFactory.Instance;
                return new ConfigureOptions<KeyManagementOptions>(options =>
                {
                    options.XmlRepository = new ArangoXmlRepository(services, loggerFactory, database, collection);
                });
            });

            return builder;
        }
    }
}