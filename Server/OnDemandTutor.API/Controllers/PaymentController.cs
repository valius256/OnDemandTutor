using Microsoft.AspNetCore.Mvc;
using OnDemandTutor.API.Middlesware;
using OnDemandTutor.API.Models;
using OnDemandTutor.BusinessLogic.Interfaces.Payment;
using OnDemandTutor.Models.Dtos.Payment;
using OnDemandTutor.Models.Dtos.Slot;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.API.Controllers;


[Route("api/[controller]")]
[ApiController] 
public class PaymentController : BaseController<PaymentController>
{
    private readonly IVnPayServices _vnPayServices;
    public PaymentController(ILogger<PaymentController> logger, IVnPayServices vnPayServices) : base(logger)
    {
        _vnPayServices = vnPayServices;
    }
    
    //[Authorize]
    [HttpPost("create-payment")]
    [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
    [ProducesResponseType(typeof(IApiResult<string>), 200)]
    public async Task<IApiResult<string>> CreatePaymentUrl([FromBody] PaymentInformationModel requestDtos)
    {
        var url = _vnPayServices.CreatePaymentUrl(requestDtos, HttpContext);
        return OKAsync(url);
    }

    [HttpPost("execute")]
    [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
    [ProducesResponseType(typeof(IApiResult<PaymentResponseModel>), 200)]
    public async Task<IApiResult<PaymentResponseModel>> PaymentExecute([FromBody] IQueryCollection collections)
    {
        var response = await _vnPayServices.PaymentExecute(Request.Query);
        return OKAsync(response);
    }
}