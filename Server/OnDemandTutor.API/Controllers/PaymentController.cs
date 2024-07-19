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
    private readonly IClassServices _classServices;
    public PaymentController(ILogger<PaymentController> logger, IVnPayServices vnPayServices, ISlotServices slotServices,
    IClassServices classServices
    ) : base(logger)
    {
        _vnPayServices = vnPayServices;
        _slotServices = slotServices;
        _classServices = classServices;
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

    [HttpPost("create-payment-slot-user-balance")]
    [Authorize]
    [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
    [ProducesResponseType(typeof(IApiResult<string>), 200)]
    public async Task<IActionResult> CreatePaymentForSlotByBalance([FromBody] PaySlotDto paymentInfo)
    {
        bool result;
        if (paymentInfo.SlotId != null)
        {
            var slot = await _slotServices.GetSlotByIdAsync(paymentInfo.SlotId.Value);

            result = await _vnPayServices.CreatePaymentForSlotByUserBalance(paymentInfo, HttpContext, slot);
            return Ok(result);
        }

        return BadRequest("cannot inittialize payment cause slot is null");
    }
    [Authorize]
    [HttpPost("create-payment-class")]
    [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
    [ProducesResponseType(typeof(IApiResult<string>), 200)]
    public async Task<IActionResult> PurchaseClass(PayClassDto request)
    {
        var classDto = await _classServices.GetClassWithFullDataSlotId(request.ClassId);
        if (classDto == null)
            return BadRequest("Class not found");

        var classPaymentUrl = await _vnPayServices.CreatePaymentForClassUrl(request, HttpContext, classDto);
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