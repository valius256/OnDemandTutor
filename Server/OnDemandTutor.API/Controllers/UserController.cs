using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnDemandTutor.API.Middlesware;
using OnDemandTutor.BusinessLogic.Interfaces.User;
using OnDemandTutor.Models.Dtos;
using OnDemandTutor.Models.Dtos.User;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserServices _userService;

    public UserController(IUserServices userService)
    {
        _userService = userService;
    }

    [Authorize]
    [HttpGet("all")]
    [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
    [ProducesResponseType(typeof(List<GetProfileUserDtos>), 200)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _userService.GetAllUsers();
        return Ok(result);
    }

    [Authorize]
    [HttpGet("profile")]
    [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
    [ProducesResponseType(typeof(GetProfileUserDtos), 200)]
    public async Task<IActionResult> GetProfile([FromBody] int userId)
    {
        var result = await _userService.GetProfile(userId, null);
        return Ok(result);
    }

    [Authorize]
    [HttpPost("register-tutor")]
    [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
    [ProducesResponseType(typeof(GetProfileTutorDtos), 200)]
    public async Task<IActionResult> RegisterTutor([FromBody] RegisterTutorDtos body)
    {
        var result = await _userService.RegisterTutor(body, HttpContext.User);
        return Ok(result);
    }

    [Authorize]
    [HttpPost("approve-tutor-registration")]
    [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
    [ProducesResponseType(typeof(GetProfileTutorDtos), 200)]
    public async Task<IActionResult> ApprovedTutorRegis([FromBody] TutorRegistrationRequestDtos body)
    {
        await _userService.ApprovedTutorRegistration(body, HttpContext.User);
        // return Ok(result);
        return Ok();
    }

    [Authorize]
    [HttpPost("update-profile")]
    [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
    [ProducesResponseType(typeof(GetProfileTutorDtos), 200)]
    public async Task<IActionResult> UpdateProfile([FromBody] RegisterTutorDtos body)
    {
        // var result = await _userService.RegisterTutor(body);
        // return Ok(result);
        return Ok();
    }

    [Authorize]
    [HttpPost("view-tutor-list")]
    [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
    [ProducesResponseType(typeof(List<TutorSimpleProfileDtos>), 200)]
    public async Task<IActionResult> ViewTutorList(PagingModel<TutorSimpleProfileRequest> request)
    {
        var result = await _userService.ViewTutorList(request);
        return Ok(result);
    }
}