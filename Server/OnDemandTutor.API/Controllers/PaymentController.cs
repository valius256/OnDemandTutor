using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnDemandTutor.API.Middlesware;
using OnDemandTutor.API.Models;
using OnDemandTutor.BusinessLogic.Interfaces.Class;
using OnDemandTutor.BusinessLogic.Interfaces.Payment;
using OnDemandTutor.BusinessLogic.Interfaces.Slot;
using OnDemandTutor.Models.Dtos.Payment;

namespace OnDemandTutor.API.Controllers;


[Route("api/[controller]")]
[ApiController]
public class PaymentController : BaseController<PaymentController>
{
    private readonly IVnPayServices _vnPayServices;
    private readonly ISlotServices _slotServices;
    private readonly IClassService _classService;
    public PaymentController(ILogger<PaymentController> logger, IVnPayServices vnPayServices, ISlotServices slotServices,
    IClassService classService
    ) : base(logger)
    {
        _vnPayServices = vnPayServices;
        _slotServices = slotServices;
        _classService = classService;
    }


    [HttpPost("create-payment-slot")]
    [Authorize]
    [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
    [ProducesResponseType(typeof(IApiResult<string>), 200)]
    public async Task<IActionResult> CreatePaymentForSlot([FromBody] PaySlotDto paymentInfo)
    {
        var paymentUrl = string.Empty;
        if (paymentInfo.SlotId != null)
        {
            var slot = await _slotServices.GetSlotByIdAsync(paymentInfo.SlotId.Value);

            paymentUrl = await _vnPayServices.CreatePaymentForSlotUrl(paymentInfo, HttpContext, slot);
        }
        return Ok(paymentUrl);
    }

    [Authorize]
    [HttpPost("create-payment-class/{classId}")]
    [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
    [ProducesResponseType(typeof(IApiResult<string>), 200)]
    public async Task<IActionResult> PurchaseClass(int classId)
    {
        var classDtos = _classService.GetClassByIdAsync(classId);
        if (classDtos == null)
            return BadRequest("Class not found");

        var classPaymentUrl = Task.CompletedTask;
        return Ok(classPaymentUrl);
    }

    [HttpGet("execute")] // demo xóa di
    public async Task<IActionResult> PaymentExecute()
    {
        var response = await _vnPayServices.PaymentExecute(Request.Query);
        var redirectTo = Redirect(response.RedirectResult);
        if (redirectTo == null)
        {
            return Ok(response);
        }
        return redirectTo;
    }

    [HttpPost("create-recharge")]
    [Authorize]
    [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
    [ProducesResponseType(typeof(IApiResult<string>), 200)]
    public async Task<IActionResult> CreateRecharge([FromBody] RechargeDto request)
    {
        var paymentUrl = await _vnPayServices.RechargePaymentAsync(request, HttpContext);
        return Ok(paymentUrl);
    }

}