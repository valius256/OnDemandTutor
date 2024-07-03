using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using OnDemandTutor.BusinessLogic.Interfaces.Payment;
using OnDemandTutor.DataAccess.Repository;
using OnDemandTutor.Models.Dtos.Payment;
using OnDemandTutor.Models.Enum;

namespace OnDemandTutor.BusinessLogic.Services.Payment;

public class VnPayProcessor : IPaymentProcessor
{
    private readonly IConfiguration _configuration;

    public VnPayProcessor(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IPaymentResponse ProcessPaymentResponse(IQueryCollection collections)
    {
        var pay = new VnPayLibrary();
        var response = pay.GetFullResponseData(collections, _configuration["Vnpay:HashSecret"]);

        var rs=  new VnPayResponse
        {
            Success = response.Success,
            OrderId = response.TransactionCode,
            UserId = response.UserId,
            SlotId = response.SlotId,
             IsRechargePayment = response.IsRechargePayment, 
             Money = response.Money,
             PaymentMethod = response.PaymentMethod,
             OrderDescription = response.OrderDescription,
             VnPayResponseCode = response.VnPayResponseCode,
            PaymentStatus = response.Success ? PaymentStatus.Paid : PaymentStatus.Notpaid
        };
        return rs;
    }
}
