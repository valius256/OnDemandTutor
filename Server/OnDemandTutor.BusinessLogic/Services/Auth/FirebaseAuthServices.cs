using FirebaseAdmin.Auth;
using OnDemandTutor.BusinessLogic.Interfaces.Auth;
using OnDemandTutor.Models.Dtos.Register;

namespace OnDemandTutor.BusinessLogic.Services.Auth
{
    public class FirebaseAuthServices : IFireBaseAuthServices
    {
        public async Task<string> RegisterUser(RegisterDtos registerDtos)
        {
            var userForFireBaseAuth = new UserRecordArgs()
            {
                Email = registerDtos.Email,
                Password = registerDtos.Password
            };

            var userRecord = await FirebaseAuth.DefaultInstance.CreateUserAsync(userForFireBaseAuth);

            await FirebaseAuth.DefaultInstance.GenerateEmailVerificationLinkAsync(userRecord.Email);
            return userRecord.Uid;
        }


        public async Task<UserRecord> GetUser(string? uid, string? email, string? phone)
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


            throw new ArgumentException("At least one parameter (uid, email, or phone) must be provided.");
        }

        public async Task<string> ForgotPassword(string email)
        {
            return await FirebaseAuth.DefaultInstance.GeneratePasswordResetLinkAsync(email);
        }
    }
}
