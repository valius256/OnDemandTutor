using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using OnDemandTutor.DataAccess.Models.Exception;
using OnDemandTutor.Helper.Utils;
using OnDemandTutor.Models;
using System.Net;

namespace OnDemandTutor.API.Filters
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IStringLocalizer<Resource> _localizer;
        private readonly AppSetting _appSetting;

        public ExceptionMiddleware(
            IStringLocalizer<Resource> localizer,
            RequestDelegate next,
            ILogger<ExceptionMiddleware> logger,
            IOptions<AppSetting> optionModel
            )
        {
            _next = next;
            _localizer = localizer;
            _logger = logger;
            _appSetting = optionModel.Value;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {

                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            int statusCode = (int)HttpStatusCode.BadRequest;
            string name = "";
            string msg;
            string code;
            string title = nameof(HttpStatusCode.BadRequest);
            if (exception is UnauthorizedAccessException)
            {
                title = nameof(StatusCodes.Status401Unauthorized);
                statusCode = StatusCodes.Status401Unauthorized;
                code = "UNAUTHORIZED";
                msg = exception.Message;
            }
            else if (exception is ForbiddenContext)
            {
                title = nameof(StatusCodes.Status403Forbidden);
                statusCode = StatusCodes.Status403Forbidden;
                code = "FORBIDDEN";
                msg = exception.Message;
            }
            else if (exception is ModelException exModel)
            {
                msg = string.Format(_localizer[exModel.Message]);
                code = exModel.Message;
                name = exModel.Field;
            }
            else if (exception.Data is DataNotFoundException)
            {
                msg = string.Format(_localizer["ERROR_DATA_NOT_FOUND"], exception.Message);
                code = "ERROR_DATA_NOT_FOUND";
            }
            else
            {
                title = nameof(HttpStatusCode.InternalServerError);
                statusCode = (int)HttpStatusCode.InternalServerError;
                msg = _appSetting.ShowInternalServerError ? $"{exception.Message} | {exception.StackTrace}" : "INTERNAL_SERVER_ERROR";
                code = "INTERNAL_SERVER_ERROR";
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;
            var result = new ApiErrorActionResult()
            {
                Title = title,
                Status = statusCode,
                Errors = new List<ValidationErrorModel>() { new ValidationErrorModel(name, msg, code) }
            }.SerializeObject();

            if (statusCode == (int)HttpStatusCode.InternalServerError)
                _logger.LogError(exception, nameof(HttpStatusCode.InternalServerError));

            return context.Response.WriteAsync(result);
        }
    }
}
