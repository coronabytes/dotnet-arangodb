using System;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Arango
{
    /// <summary>
    ///     Arango dependency injection for ASP.NET Core
    /// </summary>
    public static class ArangoDependencyInjectionExtension
    {
        /// <summary>
        ///     Add Arango service (singleton)
        /// </summary>
        public static IServiceCollection AddArango(this IServiceCollection collection,
            Func<IServiceProvider, string> configurator)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (configurator == null) throw new ArgumentNullException(nameof(configurator));

            return collection.AddSingleton<IArangoContext>(serviceProvider =>
            {
                var connectionString = configurator(serviceProvider);

                return new ArangoContext(connectionString);
            });
        }

        /// <summary>
        ///     Add Arango service (singleton)
        /// </summary>
        public static IServiceCollection AddArango(this IServiceCollection collection,
            Action<IServiceProvider, IArangoConfiguration> configurator)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (configurator == null) throw new ArgumentNullException(nameof(configurator));

            return collection.AddSingleton<IArangoContext>(serviceProvider =>
            {
                var config = new ArangoConfiguration();
                configurator(serviceProvider, config);
                return new ArangoContext(config);
            });
        }

        /// <summary>
        ///     Add Arango service (singleton)
        /// </summary>
        public static IServiceCollection AddArango(this IServiceCollection collection, string connectionString,
            IArangoConfiguration settings = null)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (connectionString == null) throw new ArgumentNullException(nameof(connectionString));

            return collection.AddSingleton<IArangoContext>(serviceProvider =>
                new ArangoContext(connectionString, settings));
        }
    }
}