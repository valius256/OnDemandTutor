using System.Reactive;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnDemandTutor.API.Middlesware;
using OnDemandTutor.API.Models;
using OnDemandTutor.BusinessLogic.Interfaces.Auth;
using OnDemandTutor.BusinessLogic.Interfaces.Notification;
using OnDemandTutor.BusinessLogic.Interfaces.User;
using OnDemandTutor.Models.Dtos.Notification;
using OnDemandTutor.Models.Dtos.User;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : BaseController<UserController>
{
    private readonly IUserServices _userService;
    private readonly IAuthServices _authServices;
    public UserController(ILogger<UserController> logger, IUserServices userService, IAuthServices authServices) : base(logger)
    {
        _userService = userService;
        _authServices = authServices;
    }

    // [Authorize]
    [HttpGet("all")]
    [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
    [ProducesResponseType(typeof(PagedResult<GetProfileUserDtos>), 200)]
    public async Task<IApiResult<PagedResult<GetProfileUserDtos>>> GetAll([FromQuery] UserFilterDto request)
    {
        var user = await _authServices.GetUserByClaimsNotRequired(HttpContext.User);
        var result = await _userService.GetAllUsersAsync(request, user);
       
        return OKAsync(result);
    }

    //[Authorize]
    [HttpGet("profile")]
    [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
    [ProducesResponseType(typeof(GetProfileUserDtos), 200)]
    public async Task<IApiResult<GetProfileUserDtos>> GetProfile([FromQuery] int userId)
    {
        var user = await _authServices.GetUserByClaimsNotRequired(HttpContext.User);
        var result = await _userService.GetProfileAsync(userId, null, user);
        
        return OKAsync(result);
    }

    [Authorize]
    [HttpGet("balance")]
    [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
    [ProducesResponseType(typeof(GetUserBalanceDto), 200)]
    public async Task<IApiResult<GetUserBalanceDto>> GetUserBalance()
    {
        var user = await _authServices.GetUserProfileByClaim(HttpContext.User);
        var result = await _userService.GetUserBalanceAsync(user.Id);
        return OKAsync(result);
    }

    //[Authorize]
    //[HttpPost("register-tutor")]
    //[ProducesResponseType(typeof(ApiErrorActionResult), 400)]
    //[ProducesResponseType(typeof(GetProfileTutorDtos), 200)]
    //public async Task<IApiResult<GetProfileTutorDtos>> RegisterTutor([FromBody] RegisterTutorDtos body)
    //{
    //    var result = await _userService.RegisterTutorAsync(body, HttpContext.User);
    //    return OKAsync(result);
    //}

    //[Authorize]
    //[HttpPost("approve-tutor-registration")]
    //[ProducesResponseType(typeof(ApiErrorActionResult), 400)]
    //[ProducesResponseType(typeof(IApiResult<List<TutorRegistrationResponseDtos>>), 200)]
    //public async Task<IApiResult<bool>> ApprovedTutorRegis([FromBody] TutorRegistrationRequestDtos body)
    //{
    //    return OKAsync(await _userService.ApprovedTutorRegistrationAsync(body, HttpContext.User));
    //    ;
    //}


    /// <summary>
    ///    update user profile 
    /// </summary>
    /// if the fe don;t place Id in UpdateUserDto.Id it will take the id from the Claims when login scf
    /// <param Name="body"></param>
    /// <param name="requestDto"></param>
    /// <returns>boolean</returns>
    [Authorize]
    [HttpPost("update-profile")]
    [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
    [ProducesResponseType(typeof(bool), 200)]
    public async Task<IApiResult<bool>> UpdateProfile([FromBody] UpdateUserDto requestDto)
    {
        var user = await _authServices.GetUserProfileByClaim(HttpContext.User);
        var result = await _userService.UpdateProfileAsync(requestDto, user);
        return OKAsync(result);
    }

    [Authorize]
    [HttpPost("update-avatar")]
    [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
    [ProducesResponseType(typeof(bool), 200)]
    public async Task<IApiResult<bool>> UpdateAvatar([FromBody] ChangeAvatarUrlDto request)
    {
        var user = await _authServices.GetUserProfileByClaim(HttpContext.User);
        var result = await _userService.UpdateAvatarImage(request.Url, user);
        return OKAsync(result);
    }

    [AllowAnonymous]
    [HttpGet("view-tutor-list")]
    [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
    [ProducesResponseType(typeof(IApiResult<PagedResult<TutorSimpleProfileDto>>), 200)]
    public async Task<IApiResult<PagedResult<TutorSimpleProfileDto>>> ViewTutorList([FromQuery]
        TutorFilterDto request)
    {
        return OKAsync(await _userService.ViewTutorListAsync(request));

    }

    [AllowAnonymous]
    [HttpGet("outstanding-tutors")]
    [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
    [ProducesResponseType(typeof(IApiResult<PagedResult<GetOutstandingTutorDto>>), 200)]
    public async Task<IApiResult<PagedResult<GetOutstandingTutorDto>>> GetOutstandingTutor([FromQuery]
        int limit = 10, [FromQuery] int page = 1)
    {
        return OKAsync(await _userService.GetOutstandingTutor(limit, page));

    }
    [Authorize]
    [HttpGet("all-operators")]
    [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
    [ProducesResponseType(typeof(IApiResult<List<GetSimpleUserDto>>), 200)]
    public async Task<IApiResult<List<GetSimpleUserDto>>> GetAllOperators()
    {
        return OKAsync(await _userService.GetAllOperators());

    }
    /// <summary>
    ///    update tutor status to Banned 
    /// </summary>
    /// <param ></param>
    /// 
    /// <returns>boolean</returns>
    [Authorize]
    [HttpPost("remove-tutor")]
    [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
    [ProducesResponseType(typeof(IApiResult<bool>), 200)]
    public async Task<IApiResult<bool>> DeleteTutor([FromBody] DeleteTutorDto requestDto)
    {
        return OKAsync(await _userService.DeleteTutorAsync(requestDto));
    }

    [Authorize]
    [HttpPatch("deactive-account")]
    [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
    [ProducesResponseType(typeof(IApiResult<bool>), 200)]
    public async Task<IApiResult<bool>> DeactiveAccount([FromBody] DeaActiveAccountDto requestDto)
    {
        return OKAsync(await _userService.DeaActiveAccountAsync(requestDto));
    }

    [Authorize]
    [HttpPatch("active-account")]
    [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
    [ProducesResponseType(typeof(IApiResult<bool>), 200)]
    public async Task<IApiResult<bool>> ActiveAccount([FromBody] GetModelDto request)
    {
        return OKAsync(await _userService.ActiveAccount(request.Id));
    }

    [Authorize]
    [HttpPatch("change-status")]
    [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
    [ProducesResponseType(typeof(IApiResult<CompareStatusDto>), 200)]
    public async Task<IApiResult<CompareStatusDto>> ChangeStatusTutor([FromBody] ChangeStatusDto request)
    {
        return OKAsync(await _userService.ChangeTutorStatus(request.Id, request.Status));
    }
}