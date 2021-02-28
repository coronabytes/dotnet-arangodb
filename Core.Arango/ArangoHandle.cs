using System;
using System.Collections.Generic;

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

    /// <summary>
    ///     Identifies database with optional transaction
    /// </summary>
    public sealed class ArangoHandle
    {
        internal readonly string Name;
        internal readonly string Transaction;
        internal readonly List<ArangoBatch> Batches;

        public ArangoHandle(string name)
        {
            Name = name;
            Transaction = null;
            Batches = null;
        }

        public ArangoHandle(Guid name)
        {
            Name = name.ToString("D");
            Transaction = null;
            Batches = null;
        }

        public ArangoHandle(ArangoHandle other, string transaction)
        {
            Name = other.Name;
            Transaction = transaction;
            Batches = null;
        }

        public ArangoHandle(ArangoHandle other, bool batch)
        {
            Name = other.Name;
            Transaction = other.Transaction;
            Batches = new List<ArangoBatch>();
        }

        public static implicit operator ArangoHandle(string x)
        {
            return new ArangoHandle(x);
        }

        public static implicit operator ArangoHandle(Guid x)
        {
            return new ArangoHandle(x);
        }

        public static implicit operator string(ArangoHandle x)
        {
            return x.Name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}