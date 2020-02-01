using System.Collections.Generic;

namespace Core.Arango.DevExtreme
{
    public class ArangoTransformSettings
    {
        public string IteratorVar { get; set; } = "x";
        public string Key { get; set; } = "Key";

        public HashSet<string> RestrictGroups = null;
        public string Filter { get; set; }
        public string Projection { get; set; }
    }
}