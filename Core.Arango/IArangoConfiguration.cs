using System;
using System.Collections.Generic;
using System.Net.Http;
using Core.Arango.Protocol;
using Core.Arango.Serialization;
using Core.Arango.Transport;

namespace Core.Arango
{
    /// <summary>
    ///  Driver configuration
    /// </summary>
    public interface IArangoConfiguration
    {
        /// <summary>
        ///     Arango connection string
        /// </summary>
        /// <example>
        ///     Server=http://localhost:8529;Realm=prod;User=root;Password=;
        /// </example>
        string ConnectionString { get; set; }

        /// <summary>
        ///     Prefixes database names
        /// </summary>
        string Realm { get; set; }

        /// <summary>
        ///     Arango server url
        /// </summary>
        string Server { get; set; }

        /// <summary>
        ///     Arango user
        /// </summary>
        string User { get; set; }

        /// <summary>
        ///     Arango user password
        /// </summary>
        string Password { get; set; }

        /// <summary>
        ///     Default batch size
        /// </summary>
        int BatchSize { get; set; }

        /// <summary>
        ///     Serializer override
        /// </summary>
        IArangoSerializer Serializer { get; set; }

        /// <summary>
        ///     Transport override
        /// </summary>
        IArangoTransport Transport { get; set; }

        /// <summary>
        ///     Callback for each query execute with stats
        /// </summary>
        Action<string, IDictionary<string, object>, ArangoQueryStatistic> QueryProfile { get; set; }

        /// <summary>
        ///   Override HttpClient
        /// </summary>
        HttpClient HttpClient { get; set; }
    }
}