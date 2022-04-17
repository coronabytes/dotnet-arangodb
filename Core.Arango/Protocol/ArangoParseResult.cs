using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Core.Arango.Protocol
{
    /// <summary>
    ///   
    /// </summary>
    public class ArangoParseResult : ArangoResponseBase
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("parsed")]
        [JsonProperty(PropertyName = "parsed")]
        public bool Parsed { get; set; }

        /// <summary>
        ///   Abstract Syntax Tree
        /// </summary>
        [JsonPropertyName("ast")]
        [JsonProperty(PropertyName = "ast")]
        public ICollection<ArangoAstNode> Ast { get; set; }
    }
}