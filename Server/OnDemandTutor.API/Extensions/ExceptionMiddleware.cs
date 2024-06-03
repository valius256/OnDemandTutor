using Elfie.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using OnDemandTutor.DataAccess;
using OnDemandTutor.DataAccess.Models;
using OnDemandTutor.DataAccess.Models.Exception;
using OnDemandTutor.Helper.Utils;
using System.Net;
using System.Security.Authentication;
using System.Text.Json;

namespace OnDemandTutor.API.Extensions
{
    public class ExceptionMiddleware : IMiddleware
    { 
        private readonly ILogger<ExceptionMiddleware> _logger;
   

        public ExceptionMiddleware(
            ILogger<ExceptionMiddleware> logger
            )
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
             try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                ProblemDetails problem = new ProblemDetails()
                {
                    Status = (int)HttpStatusCode.InternalServerError,
                    Type = "Server Error",
                    Title = "Server Error",
                    Detail = "An unexpected error occurred! Please try again later."
                };

                string json = JsonSerializer.Serialize(problem);

                await context.Response.WriteAsync(json);
                
                context.Response.ContentType = "application/json";
            }
        }

        //private Task HandleExceptionAsync(HttpContext context, Exception exception)
        //{
        //    int statusCode = (int)HttpStatusCode.BadRequest;
        //    string name = "";
        //    string msg;
        //    string code;
        //    string title = nameof(HttpStatusCode.BadRequest);
        //    if (exception is AuthenticationException)
        //    {
        //        title = nameof(StatusCodes.Status401Unauthorized);
        //        statusCode = StatusCodes.Status401Unauthorized;
        //        code = "UNAUTHORIZED";
        //        msg = exception.Message;
        //    }
        //    else if (exception is UnauthorizedAccessException)
        //    {
        //        title = nameof(StatusCodes.Status401Unauthorized);
        //        statusCode = StatusCodes.Status401Unauthorized;
        //        code = "UNAUTHORIZED";
        //        msg = exception.Message;
        //    }
        //    else if (exception is PermissionDeniedException)
        //    {
        //        title = nameof(StatusCodes.Status403Forbidden);
        //        statusCode = StatusCodes.Status403Forbidden;
        //        code = "FORBIDDEN";
        //        msg = exception.Message;
        //    }
        //    else if (exception is ModelException exModel)
        //    {
        //        msg = string.Format(_localizer[exModel.Message]);
        //        code = exModel.Message;
        //        name = exModel.Field;
        //    }
        //    else if (exception is DataNotFoundException)
        //    {
        //        msg = string.Format(_localizer["ERROR_DATA_NOT_FOUND"], exception.Message);
        //        code = "ERROR_DATA_NOT_FOUND";
        //    }
        //    else
        //    {
        //        title = nameof(HttpStatusCode.InternalServerError);
        //        statusCode = (int)HttpStatusCode.InternalServerError;
        //        msg = _appSetting.ShowInternalServerError ? $"{exception.Message} | {exception.StackTrace}" : "INTERNAL_SERVER_ERROR";
        //        code = "INTERNAL_SERVER_ERROR";
        //    }

        //    context.Response.ContentType = "application/json";
        //    context.Response.StatusCode = statusCode;
        //    var result = new ApiErrorActionResult()
        //    {
        //        Title = title,
        //        Status = statusCode,
        //        Errors = new List<ValidationErrorModel>() { new ValidationErrorModel(name, msg, code) }
        //    }.SerializeObject();

        //    if (statusCode == (int)HttpStatusCode.InternalServerError)
        //        _logger.LogError(exception, nameof(HttpStatusCode.InternalServerError));

        //    return context.Response.WriteAsync(result);
        //}
    }
}
