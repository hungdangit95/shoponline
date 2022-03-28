using System;
using FluentValidation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ShopOnlineApp.Utilities.Mvc.Filters.Exceptions;

namespace ShopOnlineApp.Utilities.Mvc.Filters
{
    public class ExceptionHandler : ExceptionFilterAttribute
    {
        private readonly ILogger<ExceptionHandler> _logger;
        private readonly IHostEnvironment _hostingEnvironment;
        public ExceptionHandler(ILogger<ExceptionHandler> logger, IHostEnvironment hostingEnvironment)
        {
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
        }
        public override void OnException(ExceptionContext context)
        {
            switch (context.Exception)
            {
                case ArgumentException argumentException:
                    {
                        if (!string.IsNullOrEmpty(argumentException.ParamName))
                        {
                            context.ModelState.AddModelError(argumentException.ParamName, argumentException.Message);
                        }
                        var problemDetails = new ValidationProblemDetails(context.ModelState);
                        problemDetails.Detail = argumentException.Message;

                        context.Result = new ObjectResult(problemDetails)
                        {
                            StatusCode = StatusCodes.Status400BadRequest
                        };
                        context.ExceptionHandled = true;
                        return;
                    }

                case InvalidOperationException invalidOperationException:
                    {
                        var problemDetails = new ValidationProblemDetails(context.ModelState);
                        problemDetails.Detail = invalidOperationException.Message;
                        context.Result = new ObjectResult(problemDetails)
                        {
                            StatusCode = StatusCodes.Status400BadRequest,
                        };
                        context.ExceptionHandled = true;
                        return;
                    }

                case ValidationException validationException:
                    {
                        foreach (var item in validationException.Errors)
                        {
                            context.ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                        }

                        var problemDetails = new ValidationProblemDetails(context.ModelState);
                        problemDetails.Detail = "Your request cannot pass our validations. Sorry";
                        context.Result = new ObjectResult(problemDetails)
                        {
                            StatusCode = StatusCodes.Status400BadRequest
                        };
                        context.ExceptionHandled = true;
                        return;
                    }
                case NotFoundException notFoundException:
                    {
                        var ex = context.Exception as NotFoundException;
                        context.Exception = null;

                        context.Result = new JsonResult(ex.Message);
                        context.HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                        return;
                    }
                case BadRequestException badRequestException:
                    {

                        return;
                    }
                case UnauthorizedAccessException unauthorizedAccessException:
                    {

                        return;
                    }

                default:
                    string stack = string.Empty;
                    if (!_hostingEnvironment.IsProduction())
                    {
                        stack = context.Exception.StackTrace;
                    }
                    context.Result = new JsonResult(new
                    {
                        error = new[] { context.Exception.Message },
                        stackTrace = stack
                    });
                    context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    break;
            }
            base.OnException(context);
        }
    }
}
