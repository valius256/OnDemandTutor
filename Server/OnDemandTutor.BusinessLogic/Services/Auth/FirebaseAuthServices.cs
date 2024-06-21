using FirebaseAdmin.Auth;
using OnDemandTutor.BusinessLogic.Interfaces.Auth;
using OnDemandTutor.DataAccess.ExceptionModels;
using OnDemandTutor.Models.Dtos.Register;

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
        if (!string.IsNullOrEmpty(uid))
        {
            var userRecord = await FirebaseAuth.DefaultInstance.GetUserAsync(uid);
            if (userRecord == null) return null;

            return userRecord;
        }

        if (!string.IsNullOrEmpty(email))
        {
            var userRecord = await FirebaseAuth.DefaultInstance.GetUserByEmailAsync(email);
            if (userRecord == null) return null;

            return userRecord;
        }

        if (!string.IsNullOrEmpty(phone))
        {
            var userRecord = await FirebaseAuth.DefaultInstance.GetUserByPhoneNumberAsync(phone);
            if (userRecord == null) return null;

            return userRecord;
        }


        throw new ArgumentException("At least one parameter (uid, email, or phone) must be provided.");
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

    public Task<string> LoginFireBase(string email, string password)
    {
        throw new NotImplementedException();
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
}