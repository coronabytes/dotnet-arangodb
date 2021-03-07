using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Core.Arango.Protocol
{
    /// <summary>
    ///   Arango HotBackup request
    /// </summary>
    public class ArangoBackupRequest
    {
        /// <summary>
        ///   The label for this backup.
        ///   The label is used together with a timestamp string create a unique backup identifier, {timestamp}_{label}.
        ///   If no label is specified, the empty string is assumed and a default UUID is created for this part of the ID.
        /// </summary>
        [JsonPropertyName("label")]
        [JsonProperty(PropertyName = "label")]
        public string Label { get; set; }

        /// <summary>
        ///   The time in seconds that the operation tries to get a consistent snapshot. The default is 120 seconds.
        /// </summary>
        [JsonPropertyName("timeout")]
        [JsonProperty(PropertyName = "timeout")]
        public int Timeout { get; set; }

        /// <summary>
        ///   If this flag is set to true and no global transaction lock can be acquired within the given timeout, a possibly inconsistent backup is taken.
        ///   The default for this flag is false and in this case a timeout results in an HTTP 408 error.
        /// </summary>
        [JsonPropertyName("allowInconsistent")]
        [JsonProperty(PropertyName = "allowInconsistent")]
        public bool AllowInconsistent { get; set; }

        /// <summary>
        ///   If this flag is set to true and no global transaction lock can be acquired within the given timeout, all running transactions are forcefully aborted to ensure that a consistent backup can be created.
        ///   This does not include JavaScript transactions.
        ///   It waits for the transactions to be aborted at most timeout seconds.
        ///   Thus using force the request timeout is doubled. To abort transactions is almost certainly not what you want for your application.
        ///   In the presence of intermediate commits it can even destroy the atomicity of your transactions.
        ///   Use at your own risk, and only if you need a consistent backup at all costs.
        ///   The default and recommended value is false.
        ///   If both allowInconsistent and force are set to true, then the latter takes precedence and transactions are aborted.
        ///   This is only available in the cluster.
        /// </summary>
        [JsonPropertyName("force")]
        [JsonProperty(PropertyName = "force")]
        public bool Force { get; set; }
    }
}