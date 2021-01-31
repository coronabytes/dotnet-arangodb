namespace Core.Arango.Protocol
{
    public class ArangoError
    {
        public ArangoError(string errorMessage, ArangoErrorCode errorNumber)
        {
            ErrorMessage = errorMessage;
            ErrorNumber = errorNumber;
        }

        public string ErrorMessage { get; }
        public ArangoErrorCode? ErrorNumber { get; }
    }
}