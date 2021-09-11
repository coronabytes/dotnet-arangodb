using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Core.Arango.Serialization.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Core.Arango.Modules
{
    public class ArangoPregel
    {
        /// <summary>
        ///     Name of the algorithm
        /// </summary>
        [JsonPropertyName("algorithm")]
        [JsonProperty(PropertyName = "algorithm")]
        public string Algorithm { get; set; }

        /// <summary>
        ///     Name of a graph. Either this or the parameters vertexCollections and edgeCollections are required. Please note that
        ///     there are special sharding requirements for graphs in order to be used with Pregel.
        /// </summary>
        [JsonPropertyName("graphName")]
        [JsonProperty(PropertyName = "graphName")]
        public string GraphName { get; set; }

        /// <summary>
        ///     List of vertex collection names. Please note that there are special sharding requirements for collections in order
        ///     to be used with Pregel.
        /// </summary>
        [JsonPropertyName("vertexCollections")]
        [JsonProperty(PropertyName = "vertexCollections")]
        public ICollection<string> VertexCollections { get; set; }

        /// <summary>
        ///     List of edge collection names. Please note that there are special sharding requirements for collections in order to
        ///     be used with Pregel.
        /// </summary>
        [JsonPropertyName("edgeCollections")]
        [JsonProperty(PropertyName = "edgeCollections")]
        public ICollection<string> EdgeCollections { get; set; }

        /// <summary>
        ///     General as well as algorithm-specific options.
        /// </summary>
        [JsonPropertyName("params")]
        [JsonProperty(PropertyName = "params")]
        public IDictionary<string, object> Params { get; set; }
    }

    /// <summary>
    ///     Pregel State
    /// </summary>
    [System.Text.Json.Serialization.JsonConverter(typeof(JsonStringEnumMemberConverter))]
    [Newtonsoft.Json.JsonConverter(typeof(StringEnumConverter))]
    public enum ArangoPregelState
    {
        /// <summary>
        ///     Algorithm is executing normally.
        /// </summary>
        [EnumMember(Value = "running")] Running,

        /// <summary>
        ///     The algorithm finished, but the results are still being written back into the collections. Occurs only if the store
        ///     parameter is set to true.
        /// </summary>
        [EnumMember(Value = "storing")] Storing,

        /// <summary>
        ///     The execution is done. In version 3.7.1 and later, this means that storing is also done. In earlier versions, the
        ///     results may not be written back into the collections yet. This event is announced in the server log (requires at
        ///     least info log level for the pregel log topic).
        /// </summary>
        [EnumMember(Value = "done")] Done,

        /// <summary>
        ///     The execution was permanently canceled, either by the user or by an error.
        /// </summary>
        [EnumMember(Value = "canceled")] Canceled,

        /// <summary>
        ///     The execution has failed and cannot recover.
        /// </summary>
        [EnumMember(Value = "fatal error")] FatalError,

        /// <summary>
        ///     The execution is in an error state. This can be caused by DB-Servers being not reachable or being non responsive.
        ///     The execution might recover later, or switch to "canceled" if it was not able to recover successfully.
        /// </summary>
        [EnumMember(Value = "in error")] InError,

        /// <summary>
        ///     The execution is actively recovering, will switch back to running if the recovery was successful.
        /// </summary>
        [EnumMember(Value = "recovering")] Recovering
    }

    public class ArangoPregelStatus
    {
        /// <summary>
        ///     State of the execution.
        /// </summary>
        [JsonPropertyName("state")]
        [JsonProperty(PropertyName = "state")]
        public ArangoPregelState State { get; set; }

        /// <summary>
        ///     The number of global supersteps executed.
        /// </summary>
        [JsonPropertyName("gss")]
        [JsonProperty(PropertyName = "gss")]
        public long Gss { get; set; }

        /// <summary>
        ///     Total runtime of the execution up to now (if the execution is still ongoing).
        /// </summary>
        [JsonPropertyName("totalRuntime")]
        [JsonProperty(PropertyName = "totalRuntime")]
        public double TotalRuntime { get; set; }

        /// <summary>
        ///     Startup runtime of the execution. The startup time includes the data loading time and can be substantial. The
        ///     startup time will be reported as 0 if the startup is still ongoing.
        /// </summary>
        [JsonPropertyName("startupTime")]
        [JsonProperty(PropertyName = "startupTime")]
        public double StartupTime { get; set; }

        /// <summary>
        ///     Algorithm execution time. The computation time will be reported as 0 if the computation still ongoing.
        /// </summary>
        [JsonPropertyName("computationTime")]
        [JsonProperty(PropertyName = "computationTime")]
        public double ComputationTime { get; set; }

        /// <summary>
        ///     Time for storing the results if the job includes results storage. The storage time be reported as 0 if storing the
        ///     results is still ongoing.
        /// </summary>
        [JsonPropertyName("storageTime")]
        [JsonProperty(PropertyName = "storageTime")]
        public double StorageTime { get; set; }

        /// <summary>
        ///     ?
        /// </summary>
        [JsonPropertyName("aggregators")]
        [JsonProperty(PropertyName = "aggregators")]
        public IDictionary<string, object> Aggregators { get; set; }

        /// <summary>
        ///     ?
        /// </summary>
        [JsonPropertyName("sendCount")]
        [JsonProperty(PropertyName = "sendCount")]
        public long SendCount { get; set; }

        /// <summary>
        ///     ?
        /// </summary>
        [JsonPropertyName("receivedCount")]
        [JsonProperty(PropertyName = "receivedCount")]
        public long ReceivedCount { get; set; }

        /// <summary>
        ///     Total number of vertices processed.
        /// </summary>
        [JsonPropertyName("vertexCount")]
        [JsonProperty(PropertyName = "vertexCount")]
        public long VertexCount { get; set; }

        /// <summary>
        ///     Total number of edges processed.
        /// </summary>
        [JsonPropertyName("edgeCount")]
        [JsonProperty(PropertyName = "edgeCount")]
        public long EdgeCount { get; set; }

        /// <summary>
        ///     Statistics about the Pregel execution. The value will only be populated once the algorithm has finished.
        /// </summary>
        [JsonPropertyName("reports")]
        [JsonProperty(PropertyName = "reports")]
        public ICollection<IDictionary<string, object>> Reports { get; set; }

        /// <summary>
        ///     ?
        /// </summary>
        [JsonPropertyName("parallelism")]
        [JsonProperty(PropertyName = "parallelism")]
        public string Parallelism { get; set; }
    }

    /// <summary>
    ///     Arango Pregel Control
    /// </summary>
    public interface IArangoPregelModule
    {
        /// <summary>
        ///     Start the execution of a Pregel algorithm
        /// </summary>
        Task<long> StartJobAsync(ArangoHandle database, ArangoPregel job);

        /// <summary>
        ///     Get the status of a Pregel execution
        /// </summary>
        Task<ArangoPregelStatus> GetJobStatusAsync(ArangoHandle database, long id);

        /// <summary>
        ///     Cancel an ongoing Pregel execution
        /// </summary>
        Task DeleteJobAsync(ArangoHandle database, long id);
    }
}