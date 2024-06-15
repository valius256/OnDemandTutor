namespace OnDemandTutor.API.Extensions;

public static class ResultExtensions
{
    public static IResult ToProblemDetails(this IResult result)
    {
        return Results.Problem(
            statusCode: StatusCodes.Status400BadRequest,
            title: "Bad Request",
            extensions: new Dictionary<string, object?>
            {
                { "error", result.ToString() }
            });
    }
}