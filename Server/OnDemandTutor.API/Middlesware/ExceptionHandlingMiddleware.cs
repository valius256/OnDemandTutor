using Hangfire;
using OnDemandTutor.DataAccess.ExceptionModels;
using System.Net;
using System.Text.Json;

namespace OnDemandTutor.API.Middlesware;

public class ExceptionHandlingMiddleware
{
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (status, response) = GenerateErrorResponse(exception);
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)status;
        _logger.LogError(exception, response.Title);
        return context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }

    private (HttpStatusCode, ApiErrorActionResult) GenerateErrorResponse(Exception exception)
    {
        HttpStatusCode status;
        ApiErrorActionResult response;

        switch (exception)
        {
            case BadRequestException badRequestException:
                status = HttpStatusCode.BadRequest;
                response = new ApiErrorActionResult
                {
                    Title = "Bad Request",
                    Status = (int)status,
                    Errors = new List<ValidationErrorModel>
                    {
                        new(badRequestException.Message)
                    }
                };
                break;
            case ModelException modelException:
                status = HttpStatusCode.BadRequest;
                response = new ApiErrorActionResult
                {
                    Title = "Conflict",
                    Status = (int)status,
                    Errors = new List<ValidationErrorModel>
                    {
                        new(modelException.Message, modelException.PropertyName, modelException.ErrorCode)
                    }
                };
                break;
            case BackgroundJobClientException hangfireClientException:
                status = HttpStatusCode.InternalServerError;
                response = new ApiErrorActionResult
                {
                    Title = "Hangfire Job Client Error",
                    Status = (int)status,
                    Errors = new List<ValidationErrorModel>
                    {
                        new(hangfireClientException.Message)
                    }
                };
                break;
            case FirebaseAuthException firebaseAuthException:
                status = HttpStatusCode.InternalServerError;
                response = new ApiErrorActionResult
                {
                    Title = "Firebase Auth Error",
                    Status = (int)status,
                    Errors = new List<ValidationErrorModel>
                    {
                        new(firebaseAuthException.Message, "FirebaseAuth", "FirebaseAuthError")
                    }
                };
                break;
            default:
                status = HttpStatusCode.InternalServerError;
                response = new ApiErrorActionResult
                {
                    Title = "Internal Server Error",
                    Status = (int)status,
                    Errors = new List<ValidationErrorModel>
                    {
                        new("An unexpected error occurred.")
                    }
                };
                break;
        }

        return (status, response);
    }
}