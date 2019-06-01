namespace Conduit.Core.Exceptions
{
    public class ConduitApiError
    {
        public ConduitApiError(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        public ConduitApiError(string errorMessage, string propertyName)
        {
            ErrorMessage = errorMessage;
            PropertyName = propertyName;
        }

        public string ErrorMessage { get; }

        public string PropertyName { get; }
    }
}