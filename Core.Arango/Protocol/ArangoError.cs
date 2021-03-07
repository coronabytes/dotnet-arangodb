namespace Core.Arango.Protocol
{
    /// <summary>
    ///  Arango Error Description
    /// </summary>
    public class ArangoError
    {
        // <summary>
        ///  Arango Error Description
        /// <param name="errorMessage">message</param>
        /// <param name="errorNumber">code</param>
        public ArangoError(string errorMessage, ArangoErrorCode errorNumber)
        {
            ErrorMessage = errorMessage;
            ErrorNumber = errorNumber;
        }

        /// <summary>
        ///  Message
        /// </summary>
        public string ErrorMessage { get; }
        
        /// <summary>
        ///  Code
        /// </summary>
        public ArangoErrorCode? ErrorNumber { get; }
    }
}