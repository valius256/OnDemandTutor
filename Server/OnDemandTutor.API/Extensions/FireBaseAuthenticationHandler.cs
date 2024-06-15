using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace OnDemandTutor.API.Extensions
{
    public class FireBaseAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private FirebaseApp _firebaseApp;

        public FireBaseAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Context.Request.Headers.ContainsKey("Authorization"))
            {
                return AuthenticateResult.NoResult();
            }

            string bearerToken = Context.Request.Headers["Authorization"];
            if (bearerToken != null && bearerToken.StartsWith("Bearer "))
            {
                return AuthenticateResult.Fail("Invalid Authorization Header");
            }

            string token = bearerToken.Substring("Bearer ".Length).Trim();

            FirebaseToken firebaseToken = await FirebaseAuth.GetAuth(_firebaseApp).VerifyIdTokenAsync(token);

            return AuthenticateResult.Success(new AuthenticationTicket(new ClaimsPrincipal(new List<ClaimsIdentity>()
            {
                new ClaimsIdentity(ToClaims(firebaseToken.Claims))
            }), JwtBearerDefaults.AuthenticationScheme));
        }

        private IEnumerable<Claim>? ToClaims(IReadOnlyDictionary<string, object> claims)
        {
            return new List<Claim>
            {

            };
        }
    }
}
