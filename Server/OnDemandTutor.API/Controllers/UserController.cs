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
    [ProducesResponseType(typeof(List<GetProfileUserDtos>), 200)]
    public async Task<IApiResult<List<GetProfileUserDtos>>> GetAll([FromQuery] UserFilterDto request)
    {
        var result = await _userService.GetAllUsers(request);
        return OKAsync(result);
    }

    [Authorize]
    [HttpPost("profile")]
    [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
    [ProducesResponseType(typeof(GetProfileUserDtos), 200)]
    public async Task<IApiResult<GetProfileUserDtos>> GetProfile([FromBody] int userId)
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
    /// <param name="body"></param>
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
    [HttpGet("view-tutor-list")]
    [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
    [ProducesResponseType(typeof(IApiResult<PagedResult<TutorSimpleProfileDto>>), 200)]
    public async Task<IApiResult<List<TutorSimpleProfileDto>>> ViewTutorList([FromQuery]
        TutorFilterDto request)
    {
        return OKAsync(await _userService.ViewTutorList(request));

    }

    [Authorize]
    [HttpPost("remove-tutor")]
    [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
    [ProducesResponseType(typeof(IApiResult<bool>), 200)]
    public async Task<IApiResult<bool>> DeleteTutor([FromBody] DeleteTutorDto requestDto)
    {
        return OKAsync(await _userService.DeleteTutor(requestDto));
    }



}