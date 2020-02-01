using System;

namespace Core.Arango
{
    /// <summary>
    ///     Identifies database with optional transaction
    /// </summary>
    public readonly struct ArangoHandle
    {
        internal readonly string Name;
        internal readonly string Transaction;

        public ArangoHandle(string name)
        {
            Name = name;
            Transaction = null;
        }

        public ArangoHandle(Guid name)
        {
            Name = name.ToString("D");
            Transaction = null;
        }

        public ArangoHandle(ArangoHandle other, string transaction)
        {
            Name = other.Name;
            Transaction = transaction;
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