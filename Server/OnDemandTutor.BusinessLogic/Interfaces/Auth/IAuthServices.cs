using OnDemandTutor.Models.Dtos.Authen;
using OnDemandTutor.Models.Dtos.User;
using System.Security.Claims;

namespace OnDemandTutor.BusinessLogic.Interfaces.Auth;

public interface IAuthServices
{
    Task<string> LoginWithFireBase(LoginDtos loginDto);
    Task<GetProfileUserDtos> GetUserProfileByClaim(ClaimsPrincipal claimsPrincipal);
    Task<string> ForgotPassword(string email);
    Task<bool> DeleteUserAsync(string? email);
}