using OnDemandTutor.API.Filters;

namespace OnDemandTutor.API.Extensions
{
    public static class MiddleswareExtensions
    {
        public static void RegisterMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<JwtMiddleware>();
            //app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
