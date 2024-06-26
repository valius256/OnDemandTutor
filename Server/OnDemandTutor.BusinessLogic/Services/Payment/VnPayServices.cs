using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using OnDemandTutor.BusinessLogic.Interfaces.Payment;
using OnDemandTutor.DataAccess;
using OnDemandTutor.DataAccess.Repository;
using OnDemandTutor.Models;
using OnDemandTutor.Models.Dtos.Payment;

namespace OnDemandTutor.BusinessLogic.Services.Payment;

public class VnPayServices : IVnPayServices
{
    private readonly VnPay _vnPay;
    private readonly IConfiguration _configuration;
    private readonly IUnitOfWorkRepository _unitOfWorkRepository;
    public VnPayServices(IOptions<VnPay> vnPay, IConfiguration configuration, IUnitOfWorkRepository unitOfWorkRepository)
    {
        _vnPay = vnPay.Value;
        _configuration = configuration;
        _unitOfWorkRepository = unitOfWorkRepository;  
    }
    
    public string CreatePaymentUrl(PaymentInformationModel model, HttpContext context)
    {
        var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById(_configuration["TimeZoneId"]);
        var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);
        var tick = DateTime.Now.Ticks.ToString();
        var pay = new VnPayLibrary();
        var urlCallBack = _configuration["PaymentCallBack:ReturnUrl"];

        pay.AddRequestData("vnp_Version", _vnPay.Version);
        pay.AddRequestData("vnp_Command", _vnPay.Command);
        pay.AddRequestData("vnp_TmnCode", _vnPay.TmnCode);
        pay.AddRequestData("vnp_Amount", ((int)model.Amount * 100).ToString());
        pay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
        pay.AddRequestData("vnp_CurrCode",_vnPay.CurrCode);
        pay.AddRequestData("vnp_IpAddr", pay.GetIpAddress(context));
        pay.AddRequestData("vnp_Locale",_vnPay.Locale);
        pay.AddRequestData("vnp_OrderInfo", $"{model.Email} {model.OrderDescription} {model.Amount}");
        pay.AddRequestData("vnp_OrderType", model.OrderType);
        pay.AddRequestData("vnp_ReturnUrl", urlCallBack);
        pay.AddRequestData("vnp_TxnRef", tick);
        pay.AddRequestData("vnp_Email", model.Email);
        
        var paymentUrl =
            pay.CreateRequestUrl(_configuration["Vnpay:BaseUrl"], _configuration["Vnpay:HashSecret"]);

        return paymentUrl;
    }

    public async Task<PaymentResponseModel> PaymentExecute(IQueryCollection collections)
    {
        var pay = new VnPayLibrary();
        var response = pay.GetFullResponseData(collections, _configuration["Vnpay:HashSecret"]);

       
        return response;
    }
}