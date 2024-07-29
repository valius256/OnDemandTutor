using FirebaseAdmin.Auth;
using OnDemandTutor.BusinessLogic.Interfaces.Auth;
using OnDemandTutor.DataAccess.ExceptionModels;
using OnDemandTutor.Models.Dtos.Register;
using FirebaseAuthException = OnDemandTutor.DataAccess.ExceptionModels.FirebaseAuthException;

namespace OnDemandTutor.BusinessLogic.Services.Auth;

public class FirebaseAuthServices : IFireBaseAuthServices
{
    public async Task<string> RegisterUser(RegisterDtos registerDtos)
    {
        var userForFireBaseAuth = new UserRecordArgs
        {
            Email = registerDtos.Email,
            Password = registerDtos.Password
        };

        var userRecord = await FirebaseAuth.DefaultInstance.CreateUserAsync(userForFireBaseAuth);
        await FirebaseAuth.DefaultInstance.GenerateEmailVerificationLinkAsync(userRecord.Email);
        return userRecord.Uid;
    }

    public async Task<UserRecord?> GetUserAsync(string? uid, string? email, string? phone)
    {
        try
        {
            if (!string.IsNullOrEmpty(uid))
            {
                var userRecord = await FirebaseAuth.DefaultInstance.GetUserAsync(uid);
                return userRecord;
            }

            if (!string.IsNullOrEmpty(email))
            {
                var userRecord = await FirebaseAuth.DefaultInstance.GetUserByEmailAsync(email);
                return userRecord;
            }

            if (!string.IsNullOrEmpty(phone))
            {
                var userRecord = await FirebaseAuth.DefaultInstance.GetUserByPhoneNumberAsync(phone);
                return userRecord;
            }

            throw new ArgumentException("At least one parameter (uid, Email, or Phone) must be provided.");
        }
        catch (FirebaseAuthException ex)
        {

            return null;
        }
    }


    public async Task<bool> DeleteUserAsync(string? email)
    {
        var user = await FirebaseAuth.DefaultInstance.GetUserByEmailAsync(email);
        if (user == null)
            throw new ModelException(user.ToString(), "not found", "");

        await FirebaseAuth.DefaultInstance.DeleteUserAsync(user.Uid);
        return true;
    }

    public async Task<string> ForgotPassword(string email)
    {
        return await FirebaseAuth.DefaultInstance.GeneratePasswordResetLinkAsync(email);
    }

    public async Task SetCustomClaimsAsync(string userId, Dictionary<string, object> claims)
    {
        await FirebaseAuth.DefaultInstance.SetCustomUserClaimsAsync(userId, claims);
    }


    public async Task<List<ExportedUserRecord>> GetAllUserRecord()
    {
        var users = new List<ExportedUserRecord>();
        var pagedEnumerable = FirebaseAuth.DefaultInstance.ListUsersAsync(null);

        await foreach (var user in pagedEnumerable) users.Add(user);

        return users;
    }

    public async Task<string> CreateSessionCookieAsync(string idToken, SessionCookieOptions options)
    {
        var sessionCookie = await FirebaseAuth.DefaultInstance
            .CreateSessionCookieAsync(idToken, options);
        return sessionCookie;
    }
    
}