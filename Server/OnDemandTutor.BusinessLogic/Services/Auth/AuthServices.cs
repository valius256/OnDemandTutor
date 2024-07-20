using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OnDemandTutor.BusinessLogic.Interfaces.Auth;
using OnDemandTutor.BusinessLogic.Interfaces.User;
using OnDemandTutor.DataAccess;
using OnDemandTutor.DataAccess.ExceptionModels;
using OnDemandTutor.Models.Dtos.Authen;
using OnDemandTutor.Models.Dtos.User;
using System.Security.Claims;

namespace OnDemandTutor.BusinessLogic.Services.Auth;

public class AuthServices : IAuthServices
{
    private readonly IConfiguration _configuration;
    private readonly IFireBaseAuthServices _fireBaseAuthServices;
    private readonly IJwtProviderServices _jwtProviderServices;
    private readonly IUnitOfWorkRepository _unitOfWorkRepository;
    private readonly IUserServices _userServices;
    private readonly IHttpContextAccessor _httpContextAccessor;


    public AuthServices(IUserServices userServices, IUnitOfWorkRepository unitOfWorkRepository,
        IJwtProviderServices jwtProviderServices, IFireBaseAuthServices fireBaseAuthServices, IHttpContextAccessor HttpContextAccessor)
    {
        _unitOfWorkRepository = unitOfWorkRepository;
        _userServices = userServices;
        _jwtProviderServices = jwtProviderServices;
        _fireBaseAuthServices = fireBaseAuthServices;
        _httpContextAccessor = HttpContextAccessor;
    }

    public async Task<AuthenResponseDto> LoginWithFireBase(LoginDtos loginDto)
    {
        var listUser = await _fireBaseAuthServices.GetAllUserRecord();
        await _userServices.SyncUserAsync(listUser);
        return await _jwtProviderServices.GetForCredentialsAsync(loginDto.Email, loginDto.Password);
    }

    public async Task<GetProfileUserDtos> GetUserProfileByClaim(ClaimsPrincipal claimsPrincipal)
    {
        if (claimsPrincipal.Identities == null) throw new BadRequestException("User not Authenticate");

        var userId = claimsPrincipal.FindFirst(c => c.Type == "id")?.Value;
        if (userId.IsNullOrEmpty()) throw new BadRequestException("User not found");


        var user = await _userServices.GetUserByIdAsync(int.Parse(userId));
        if (user == null) throw new BadRequestException("User not found");

        return user;
    }

    public async Task<GetProfileUserDtos?> GetUserByClaimsNotRequired(ClaimsPrincipal claimsPrincipal)
    {
        if (claimsPrincipal.Identities == null) return null;

        var userId = claimsPrincipal.FindFirst(c => c.Type == "user_id")?.Value;
        if (userId.IsNullOrEmpty()) return null;

        var user = await _userServices.GetUserProfileByFireBaseIdAsync(userId);
        return user;
    }

    public async Task<string> ForgotPassword(string email)
    {
        var userExist = await _userServices.GetUserByEmailAsync(email);
        if (userExist == null) throw new BadRequestException("User not found");

        return await _fireBaseAuthServices.ForgotPassword(email);
    }


    public async Task<bool> DeleteUserAsync(string? email)
    {
        await _fireBaseAuthServices.DeleteUserAsync(email);
        await _userServices.DeleteUserAsync(email);

        await _unitOfWorkRepository.SaveChangesAsync();
        return true;
    }

    public async Task<string> GrantRole(GrantRoleDto request)
    {
        var record = await _unitOfWorkRepository.UserRepository.FirstOrDefaultAsync(l => l.Email == request.email || l.Id == request.id);

        record.Role = request.Role;
        _unitOfWorkRepository.UserRepository.Update(record);
        await _unitOfWorkRepository.SaveChangesAsync();
        return record.Role.ToString();
    }

    public async Task<bool> ChangePasswordAsync(ClaimsPrincipal claimsPrincipal,ChangePasswordDto changePasswordDto)
    {
        // Retrieve the user's email or ID from claims
        var user = await GetUserProfileByClaim(claimsPrincipal);
      // var email = user.FindFirst(ClaimTypes.Email)?.Value;
        if (user is null)
        {
            throw new Exception("User email not found in claims.");
        }

        // Check if the old password is correct
        var isOldPasswordCorrect = await _jwtProviderServices.GetForCredentialsAsync(user.Email, changePasswordDto.OldPassword);
        if (isOldPasswordCorrect is null)
        {
            throw new Exception("Old password is incorrect.");
        }
        if(changePasswordDto.OldPassword == changePasswordDto.NewPassword)
        {
            throw new Exception("New password similar to the old password, please choose another one.");
        }
        var userEntity = await _unitOfWorkRepository.UserRepository.FirstOrDefaultAsync(u => u.Email == user.Email);
        if (userEntity is null)
        {
            throw new Exception("User not found.");
        }
        // Change the password
        if (userEntity is not null)
        {
            userEntity.Password = changePasswordDto.NewPassword;
            _unitOfWorkRepository.UserRepository.Update(userEntity);
            _unitOfWorkRepository.SaveChanges();
            return true;
        }
     
        return false;
    }
}