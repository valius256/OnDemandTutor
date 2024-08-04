using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnDemandTutor.API.Middlesware;
using OnDemandTutor.BusinessLogic.Interfaces.Auth;
using OnDemandTutor.BusinessLogic.Interfaces.Transaction;
using OnDemandTutor.Models.Dtos.ConsultationRequestDtos;
using OnDemandTutor.Models.Dtos.Transaction;

namespace OnDemandTutor.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TransactionController : BaseController<TransactionController>
{
    private readonly IAuthServices _authService;
    private readonly ITransactionServices _transactionServices;

    public TransactionController(ILogger<TransactionController> logger, ITransactionServices transactionServices,
        IAuthServices authServices) : base(logger)
    {
        _authService = authServices;
        _transactionServices = transactionServices;
    }

    [Authorize]
    [HttpGet("all")]
    [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
    [ProducesResponseType(typeof(GetConsultationRequestDto), 200)]
    public async Task<ActionResult> ViewAllTransaction([FromQuery] TransactionFilterDto requestDtos)
    {
        var user = await _authService.GetUserProfileByClaim(HttpContext.User);
        return Ok(await _transactionServices.ViewALlTransaction(requestDtos, user));
    }

    [Authorize]
    [HttpGet("all-admin")]
    [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
    [ProducesResponseType(typeof(GetConsultationRequestDto), 200)]
    public async Task<ActionResult> ViewAllTransactionAsAdmin([FromQuery] TransactionFilterDto requestDtos)
    {
        return Ok(await _transactionServices.ViewALlTransactionAsAdmmin(requestDtos));
    }

    [Authorize]
    [HttpGet("get-by-id")]
    [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
    [ProducesResponseType(typeof(GetConsultationRequestDto), 200)]
    public async Task<ActionResult> GetTransactionById([FromQuery] int id)
    {
        var user = await _authService.GetUserProfileByClaim(HttpContext.User);
        return Ok(await _transactionServices.GetTransactionById(id, user));
    }
}