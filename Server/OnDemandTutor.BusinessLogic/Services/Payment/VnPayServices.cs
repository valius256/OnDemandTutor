using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using OnDemandTutor.BusinessLogic.Interfaces.Payment;
using OnDemandTutor.BusinessLogic.Interfaces.SlotStudent;
using OnDemandTutor.BusinessLogic.Interfaces.Transaction;
using OnDemandTutor.BusinessLogic.Interfaces.User;
using OnDemandTutor.DataAccess;
using OnDemandTutor.DataAccess.Repository;
using OnDemandTutor.Models;
using OnDemandTutor.Models.Dtos.Payment;
using OnDemandTutor.Models.Dtos.Slot;
using OnDemandTutor.Models.Dtos.Transaction;
using OnDemandTutor.Models.Enum;

namespace OnDemandTutor.BusinessLogic.Services.Payment;

public class VnPayServices : IVnPayServices
{
    private readonly VnPay _vnPay;
    private readonly IConfiguration _configuration;
    private readonly IUnitOfWorkRepository _unitOfWorkRepository;
    private readonly ITransactionServices _transactionServices;
    private readonly ISlotStudentServices _slotStudentServices;
    private readonly IPaymentProcessor _paymentProcessor;
    private readonly IUserServices _userServices;

    public VnPayServices(IOptions<VnPay> vnPay, IConfiguration configuration,
        IUnitOfWorkRepository unitOfWorkRepository, ITransactionServices transactionServices,
        ISlotStudentServices slotStudentServices, IPaymentProcessor paymentProcessor, IUserServices userServices)
    {
        _vnPay = vnPay.Value;
        _configuration = configuration;
        _unitOfWorkRepository = unitOfWorkRepository;
        _transactionServices = transactionServices;
        _slotStudentServices = slotStudentServices;
        _paymentProcessor = paymentProcessor;
        _userServices = userServices;
    }

    public async Task<string> CreatePaymentForSlotUrl(PaySlotDto model, HttpContext context, GetSlotsDtos slot)
    {
        var tick = DateTime.Now.Ticks.ToString();
        var paymentUrl = CreateVnPayRequest(model, context, slot.Id, (decimal)(model.Price * model.Time), model.OrderDescription, false, tick);

        var transactionDto = CreateTransactionDto(tick, "Vnpay-bankcode", (decimal)(model.Price * model.Time), model.OrderDescription, slot.Id, context);
        await _transactionServices.CreateTransactionDb(transactionDto);


        return paymentUrl;
    }

    public async Task<PaymentSlotResponseModel> PaymentExecute(IQueryCollection collections)
    {
        var response = _paymentProcessor.ProcessPaymentResponse(collections);
        int transactionId = 0;
        if (response.Success)
        {
            transactionId = await _transactionServices.TransactionPaid(response.OrderId, DateTime.UtcNow);

            if (response.SlotId != null)
            {
                if (await _slotStudentServices.GetSlotStudentAsync(response.SlotId.Value, response.UserId) != null)
                {
                    await _slotStudentServices.SlotStudentPaidAsync(response.SlotId.Value, response.UserId);
                }
            }
            else
            {
                await _userServices.RechargeAccount(response.UserId, response.Money);
            }

        }

        response.PaymentStatus = PaymentStatus.Paid;
        return new PaymentSlotResponseModel
        {
            PaymentStatus = response.PaymentStatus,
            SlotId = response.SlotId,
            TransactionCode = response.OrderId,
            UserId = response.UserId,
            TransactionId = transactionId,
            Money = response.Money,
            Success = response.Success,
            PaymentMethod = response.PaymentMethod,
            VnPayResponseCode = response.VnPayResponseCode,
            IsRechargePayment = response.IsRechargePayment,
            OrderDescription = response.OrderDescription
        };

    }

    public async Task<string> RechargePaymentAsync(RechargeDto model, HttpContext context)
    {
        var tick = DateTime.Now.Ticks.ToString();
        var paymentUrl = CreateVnPayRequest(model, context, null, model.Amount, model.Notes, true, tick);

        var transactionDto = CreateTransactionDto(tick, "Vnpay-bankcode", model.Amount, model.Notes, null, context);
        await _transactionServices.CreateTransactionDb(transactionDto);


        return paymentUrl;
    }

    private string CreateVnPayRequest<T>(T model, HttpContext context, int? slotId, decimal amount, string? description, bool? isRechargePayment, string tick)
    {
        var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById(_configuration["TimeZoneId"]);
        var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);
        var pay = new VnPayLibrary();

        var currUid = context.User.FindFirst(c => c.Type == "id")?.Value;

        pay.AddRequestData("vnp_Version", _vnPay.Version);
        pay.AddRequestData("vnp_Command", _vnPay.Command);
        pay.AddRequestData("vnp_TmnCode", _vnPay.TmnCode);
        pay.AddRequestData("vnp_Amount", ((int)amount * 100).ToString());
        pay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
        pay.AddRequestData("vnp_CurrCode", _vnPay.CurrCode);
        pay.AddRequestData("vnp_IpAddr", pay.GetIpAddress(context));
        pay.AddRequestData("vnp_Locale", _vnPay.Locale);
        pay.AddRequestData("vnp_OrderInfo", $"{isRechargePayment}|{description}|{currUid}|{slotId}");
        pay.AddRequestData("vnp_OrderType", "other");
        pay.AddRequestData("vnp_ReturnUrl", "https://localhost:7142/api/Payment/execute");
        pay.AddRequestData("vnp_TxnRef", tick);
        pay.AddRequestData("vnp_ExpireDate", timeNow.AddMinutes(20).ToString("yyyyMMddHHmmss"));

        var paymentUrl = pay.CreateRequestUrl(_vnPay.BaseUrl, _vnPay.HashSecret);
        return paymentUrl;
    }

    private TransactionDto CreateTransactionDto(string tick, string paymentMethod, decimal amount, string? notes, int? slotId, HttpContext context)
    {
        var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById(_configuration["TimeZoneId"]);
        var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);
        var currUid = context.User.FindFirst(c => c.Type == "id")?.Value;

        return new TransactionDto
        {
            TransactionCode = tick,
            PaymentMethod = paymentMethod,
            Amount = amount * 100,
            Notes = notes,
            SlotId = slotId.HasValue ? slotId.Value : null,
            Status = PaymentStatus.Notpaid,
            CreatedDate = timeNow,
            CreatedById = int.Parse(currUid),
        };
    }
}
