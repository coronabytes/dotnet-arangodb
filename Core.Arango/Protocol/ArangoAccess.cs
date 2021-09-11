using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Core.Arango.Serialization.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Core.Arango.Protocol
{
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
    ///     User access modes for database and collection
    /// </summary>
    [System.Text.Json.Serialization.JsonConverter(typeof(JsonStringEnumMemberConverter))]
    [Newtonsoft.Json.JsonConverter(typeof(StringEnumConverter))]
    public enum ArangoAccess
    {
        /// <summary>
        ///     No access
        /// </summary>
        [EnumMember(Value = "none")] None,

        /// <summary>
        ///     Read access only
        /// </summary>
        [EnumMember(Value = "ro")] ReadOnly,

        /// <summary>
        ///     Read and write access
        /// </summary>
        [EnumMember(Value = "rw")] ReadWrite
    }
}