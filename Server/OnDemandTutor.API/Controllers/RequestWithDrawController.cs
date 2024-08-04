using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnDemandTutor.API.Middlesware;
using OnDemandTutor.BusinessLogic.Interfaces.Auth;
using OnDemandTutor.BusinessLogic.Interfaces.RequestWithDraw;
using OnDemandTutor.Models.Dtos.WithDrawDto;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RequestWithDrawController : BaseController<RequestWithDrawController>
{
    private readonly IAuthServices _authServices;
    private readonly IRequestWithDrawServices _requestWithDrawServices;

    public RequestWithDrawController(ILogger<RequestWithDrawController> logger,
        IRequestWithDrawServices requestWithDrawServices, IAuthServices authServices) : base(logger)
    {
        _requestWithDrawServices = requestWithDrawServices;
        _authServices = authServices;
    }

    [Authorize]
    [HttpGet("all")]
    [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
    [ProducesResponseType(typeof(PagedResult<GetRequestWithdrawDto>), 200)]
    public async Task<IActionResult> ViewRequestWithDraw([FromQuery] RequestWithDrawFilterDto request)
    {
        var user = await _authServices.GetUserProfileByClaim(HttpContext.User);
        var result = await _requestWithDrawServices.ViewAllRequestWithDraw(request, user);
        return Ok(result);
    }

    [Authorize]
    [HttpGet("admin-get-all")]
    [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
    [ProducesResponseType(typeof(PagedResult<GetRequestWithdrawDto>), 200)]
    public async Task<IActionResult> ViewRequestWithDrawAsAdmin([FromQuery] RequestWithDrawFilterDto request)
    {
        var result = await _requestWithDrawServices.ViewAllRequestWithDrawAsAdmin(request);
        return Ok(result);
    }


    [Authorize]
    [HttpPost("create-withdraw")]
    [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
    [ProducesResponseType(typeof(bool), 200)]
    public async Task<IActionResult> CreateWithdrawRequest([FromBody] CreateRequestWithdrawDto request)
    {
        var user = await _authServices.GetUserProfileByClaim(HttpContext.User);
        var result = await _requestWithDrawServices.CreateWithdrawRequest(request, user);
        return Ok(result);
    }

    [Authorize(Roles = "Admin, Operator")]
    [HttpPost("approve")]
    [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
    [ProducesResponseType(typeof(bool), 200)]
    public async Task<IActionResult> ApproveWithDraw([FromBody] ApproveWithDrawDto request)
    {
        var user = await _authServices.GetUserProfileByClaim(HttpContext.User);
        var result = await _requestWithDrawServices.ApproveWithDraw(request, user);
        return Ok(result);
    }
}