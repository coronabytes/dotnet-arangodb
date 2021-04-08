using System;

namespace Core.Arango
{
    /// <summary>
    ///     Arango database / transaction / batch handle
    /// </summary>
    public sealed class ArangoHandle
    {
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
        }

        /// <summary>
        ///     Construct handle from database guid (requires realm)
        /// </summary>
        /// <param name="name">database name</param>
        public ArangoHandle(Guid name)
        {
            Name = name.ToString("D");
            Transaction = null;
        }

        /// <summary>
        ///     Constructs wrapping handle with transaction
        /// </summary>
        public ArangoHandle(ArangoHandle other, string transaction)
        {
            Name = other.Name;
            Transaction = transaction;
        }

        /// <summary>
        ///     Convert from string
        /// </summary>
        /// <param name="x"></param>
        public static implicit operator ArangoHandle(string x)
        {
            return new(x);
        }

        /// <summary>
        ///     Convert from Guid
        /// </summary>
        public static implicit operator ArangoHandle(Guid x)
        {
            return new(x);
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