using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnDemandTutor.API.Middlesware;
using OnDemandTutor.API.Models;
using OnDemandTutor.BusinessLogic.Interfaces.User;
using OnDemandTutor.Models.Dtos.User;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : BaseController<UserController>
{
    private readonly IUserServices _userService;

    public UserController(ILogger<UserController> logger, IUserServices userService) : base(logger)
    {
        _userService = userService;
    }

    [Authorize]
    [HttpGet("all")]
    [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
    [ProducesResponseType(typeof(PagedResult<GetProfileUserDtos>), 200)]
    public async Task<IApiResult<PagedResult<GetProfileUserDtos>>> GetAll([FromQuery] UserFilterDto request)
    {
        var result = await _userService.GetAllUsers(request);
        return OKAsync(result);
    }

    [Authorize]
    [HttpGet("profile")]
    [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
    [ProducesResponseType(typeof(GetProfileUserDtos), 200)]
    public async Task<IApiResult<GetProfileUserDtos>> GetProfile([FromQuery] int userId)
    {
        var result = await _userService.GetProfile(userId, null);
        return OKAsync(result);
    }

    [Authorize]
    [HttpPost("register-tutor")]
    [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
    [ProducesResponseType(typeof(GetProfileTutorDtos), 200)]
    public async Task<IApiResult<GetProfileTutorDtos>> RegisterTutor([FromBody] RegisterTutorDtos body)
    {
        var result = await _userService.RegisterTutor(body, HttpContext.User);
        return OKAsync(result);
    }

    [Authorize]
    [HttpPost("approve-tutor-registration")]
    [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
    [ProducesResponseType(typeof(IApiResult<List<TutorRegistrationResponseDtos>>), 200)]
    public async Task<IApiResult<bool>> ApprovedTutorRegis([FromBody] TutorRegistrationRequestDtos body)
    {
        return OKAsync(await _userService.ApprovedTutorRegistration(body, HttpContext.User));
        ;
    }


    /// <summary>
    ///    update user profile 
    /// </summary>
    /// if the fe don;t place Id in UpdateUserDto.Id it will take the id from the Claims when login scf
    /// <param Name="body"></param>
    /// <returns>boolean</returns>
    [Authorize]
    [HttpPost("update-profile")]
    [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
    [ProducesResponseType(typeof(bool), 200)]
    public async Task<IApiResult<bool>> UpdateProfile([FromBody] UpdateUserDto requestDto)
    {
        var result = await _userService.UpdateProfile(requestDto, HttpContext.User);
        return OKAsync(result);
    }

    [Authorize]
    [HttpPost("update-avatar")]
    [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
    [ProducesResponseType(typeof(bool), 200)]
    public async Task<IApiResult<bool>> UpdateAvatar([FromBody] string imageUrl)
    {
        var result = await _userService.UpdateAvatarImage(imageUrl, HttpContext.User);
        return OKAsync(result);
    }

    [AllowAnonymous]
    [HttpGet("view-tutor-list")]
    [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
    [ProducesResponseType(typeof(IApiResult<PagedResult<TutorSimpleProfileDto>>), 200)]
    public async Task<IApiResult<PagedResult<TutorSimpleProfileDto>>> ViewTutorList([FromQuery]
        TutorFilterDto request)
    {
        return OKAsync(await _userService.ViewTutorList(request));

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
        return OKAsync(await _userService.DeleteTutor(requestDto));
    }

    [AllowAnonymous] // sau sửa lại thành authorize r gán thêm operatorId vào 
    [HttpPatch("deactive-account")]
    [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
    [ProducesResponseType(typeof(IApiResult<bool>), 200)]
    public async Task<IApiResult<bool>> DeactiveAccount([FromBody] DeaActiveAccountDto requestDto)
    {
        return OKAsync(await _userService.DeaActiveAccount(requestDto));
    }

    [AllowAnonymous] // sau sửa lại thành authorize r gán thêm operatorId vào 
    [HttpPatch("active-account")]
    [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
    [ProducesResponseType(typeof(IApiResult<bool>), 200)]
    public async Task<IApiResult<bool>> ActiveAccount([FromBody] GetModelDto request)
    {
        return OKAsync(await _userService.ActiveAccount(request.Id));
    }

    [AllowAnonymous]
    [HttpPatch("change-status")] // sau sửa lại thành authorize r gán thêm operatorId vào 
    [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
    [ProducesResponseType(typeof(IApiResult<bool>), 200)]
    public async Task<IApiResult<bool>> ChangeStatusTutor([FromBody] GetModelDto request)
    {
        return OKAsync(await _userService.ActiveAccount(request.Id));
    }
}