namespace Conduit.Core.Exceptions
{
    using System;
    using System.Collections.Generic;
    using System.Net;

    public class ConduitApiException : Exception
    {
        public ConduitApiException(string message, HttpStatusCode statusCode)
            : base(message)
        {
            StatusCode = statusCode;
            ApiErrors = new List<ConduitApiError>();
        }

        public HttpStatusCode StatusCode { get; }

        public IEnumerable<ConduitApiError> ApiErrors { get; }
    }
}