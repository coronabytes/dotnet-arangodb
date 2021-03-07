using System;

namespace Core.Arango
{
    internal class ArangoBatch
    {
        public Guid ContentId { get; set; }
        public Action<string> Complete { get; set; }
        public Action<Exception> Fail { get; set; }
        public Action Cancel { get; set; }
        public bool Completed { get; set; }
        public string Request { get; set; }
    }
}