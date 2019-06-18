namespace Conduit.Api.Middleware
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using Core.Exceptions;
    using Domain.Dtos;
    using Domain.ViewModels;
    using FluentValidation;
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
            ErrorDto errors;
            ICollection<object> errorList = new List<object>();

            /*
             * Handle exceptions based on type, while defaulting to generic internal server error for unexpected exceptions.
             * Each case handles binding the API response message, API response status code, the HTTP response status code,
             * and any errors incurred in the application layer. Validation failures returned from Fluent Validation will
             * be added to the API response if there are any instances.
             */
            switch (exception)
            {
                case ConduitApiException conduitApiException:
                    errors = new ErrorDto(conduitApiException.Message);
                    context.Response.StatusCode = (int)conduitApiException.StatusCode;
                    if (conduitApiException.ApiErrors.Any())
                    {
                        errors.Details = conduitApiException.ApiErrors;
                    }

                    break;

                case ValidationException validationException:
                    context.Response.StatusCode = (int)HttpStatusCode.UnsupportedMediaType;
                    foreach (var validationFailure in validationException.Errors)
                    {
                        var conduitValidationError = new ConduitApiError(validationFailure.ErrorMessage, validationFailure.PropertyName);
                        errorList.Add(conduitValidationError);
                    }

                    errors = new ErrorDto(ConduitErrorMessages.ValidationError, errorList);
                    break;

                default:
                    errors = new ErrorDto(ConduitErrorMessages.InternalServerError);
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            // Instantiate the response
            context.Response.ContentType = "application/json";
            var errorResponse = new ErrorViewModel(errors);

            // Serialize the response and write out to the context buffer to return
            var result = JsonConvert.SerializeObject(errorResponse, ConduitConstants.ConduitJsonSerializerSettings);
            await context.Response.WriteAsync(result);
        }
    }
}