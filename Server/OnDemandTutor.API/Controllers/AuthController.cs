using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnDemandTutor.API.Middlesware;
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
        private readonly IAuthServices _authServices;
        private readonly IUserServices _userServices;

        public AuthController(IUserServices userService, IAuthServices authServices)
        {
            _userServices = userService;
            _authServices = authServices;
        }

        [HttpPost("register")]
        [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
        [ProducesResponseType(typeof(GetProfileUserDtos), 200)]
        public async Task<ActionResult<GetProfileUserDtos>> Register([FromBody] RegisterDtos body)
        {
            return await _userServices.RegisterUser(body);
        }


        /// <summary>
        ///  login with facebook , later
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        //[HttpPost("login")]
        //[ProducesResponseType(typeof(ApiErrorActionResult), 400)]
        //[ProducesResponseType(typeof(AuthResponseDto), 200)]
        //public async Task<AuthResponseDto> Login([FromBody] LoginDtos body)
        //{
        //    return await _authService.Login(body);
        //}


        [HttpPost("login-firebase")]
        [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
        [ProducesResponseType(typeof(string), 200)]
        public async Task<ActionResult<string>> LoginFireBase([FromBody] LoginDtos body)
        {
            return await _authServices.LoginWithFireBase(body);
        }
        [AllowAnonymous]
        [HttpPost("forgot-password")]
        [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
        [ProducesResponseType(typeof(string), 200)]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest request)
        {
            var result = await _authServices.ForgotPassword(request.Email);
            return Ok(result);
        }

        [HttpGet("who-am-i")]
        [Authorize]
        [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
        [ProducesResponseType(typeof(GetProfileUserDtos), 200)]
        public async Task<IActionResult> GetProfile()
        {
            var result = await _authServices.GetUserProfileByClaim(HttpContext.User);
            return Ok(result);
        }

        //[AllowAnonymous]
        [Authorize]
        [HttpPost("delete")]
        [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<ActionResult<bool>> DeleteUser([FromBody] string userEmail)
        {
            return await _authServices.DeleteUserAsync(userEmail);
        }
    }
}
