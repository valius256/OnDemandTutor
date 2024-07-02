using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using OnDemandTutor.BusinessLogic.Interfaces.Payment;
using OnDemandTutor.BusinessLogic.Interfaces.SlotStudent;
using OnDemandTutor.BusinessLogic.Interfaces.Transaction;
using OnDemandTutor.DataAccess;
using OnDemandTutor.DataAccess.IRepository;
using OnDemandTutor.DataAccess.Repository;
using OnDemandTutor.Models;
using OnDemandTutor.Models.Dtos.Payment;
using OnDemandTutor.Models.Dtos.Slot;
using OnDemandTutor.Models.Dtos.SlotStudent;
using OnDemandTutor.Models.Dtos.Transaction;
using OnDemandTutor.Models.Enum;
using OnDemandTutor.Models.Models;

namespace OnDemandTutor.BusinessLogic.Services.Payment;

public class VnPayServices : IVnPayServices
{
    private readonly VnPay _vnPay;
    private readonly IConfiguration _configuration;
    private readonly IUnitOfWorkRepository _unitOfWorkRepository;
    private readonly ITransactionServices _transactionServices;
    private readonly ISlotStudentServices _slotStudentServices;
    public VnPayServices(IOptions<VnPay> vnPay, IConfiguration configuration, 
        IUnitOfWorkRepository unitOfWorkRepository, ITransactionServices transactionServices,
            ISlotStudentServices slotStudentServices
        )
    {
        _vnPay = vnPay.Value;
        _configuration = configuration;
        _unitOfWorkRepository = unitOfWorkRepository;
        _transactionServices = transactionServices;
        _slotStudentServices = slotStudentServices;
    }
    
    public async Task<string> CreatePaymentForSlotUrl(PaySlotDto model, HttpContext context, GetSlotsDtos slot)
    {
        var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById(_configuration["TimeZoneId"]);
        var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);
        var tick = DateTime.Now.Ticks.ToString();
        var pay = new VnPayLibrary();
        
        
        var currUid = context.User.FindFirst(c => c.Type == "id")?.Value;
        
        
        pay.AddRequestData("vnp_Version", _vnPay.Version);
        pay.AddRequestData("vnp_Command", _vnPay.Command);
        pay.AddRequestData("vnp_TmnCode", _vnPay.TmnCode);
        pay.AddRequestData("vnp_Amount", ((int)model.Price * model.Time * 100).ToString()); 
        pay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
        pay.AddRequestData("vnp_CurrCode", _vnPay.CurrCode);
        pay.AddRequestData("vnp_IpAddr", pay.GetIpAddress(context));
        pay.AddRequestData("vnp_Locale", _vnPay.Locale);
        pay.AddRequestData("vnp_OrderInfo", $"{model.Email} {int.Parse(currUid)} {slot.Id} {model.OrderDescription}  {model.Time} ");
        pay.AddRequestData("vnp_OrderType", "other");
        pay.AddRequestData("vnp_ReturnUrl", "https://localhost:7142/api/Payment/execute");
        pay.AddRequestData("vnp_TxnRef", tick);
        pay.AddRequestData("vnp_ExpireDate", timeNow.AddMinutes(20).ToString("yyyyMMddHHmmss"));
        TransactionDto transactionDto = new TransactionDto()
        {
            TransactionCode = tick,
            PaymentMethod = "Vnpay-bankcode",
            Amount = (decimal)(model.Price * model.Time * 100),
            Notes = model.OrderDescription,
            SlotId = slot.Id,
            Status = PaymentStatus.Notpaid,
            CreatedDate = timeNow,
            CreatedById = int.Parse(currUid),
        };
        
        await _transactionServices.CreateTransactionDb(transactionDto);

     
        
        
        var paymentUrl = pay.CreateRequestUrl(_vnPay.BaseUrl, _vnPay.HashSecret);
        return paymentUrl;
    }

    public async Task<PaymentSlotResponseModel> PaymentExecute(IQueryCollection collections)
    {
        var pay = new VnPayLibrary();
        var response = pay.GetFullResponseData(collections, _configuration["Vnpay:HashSecret"]);
        
        if (response.Success)
        {
            await _transactionServices.TransactionPaid(response.OrderId, DateTime.UtcNow);
            if (await _slotStudentServices.GetSlotStudentAsync(response.SlotId, response.UserId) != null)
            {
                await _slotStudentServices.SlotStudentPaidAsync(response.SlotId, response.UserId);
            }
        }
        // implemnent them cac truong hop cua the sau base : https://sandbox.vnpayment.vn/apis/docs/bang-ma-loi/
        // if(response.VnPayResponseCode  == "01")
        
   

        response.PaymentStatus = PaymentStatus.Paid;
        return response;
    }
}