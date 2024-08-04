using System.Net;
using System.Security.Authentication;
using Hangfire;
using OnDemandTutor.DataAccess.ExceptionModels;
using OnDemandTutor.Helper;

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

    private (HttpStatusCode, ApiErrorActionResult) GenerateErrorResponse(Exception exception)
    {
        HttpStatusCode status;
        string title;
        var errors = new List<ValidationErrorModel>();

        switch (exception)
        {
            case BadRequestException badRequestException:
                status = HttpStatusCode.BadRequest;
                title = "Bad Request";
                errors.Add(new ValidationErrorModel(badRequestException.Message));
                break;
            case ModelException modelException:
                status = HttpStatusCode.BadRequest;
                title = "Conflict";
                errors.Add(new ValidationErrorModel(modelException.Message, modelException.PropertyName,
                    modelException.ErrorCode));
                break;
            case AuthenticationException authenticationException:
                status = HttpStatusCode.Unauthorized;
                title = "Authentication Exception";
                errors.Add(new ValidationErrorModel(authenticationException.Message, string.Empty, string.Empty));
                break;
            case UnauthorizedAccessException unauthorizedAccessException:
                status = HttpStatusCode.Unauthorized;
                title = "Unauthorized";
                errors.Add(new ValidationErrorModel(unauthorizedAccessException.Message));
                break;
            case DataNotFoundException dataNotFoundException:
                status = HttpStatusCode.NotFound;
                title = "Not Found";
                errors.Add(new ValidationErrorModel(dataNotFoundException.Message));
                break;
            case BackgroundJobClientException hangfireClientException:
                status = HttpStatusCode.InternalServerError;
                title = "Hangfire Job Client Error";
                errors.Add(new ValidationErrorModel(hangfireClientException.Message));
                break;
            case FirebaseAuthException firebaseAuthException:
                status = HttpStatusCode.InternalServerError;
                title = "Firebase Auth Error";
                errors.Add(new ValidationErrorModel(firebaseAuthException.Message));
                break;
            case FirebaseAdmin.Auth.FirebaseAuthException firebaseAuthExceptionWithEmail:
                status = HttpStatusCode.BadGateway;
                title = "Firebase Auth Exception Auth Error";
                errors.Add(new ValidationErrorModel(firebaseAuthExceptionWithEmail.Message));
                break;
            case NullReferenceException nullReferenceException:
                status = HttpStatusCode.NotFound;
                title = "Null Reference Exception";
                errors.Add(new ValidationErrorModel(nullReferenceException.Message));
                break;
            case ArgumentException argumentException:
                status = HttpStatusCode.BadRequest;
                title = "Argument Exception Exception";
                errors.Add(new ValidationErrorModel(argumentException.Message));
                break;
            default:
                status = HttpStatusCode.InternalServerError;
                title = "Internal Server Error";
                errors.Add(new ValidationErrorModel("An unexpected error occurred."));
                break;
        }

        var response = new ApiErrorActionResult
        {
            Title = title,
            Status = (int)status,
            Errors = errors
        };

        return (status, response);
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        (var status, var response) = GenerateErrorResponse(exception);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)status;

        if (status == HttpStatusCode.InternalServerError)
            _logger.LogError(exception, nameof(HttpStatusCode.InternalServerError));

        await context.Response.WriteAsync(response.SerializeObject());
    }
}