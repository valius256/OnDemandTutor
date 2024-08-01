using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnDemandTutor.API.Middlesware;
using OnDemandTutor.API.Models;
using OnDemandTutor.BusinessLogic.Interfaces.Auth;
using OnDemandTutor.BusinessLogic.Interfaces.User;
using OnDemandTutor.Models.Dtos.Authen;
using OnDemandTutor.Models.Dtos.Register;
using OnDemandTutor.Models.Dtos.User;

namespace OnDemandTutor.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : BaseController<AuthController>
{
    private readonly IAuthServices _authServices;
    private readonly IUserServices _userServices;

    public AuthController(IUserServices userService, IAuthServices authServices, ILogger<AuthController> logger) : base(logger)
    {
        _userServices = userService;
        _authServices = authServices;
    }

    [HttpPost("register")]
    [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
    [ProducesResponseType(typeof(IApiResult<GetProfileUserDtos>), 200)]
    public async Task<IApiResult<GetProfileUserDtos>> Register([FromBody] RegisterDtos body)
    {
        return OKAsync(await _userServices.RegisterUser(body));
    }


    /// <summary>
    ///     login with facebook , later
    /// </summary>
    /// <param Name="body"></param>
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
    [ProducesResponseType(typeof(IApiResult<AuthenResponseDto>), 200)]
    public async Task<IApiResult<AuthenResponseDto>> LoginFireBase([FromBody] LoginDtos body)
    {
        return OKAsync(await _authServices.LoginWithFireBase(body));
    }

    [AllowAnonymous]
    [HttpPost("forgot-password")]
    [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
    [ProducesResponseType(typeof(IApiResult<string>), 200)]
    public async Task<IApiResult<string>> ForgotPassword(ForgotPasswordRequest request)
    {
        return OKAsync(await _authServices.ForgotPassword(request.Email));

    }

    [Authorize("All")]
    [HttpGet("who-am-i")]
    [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
    [ProducesResponseType(typeof(IApiResult<GetProfileUserDtos>), 200)]
    public async Task<IApiResult<GetProfileUserDtos>> GetProfile()
    {
        return OKAsync(await _authServices.GetUserProfileByClaim(HttpContext.User));
    }

    //[AllowAnonymous]
    [Authorize]
    [HttpPost("delete")]
    [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
    [ProducesResponseType(typeof(IApiResult<bool>), 200)]
    public async Task<IApiResult<bool>> DeleteUser([FromBody] string userEmail)
    {
        return OKAsync(await _authServices.DeleteUserAsync(userEmail));
    }

    [HttpPost("grant-role")]
    [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
    [ProducesResponseType(typeof(IApiResult<string>), 200)]
    public async Task<IApiResult<string>> GrantRole([FromBody] GrantRoleDto request)
    {
        return OKAsync(await _authServices.GrantRole(request));
    }

    [HttpPost("change-password")]
    [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
    [ProducesResponseType(typeof(IApiResult<bool>), 200)]
    public async Task<IApiResult<bool>> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
    {
        return OKAsync(await _authServices.ChangePasswordAsync(HttpContext.User, changePasswordDto));
    }
}