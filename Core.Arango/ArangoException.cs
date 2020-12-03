using System.Collections.Generic;
using System;
using System.Net;
using System.Collections.ObjectModel;
using System.Linq;
using Core.Arango.Protocol;

namespace Core.Arango
{
    public class ArangoException : Exception
    {
        public string ErrorMessage { get; }
        public HttpStatusCode? Code { get; }
        public ArangoErrorCode? ErrorNumber { get; }
        public IReadOnlyList<ArangoError> Errors { get; }

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

        public ArangoException(string msg, IEnumerable<ArangoError> errors)
            : base(msg)
        {
            Errors = new ReadOnlyCollection<ArangoError>(errors.ToArray());
        }
    }
}