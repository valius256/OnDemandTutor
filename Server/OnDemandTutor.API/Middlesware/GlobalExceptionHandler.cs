using Microsoft.AspNetCore.Diagnostics;
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
                title: "dcm backend loi nua roi dcmmmmm",
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
                _ => (StatusCodes.Status500InternalServerError, "An unhandled exception has occurred")
            };
        }
    }
}