using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Core.Arango.Serialization.Json;
using Newtonsoft.Json.Converters;

namespace Core.Arango.Protocol
{
    /// <summary>
    ///     Pregel State
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
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
}