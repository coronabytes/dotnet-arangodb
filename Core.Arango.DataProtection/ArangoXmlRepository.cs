using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Core.Arango.DataProtection
{
    internal class ArangoXmlRepository : IXmlRepository
    {
        private readonly IServiceProvider _services;
        private readonly ILogger _logger;
        public ArangoXmlRepository(IServiceProvider services, ILoggerFactory loggerFactory)
        {
            if (loggerFactory == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }

            _logger = loggerFactory.CreateLogger<ArangoXmlRepository>();
            _services = services ?? throw new ArgumentNullException(nameof(services));
        }

        /// <inheritdoc />
        public virtual IReadOnlyCollection<XElement> GetAllElements()
        {
            using var scope = _services.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<ArangoContext>();
            var logger = _logger;

            return context.FindAsync<DataProtectionEntity>("keys", "Keys", $"true").Result
                .Select(key => TryParseKeyXml(key.Xml, logger))
                .ToList().AsReadOnly();
        }

        /// <inheritdoc />
        public void StoreElement(XElement element, string friendlyName)
        {
            using var scope = _services.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<ArangoContext>();
            var newKey = new DataProtectionEntity
            {
                FriendlyName = friendlyName,
                Xml = element.ToString(SaveOptions.DisableFormatting)
            };

            context.CreateDocumentAsync("keys", "keys", newKey).Wait();
        }

        private static XElement TryParseKeyXml(string xml, ILogger logger)
        {
            try
            {
                return XElement.Parse(xml);
            }
            catch (Exception e)
            {
                logger?.LogError(e, e.Message);
                return null;
            }
        }
    }
}