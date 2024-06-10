using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnDemandTutor.API.Models;
using OnDemandTutor.BusinessLogic.Interfaces.Auth;
using OnDemandTutor.BusinessLogic.Interfaces.User;
using OnDemandTutor.Models.Dtos;
using OnDemandTutor.Models.Dtos.Authen;
using OnDemandTutor.Models.Dtos.Register;

namespace OnDemandTutor.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthServices _authService;
        private readonly IUserServices _userService;

        public AuthController(IUserServices userService, IAuthServices authServices)
        {
            _userService = userService;
            _authService = authServices;
        }

        [HttpPost("register")]
        [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
        [ProducesResponseType(typeof(GetProfileUserDtos), 200)]
        public async Task<GetProfileUserDtos> Register([FromBody] RegisterDtos body)
        {
            return await _userService.RegisterUser(body);
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
        [ProducesResponseType(typeof(AuthResponseDto), 200)]
        public async Task<AuthResponseDto> Login([FromBody] LoginDtos body)
        {
            return await _authService.Login(body);
        }

        [Authorize]
        [HttpPost("login-firebase")]
        [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
        [ProducesResponseType(typeof(string), 200)]
        public async Task<string> LoginFireBase([FromBody] LoginDtos body)
        {
            return await _authService.LoginWithFireBase(body);
        }
    }
}
