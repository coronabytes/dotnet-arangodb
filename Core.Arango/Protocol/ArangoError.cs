using System;
using System.Net;

namespace Core.Arango.Protocol
{
    public class ArangoError
    {
        public string ErrorMessage { get; }
        public ArangoErrorCode? ErrorNumber { get; }

        public ArangoError(string errorMessage, ArangoErrorCode errorNumber)
        {
            ErrorMessage = errorMessage;
            ErrorNumber = errorNumber;
        }
    }
}