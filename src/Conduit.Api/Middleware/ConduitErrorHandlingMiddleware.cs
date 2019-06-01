namespace Conduit.Api.Middleware
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using Core.Exceptions;
    using Domain.ViewModels;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using Shared.Constants;

    public class ConduitErrorHandlingMiddleware
    {
        private readonly RequestDelegate _pipeline;
        private readonly ILogger<ConduitErrorHandlingMiddleware> _logger;

        public ConduitErrorHandlingMiddleware(RequestDelegate pipeline, ILogger<ConduitErrorHandlingMiddleware> logger)
        {
            _pipeline = pipeline;
            _logger = logger;
        }

        /// <summary>
        /// Kicks off he request pipeline while catching any exceptions thrown in the application layer.
        /// </summary>
        /// <param name="context">HTTP context from the request pipeline</param>
        /// <returns>Hand off to next request delegate in the pipeline</returns>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _pipeline(context);
            }
            catch (Exception e)
            {
                _logger.LogError($"An exception has occurred processing the request: {e.Message}");
                await HandleExceptionAsync(context, e);
            }
        }

        /// <summary>
        /// Handles any exception thrown during the pipeline process and in the application layer. Note that model state
        /// validation failures made in the web layer are handled by the ASP.NET Core model state validation failure filter.
        /// </summary>
        /// <param name="context">HTTP context from the request pipeline</param>
        /// <param name="exception">Exceptions thrown during pipeline processing</param>
        /// <returns>Writes the API response to the context to be returned in the web layer</returns>
        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            // Declare status code, response, and errors updated based on message/status code returned from the application
            string errorMessage;
            object errors;
            var validationFailures = new List<ConduitApiError>();

            /*
             * Handle exceptions based on type, while defaulting to generic internal server error for unexpected exceptions.
             * Each case handles binding the API response message, API response status code, the HTTP response status code,
             * and any errors incurred in the application layer. Validation failures returned from Fluent Validation will
             * be added to the API response if there are any instances.
             */
            switch (exception)
            {
                case ConduitApiException conduitApiException:
                    errorMessage = conduitApiException.Message;
                    context.Response.StatusCode = (int)conduitApiException.StatusCode;
                    if (conduitApiException.ApiErrors.Any())
                    {
                        validationFailures = conduitApiException.ApiErrors.ToList();
                    }

                    break;

/*                case ValidationException validationException:
                    errorMessage = ConduitErrorMessages.ValidationError;
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    errors = validationException.Message;
                    foreach (var validationFailure in validationException.Errors)
                    {
                        var brewdudeValidationError = new BrewdudeApiError(validationFailure.ErrorMessage)
                        {
                            ErrorCode = validationFailure.ErrorCode,
                            PropertyName = validationFailure.PropertyName
                        };
                        _logger.LogInformation("Validation failure to request @{validationFailure}", validationFailure);
                        validationFailures.Add(brewdudeValidationError);
                    }

                    break;*/

                default:
                    errorMessage = ConduitErrorMessages.InternalServerError;
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    errors = exception.Message;
                    break;
            }

            // Instantiate the response
            context.Response.ContentType = "application/json";
            var response = new ErrorViewModel(errorMessage);

            // Serialize the response and write out to the context buffer to return
            var result = JsonConvert.SerializeObject(response, ConduitConstants.ConduitJsonSerializerSettings);
            await context.Response.WriteAsync(result);
        }
    }
}