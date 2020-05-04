using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Core.Arango.DevExtreme
{
    /// <summary>
    ///     Shared Arango transform configuration
    /// </summary>
    public class ArangoTransformSettings
    {
        private static readonly Regex ValidPropertyRegex = new Regex("^[A-Za-z_][A-Za-z0-9\\.]*$",
            RegexOptions.Compiled, TimeSpan.FromMilliseconds(100));

        /// <summary>
        ///     Lookup display value for grouping key
        /// </summary>
        public Dictionary<string, string> GroupLookups = null;

        /// <summary>
        ///     only allow these property names to be grouped
        /// </summary>
        public HashSet<string> RestrictGroups = null;

        /// <summary>
        ///     FOR {IteratorVar} IN {Collection}
        /// </summary>
        public string IteratorVar { get; set; } = "x";

        /// <summary>
        ///     Name of primary key in client side model - default: key
        /// </summary>
        public string Key { get; set; } = "key";

        /// <summary>
        ///     Prepend query with this script - no bindvars through string interpolation
        /// </summary>
        public string Preamble { get; set; }

        /// <summary>
        ///     Additional filter clauses - no bindvars through string interpolation
        /// </summary>
        public string Filter { get; set; }

        /// <summary>
        ///     Custom return clause - no bindvars through string interpolation
        /// </summary>
        public string Projection { get; set; }

        /// <summary>
        ///     Filter clause limit
        /// </summary>
        public int MaxFilter { get; set; } = 50;

        /// <summary>
        ///     Sort clause limit
        /// </summary>
        public int MaxSort { get; set; } = 5;

        /// <summary>
        ///     Summary / aggregate limit
        /// </summary>
        public int MaxSummary { get; set; } = 5;

        /// <summary>
        ///     Group / aggregate limit
        /// </summary>
        public int MaxGroup { get; set; } = 5;

        /// <summary>
        ///     When take is not specified take this many documents
        /// </summary>
        public int DefaultTake { get; set; } = 20;

        /// <summary>
        ///     Max limit on documents
        /// </summary>
        public int MaxTake { get; set; } = 1000;

        /// <summary>
        ///     Transform property names to expressions - array lookups etc
        /// </summary>
        public Func<string, ArangoTransformSettings, string> PropertyTransform { get; set; }

        /// <summary>
        ///     When true (default) always sort by _key last if not already present
        /// </summary>
        public bool StableSort { get; set; } = true;

        /// <summary>
        ///     Validates property names
        /// </summary>
        public Func<string, string> ValidPropertyName { get; set; } = s =>
        {
            if (string.IsNullOrWhiteSpace(s) )
                throw new Exception("empty propertyName");
            
            if (s.Length > 128)
                throw new Exception("propertyName > 128");

            if (!ValidPropertyRegex.IsMatch(s))
                throw new Exception("propertyName \"" + s + "\" does not match ^[A-Za-z_][A-Za-z0-9\\._]*$");

            return s;
        };
    }
}