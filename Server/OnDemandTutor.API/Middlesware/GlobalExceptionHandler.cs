using Microsoft.AspNetCore.Diagnostics;
using OnDemandTutor.DataAccess.ExceptionModels;
using System.Diagnostics;

namespace OnDemandTutor.API.Middlesware
{
    public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger = logger;

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var traceId = Activity.Current?.Id ?? httpContext.TraceIdentifier;

            logger.LogError(exception, "An unhandled exception has occurred on machine:{machineName} with traceId: {traceId}", Environment.MachineName, traceId);

            var (statusCode, title) = MapException(exception);

            await Results.Problem(
                title: "dcm backend nhu lol, loi nua roi dcmmmmm",
                statusCode: StatusCodes.Status500InternalServerError,
                extensions: new Dictionary<string, object?>
                {
                    { "traceId" , traceId }
                }
                ).ExecuteAsync(httpContext);

            return true;
        }

        private static (int StatusCde, string Title) MapException(Exception exception)
        {
            return exception switch
            {
                ArgumentOutOfRangeException => (StatusCodes.Status400BadRequest, exception.Message),
                UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, exception.Message),
                HttpRequestException => (StatusCodes.Status400BadRequest, exception.Message),
                BadRequestException => (StatusCodes.Status400BadRequest, exception.Message),
                ModelException => (StatusCodes.Status400BadRequest, exception.Message),
                FirebaseAuthException => (StatusCodes.Status503ServiceUnavailable, "Failed to register user with Firebase"),
                _ => (StatusCodes.Status500InternalServerError, "An unhandled exception has occurred")
            };
        }
    }
}