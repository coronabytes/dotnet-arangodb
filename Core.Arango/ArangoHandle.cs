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
    ///     Arango database / transaction / batch handle
    /// </summary>
    public sealed class ArangoHandle
    {
        internal readonly List<ArangoBatch> Batches;
        internal readonly string Name;
        internal readonly string Transaction;

        /// <summary>
        ///     Construct handle from database name
        /// </summary>
        /// <param name="name">database name</param>
        public ArangoHandle(string name)
        {
            Name = name;
            Transaction = null;
            Batches = null;
        }

        /// <summary>
        ///     Construct handle from database guid (requires realm)
        /// </summary>
        /// <param name="name">database name</param>
        public ArangoHandle(Guid name)
        {
            Name = name.ToString("D");
            Transaction = null;
            Batches = null;
        }

        /// <summary>
        ///     Constructs wrapping handle with transaction
        /// </summary>
        public ArangoHandle(ArangoHandle other, string transaction)
        {
            Name = other.Name;
            Transaction = transaction;
            Batches = null;
        }

        /// <summary>
        ///     Constructs wrapping handle with batch
        /// </summary>
        public ArangoHandle(ArangoHandle other, bool batch)
        {
            Name = other.Name;
            Transaction = other.Transaction;
            Batches = new List<ArangoBatch>();
        }

        /// <summary>
        ///     Convert from string
        /// </summary>
        /// <param name="x"></param>
        public static implicit operator ArangoHandle(string x)
        {
            return new ArangoHandle(x);
        }

        /// <summary>
        ///     Convert from Guid
        /// </summary>
        public static implicit operator ArangoHandle(Guid x)
        {
            return new ArangoHandle(x);
        }

        /// <summary>
        ///     Convert to string
        /// </summary>
        public static implicit operator string(ArangoHandle x)
        {
            return x.Name;
        }

        /// <summary>
        ///     ToString
        /// </summary>
        public override string ToString()
        {
            return Name;
        }
    }
}