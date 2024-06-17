using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OnDemandTutor.BusinessLogic.Interfaces.Auth;
using OnDemandTutor.BusinessLogic.Interfaces.User;
using OnDemandTutor.DataAccess;
using OnDemandTutor.DataAccess.ExceptionModels;
using OnDemandTutor.Models.Dtos;
using OnDemandTutor.Models.Dtos.Authen;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OnDemandTutor.BusinessLogic.Services.Auth
{
    public class AuthServices : IAuthServices
    {
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;
        private readonly IConfiguration _configuration;
        private readonly IUserServices _userServices;
        private readonly IFireBaseAuthServices _fireBaseAuthServices;
        private readonly IJwtProviderServices _jwtProviderServices;
        public AuthServices(IUserServices userServices, IUnitOfWorkRepository unitOfWorkRepository, IJwtProviderServices jwtProviderServices, IFireBaseAuthServices fireBaseAuthServices)
        {
            _unitOfWorkRepository = unitOfWorkRepository;
            _userServices = userServices;
            _jwtProviderServices = jwtProviderServices;
            _fireBaseAuthServices = fireBaseAuthServices;
        }

        public async Task<AuthResponseDto> Login(LoginDtos loginDto)
        {
            var user = await _userServices.VerifyLogin(loginDto.Email, loginDto.Password);
            return new AuthResponseDto
            {
                Token = CreateToken(user.Id, user.Role.ToString())
            };
        }

        public string CreateToken(long userId, string userRole)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:Key"]!);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("uid", userId.ToString()),
                    new Claim(ClaimTypes.Role, userRole)
                }),
                Expires = DateTime.Now.AddDays(Convert.ToInt32(_configuration["JwtSettings:DurationInDays"])),
                Issuer = _configuration["JwtSettings:Issuer"],
                Audience = _configuration["JwtSettings:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<string> LoginWithFireBase(LoginDtos loginDto)
        {
            var listUser = await _fireBaseAuthServices.GetAllUserRecord();
            await _userServices.SyncUserAsync(listUser);
            return await _jwtProviderServices.GetForCredentialsAsync(loginDto.Email, loginDto.Password);
        }

        public async Task<GetProfileUserDtos> GetUserProfileByClaim(ClaimsPrincipal claimsPrincipal)
        {
            if (claimsPrincipal.Identities == null)
            {
                throw new BadRequestException("User not Authenticate");
            }

            var userId = claimsPrincipal.FindFirst(c => c.Type == "id")?.Value;
            if (userId.IsNullOrEmpty())
            {
                throw new BadRequestException("User not found");
            }


            var user = await _userServices.GetUserProfileById(Int32.Parse(userId));
            if (user == null)
            {
                throw new BadRequestException("User not found");
            }

            return user;
        }

        public async Task<string> ForgotPassword(string email)
        {
            var userExist = await _userServices.GetProfile(null, email);
            if (userExist == null)
            {
                throw new BadRequestException("User not found");
            }

            return await _fireBaseAuthServices.ForgotPassword(email);
        }

        public async Task<bool> DeleteUserAsync(string? email)
        {
            await _fireBaseAuthServices.DeleteUserAsync(email);
            await _userServices.DeleteUserAsync(email);

            await _unitOfWorkRepository.SaveChangesAsync();
            return true;
        }
    }
}
