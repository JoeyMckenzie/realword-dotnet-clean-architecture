namespace Conduit.Core.Exceptions
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using Shared.Constants;

    public class ConduitApiException : Exception
    {
        public ConduitApiException(string message, HttpStatusCode statusCode)
            : base(message)
        {
            StatusCode = statusCode;
            ApiErrors = new List<ConduitApiError>();
            ErrorType = ConduitErrorType.Error;
        }

        public ConduitApiException(string message, HttpStatusCode statusCode, ICollection<ConduitApiError> apiErrors)
            : base(message)
        {
            StatusCode = statusCode;
            ApiErrors = apiErrors;
            ErrorType = ConduitErrorType.Error;
        }

        public object Errors { get; set; }

        public HttpStatusCode StatusCode { get; }

        public ICollection<ConduitApiError> ApiErrors { get; }

        public ConduitErrorType ErrorType { get; set; }
    }
}