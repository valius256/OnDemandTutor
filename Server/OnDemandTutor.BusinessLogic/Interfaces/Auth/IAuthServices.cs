using OnDemandTutor.Models.Dtos.Authen;
using OnDemandTutor.Models.Dtos.User;
using System.Security.Claims;

namespace OnDemandTutor.BusinessLogic.Interfaces.Auth;

public interface IAuthServices
{
    Task<AuthenResponseDto> LoginWithFireBase(LoginDtos loginDto);
    Task<GetProfileUserDto> GetUserProfileByClaim(ClaimsPrincipal claimsPrincipal);
    Task<GetProfileUserDto?> GetUserByClaimsNotRequired(ClaimsPrincipal claimsPrincipal);
    Task<string> ForgotPassword(string email);
    Task<bool> DeleteUserAsync(string? email);
    Task<string> GrantRole(GrantRoleDto request);
    Task<bool> ChangePasswordAsync(ClaimsPrincipal claimsPrincipal,ChangePasswordDto changePasswordDto);
}