using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnDemandTutor.API.Middlesware;
using OnDemandTutor.BusinessLogic.Interfaces.RequestWithDraw;
using OnDemandTutor.Models.Dtos.WithDrawDto;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RequestWithDrawController : BaseController<RequestWithDrawController>
{
    private readonly IRequestWithDrawServices _requestWithDrawServices;
    public RequestWithDrawController(ILogger<RequestWithDrawController> logger, IRequestWithDrawServices requestWithDrawServices) : base(logger)
    {
        _requestWithDrawServices = requestWithDrawServices;
    }

    [Authorize]
    [HttpGet("all")]
    [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
    [ProducesResponseType(typeof(PagedResult<GetRequestWithdrawDto>), 200)]
    public async Task<IActionResult> ViewRequestWithDraw([FromQuery] RequestWithDrawFilterDto request)
    {

        var result = await _requestWithDrawServices.ViewAllRequestWithDraw(request, HttpContext.User);
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
        var result = await _requestWithDrawServices.CreateWithdrawRequest(request, HttpContext.User);
        return Ok(result);
    }

    [Authorize]
    [HttpPost("approve")]
    [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
    [ProducesResponseType(typeof(bool), 200)]
    public async Task<IActionResult> ApproveWithDraw([FromBody] ApproveWithDrawDto request)
    {
        var result = await _requestWithDrawServices.ApproveWithDraw(request, HttpContext.User);
        return Ok(result);
    }
    
    
}