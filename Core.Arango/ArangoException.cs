using System;

namespace Core.Arango
{
    public class ArangoException : Exception
    {
        public ArangoException(string msg) : base(msg)
        {
        }
    }
}