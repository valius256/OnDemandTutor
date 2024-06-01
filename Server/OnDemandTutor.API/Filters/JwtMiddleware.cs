using OnDemandTutor.BusinessLogic.Business.Interfaces;
using OnDemandTutor.Helper.Utils;
namespace OnDemandTutor.API.Filters
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IJwtService jwtService, IUserService userService)
        {
            var authorizations = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ");
            var language = context.Request.Headers["language"].FirstOrDefault();
            if (language != null)
                context.Items["lang"] = language;

            if (authorizations is not null && authorizations.Length > 1)
            {
                try
                {
                    var token = authorizations.Last();
                    var jwt = token.Base64Decode();
                    var info = await jwtService.DecodeTokenAsync(jwt);
                    if (info != null)
                    {
                        context.Items["User"] = await userService.GetUserAsync(info.Id.ToString());
                        context.Items["SessionInfo"] = info;
                        //context.Items["Permissions"] = await userService.GetUserPermissions(info.Id);
                        context.Items["LocationId"] = info.LocationId;
                    }
                }
                catch
                {
                }
            }
            await _next(context);
        }
    }
}
