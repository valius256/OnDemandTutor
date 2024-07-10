using FirebaseAdmin.Auth;
using OnDemandTutor.Models.Dtos.Register;

namespace OnDemandTutor.BusinessLogic.Interfaces.Auth;

public interface IFireBaseAuthServices
{
    Task<string> RegisterUser(RegisterDtos registerDtos);
    Task<string> ForgotPassword(string email);
    Task<UserRecord?> GetUserAsync(string? uid, string? email, string? phone);
    Task<bool> DeleteUserAsync(string? email);
    Task SetCustomClaimsAsync(string userId, Dictionary<string, object> claims);
    Task<List<ExportedUserRecord>> GetAllUserRecord();
    Task<string> CreateSessionCookieAsync(string idToken, SessionCookieOptions options);
}