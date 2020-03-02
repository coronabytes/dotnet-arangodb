using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Core.Arango.Protocol;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Core.Arango.DataProtection
{
    internal class ArangoXmlRepository : IXmlRepository
    {
        private readonly IServiceProvider _services;
        private readonly string _database;
        private readonly string _collection;
        private readonly ILogger _logger;
        public ArangoXmlRepository(IServiceProvider services, ILoggerFactory loggerFactory, 
            string database, string collection)
        {
            if (loggerFactory == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }

            _logger = loggerFactory.CreateLogger<ArangoXmlRepository>();
            _services = services ?? throw new ArgumentNullException(nameof(services));
            _database = database;
            _collection = collection;

            try
            {
                using var scope = _services.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<ArangoContext>();

                if (!context.ExistDatabaseAsync(_database).Result)
                {
                    context.CreateDatabaseAsync(_database).Wait();
                    context.CreateCollectionAsync(_database, _collection, ArangoCollectionType.Document).Wait();
                }
            }
            catch (Exception e)
            {
                var logger = _logger;
                logger?.LogError(e, e.Message);
            }
        }

        /// <inheritdoc />
        public virtual IReadOnlyCollection<XElement> GetAllElements()
        {
            using var scope = _services.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<ArangoContext>();
            var logger = _logger;

            return context.FindAsync<DataProtectionEntity>(_database, _collection, $"true").Result
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

            context.CreateDocumentAsync(_database, _collection, newKey).Wait();
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