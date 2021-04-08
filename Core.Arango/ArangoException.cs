using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using Core.Arango.Protocol;

namespace Core.Arango
{
    /// <summary>
    ///     Generic Arango exception
    /// </summary>
    public class ArangoException : Exception
    {
        /// <summary>
        /// </summary>
        public ArangoException(string msg) : base(msg)
        {
        }

        /// <summary>
        /// </summary>
        public ArangoException(string msg, string errorMessage, HttpStatusCode code, ArangoErrorCode errorNumber)
            : base(msg)
        {
            ErrorMessage = errorMessage;
            Code = code;
            ErrorNumber = errorNumber;
        }

        /// <summary>
        /// </summary>
        public ArangoException(string msg, IEnumerable<ArangoError> errors)
            : base(msg)
        {
            Errors = new ReadOnlyCollection<ArangoError>(errors.ToArray());
        }

        /// <summary>
        ///     Error message
        /// </summary>
        public string ErrorMessage { get; }

        /// <summary>
        ///     HTTP status code
        /// </summary>
        public HttpStatusCode? Code { get; }

        /// <summary>
        ///     Arango error code
        /// </summary>
        public ArangoErrorCode? ErrorNumber { get; }

        /// <summary>
        ///     Error descriptions
        /// </summary>
        public IReadOnlyList<ArangoError> Errors { get; }
    }
}