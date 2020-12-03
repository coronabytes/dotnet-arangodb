using System;
using System.Net;

namespace Core.Arango
{
    public class ArangoException : Exception
    {
        public string ErrorMessage { get; }
        public HttpStatusCode? Code { get; }
        public ArangoErrorCode? ErrorNumber { get; }
        public ArangoException(string msg) : base(msg)
        {
        }

        public ArangoException(string msg, string errorMessage, HttpStatusCode code, ArangoErrorCode errorNumber)
            : base(msg)
        {
            ErrorMessage = errorMessage;
            Code = code;
            ErrorNumber = errorNumber;
        }
    }
}