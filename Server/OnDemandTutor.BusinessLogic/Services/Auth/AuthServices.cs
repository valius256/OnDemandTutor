using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OnDemandTutor.BusinessLogic.Interfaces.Auth;
using OnDemandTutor.BusinessLogic.Interfaces.User;
using OnDemandTutor.DataAccess;
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
        private readonly IJwtProviderServices _jwtProviderServices;
        public AuthServices(IUserServices userServices, IUnitOfWorkRepository unitOfWorkRepository, IJwtProviderServices jwtProviderServices)
        {
            _unitOfWorkRepository = unitOfWorkRepository;
            _userServices = userServices;
            _jwtProviderServices = jwtProviderServices;
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
            return await _jwtProviderServices.GetForCredentialsAsync(loginDto.Email, loginDto.Password);
        }
    }
}
