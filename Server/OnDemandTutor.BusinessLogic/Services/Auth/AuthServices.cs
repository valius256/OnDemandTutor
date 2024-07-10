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

    public AuthServices(IUserServices userServices, IUnitOfWorkRepository unitOfWorkRepository,
        IJwtProviderServices jwtProviderServices, IFireBaseAuthServices fireBaseAuthServices)
    {
        _unitOfWorkRepository = unitOfWorkRepository;
        _userServices = userServices;
        _jwtProviderServices = jwtProviderServices;
        _fireBaseAuthServices = fireBaseAuthServices;
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

        var userId = claimsPrincipal.FindFirst(c => c.Type == "user_id")?.Value;
        if (userId.IsNullOrEmpty()) throw new BadRequestException("User not found");


        var user = await _userServices.GetUserProfileByFireBaseId(userId);
        if (user == null) throw new BadRequestException("User not found");

        return user;
    }

    public async Task<GetProfileUserDtos?> GetUserByClaimsNotRequired(ClaimsPrincipal claimsPrincipal)
    {
        if (claimsPrincipal.Identities == null) return null;

        var userId = claimsPrincipal.FindFirst(c => c.Type == "user_id")?.Value;
        if (userId.IsNullOrEmpty()) return null;

        var user = await _userServices.GetUserProfileByFireBaseId(userId);
        return user;
    }

    public async Task<string> ForgotPassword(string email)
    {
        var userExist = await _userServices.GetUserByEmail(email);
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
}