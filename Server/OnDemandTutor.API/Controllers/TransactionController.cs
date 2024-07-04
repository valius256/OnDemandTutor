using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnDemandTutor.API.Middlesware;
using OnDemandTutor.BusinessLogic.Interfaces.Transaction;
using OnDemandTutor.Models.Dtos.ConsultationRequestDtos;
using OnDemandTutor.Models.Dtos.Transaction;

namespace OnDemandTutor.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TransactionController : BaseController<TransactionController>
{
    private readonly ITransactionServices _transactionServices;
    public TransactionController(ILogger<TransactionController> logger, ITransactionServices transactionServices) : base(logger)
    {
        _transactionServices = transactionServices;
    }

    // [AllowAnonymous]
    [Authorize]
    [HttpGet("all")]
    [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
    [ProducesResponseType(typeof(GetConsultationRequestDto), 200)]
    public async Task<ActionResult> RegisterForConsultation([FromQuery] TransactionFilterDto requestDtos)
    {
        return Ok(await _transactionServices.ViewALlTransaction(requestDtos, HttpContext.User));
    }

}