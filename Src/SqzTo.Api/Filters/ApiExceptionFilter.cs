﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SqzTo.Api.Common.ObjectResults;
using SqzTo.Application.Common.Exceptions;
using System;
using System.Collections.Generic;

namespace SqzTo.Api.Filters
{
    /// <summary>
    /// An abstract API filter that runs asynchronously after an action has thrown an System.Exception.
    /// </summary>
    public class ApiExceptionFilter : ExceptionFilterAttribute
    {
        private readonly IDictionary<Type, Action<ExceptionContext>> _exceptionHandlers;

        /// <summary>
        /// Initializes a new instance of <see cref="ApiExceptionFilter"/> class.
        /// </summary>
        public ApiExceptionFilter()
        {
            _exceptionHandlers = new Dictionary<Type, Action<ExceptionContext>>
            {
                { typeof(ValidationException), HandleValidationException },
                { typeof(NotFoundException), HandleNotFoundException },
                { typeof(NotImplementedException), HandleNotImplementedException }
            };
            
        }

        /// <summary>
        /// Called after an action has thrown an <see cref="Exception"/>.
        /// </summary>
        /// <param name="context">The <see cref="ExceptionContext"/></param>
        public override void OnException(ExceptionContext context)
        {
            HandleException(context);
            base.OnException(context);
        }

        private void HandleException(ExceptionContext context)
        {
            var type = context.Exception.GetType();
            if (_exceptionHandlers.ContainsKey(type))
            {
                _exceptionHandlers[type].Invoke(context);
                return;
            }

            HandleUnknownException(context);
        }

        /// <summary>
        /// Handles an <see cref="Exception"/> of type <see cref="ValidationException"/>.
        /// </summary>
        /// <param name="context">The <see cref="ExceptionContext"/></param>
        private void HandleValidationException(ExceptionContext context)
        {
            var exception = context.Exception as ValidationException;

            var details = new ValidationProblemDetails(exception.Errors)
            {
                Status = StatusCodes.Status400BadRequest,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                Title = "The server cannot process the request due to a client error.",
                Detail = exception.Message
            };

            context.Result = new BadRequestObjectResult(details);

            context.ExceptionHandled = true;
        }

        /// <summary>
        /// Handles an <see cref="Exception"/> of type <see cref="NotFoundException"/>.
        /// </summary>
        /// <param name="context">The <see cref="ExceptionContext"/></param>
        private void HandleNotFoundException(ExceptionContext context)
        {
            var exception = context.Exception as NotFoundException;

            var details = new ProblemDetails()
            {
                Status = StatusCodes.Status404NotFound,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                Title = "The specified resource was not found.",
                Detail = exception.Message
            };

            context.Result = new NotFoundObjectResult(details);

            context.ExceptionHandled = true;
        }

        /// <summary>
        /// Handles an <see cref="Exception"/> of type <see cref="NotImplementedException"/>.
        /// </summary>
        /// <param name="context">The <see cref="ExceptionContext"/></param>
        private void HandleNotImplementedException(ExceptionContext context)
        {
            var exception = context.Exception as NotImplementedException;

            var details = new ProblemDetails()
            {
                Status = StatusCodes.Status501NotImplemented,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.6.2",
                Title = "The server does not support the functionality required to fulfill the request.",
                Detail = exception.Message
            };

            context.Result = new NotImplementedObjectResult(details);

            context.ExceptionHandled = true;
        }

        /// <summary>
        /// Handles an <see cref="Exception"/> of unknown type.
        /// </summary>
        /// <param name="context">The <see cref="ExceptionContext"/></param>
        private void HandleUnknownException(ExceptionContext context)
        {
            var details = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "An error occurred while processing your request.",
                Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1"
            };

            context.Result = new ObjectResult(details)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };

            context.ExceptionHandled = true;
        }
    }
}
