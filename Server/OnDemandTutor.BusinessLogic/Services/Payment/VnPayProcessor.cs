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
        var hashSecret = _configuration["Vnpay:HashSecret"];
        if (hashSecret == null)
        {
            throw new ArgumentNullException("Hashsecret is null");
        }
        var response = pay.GetFullResponseData(collections, hashSecret);

        var rs = new VnPayResponse
        {
            Success = response.Success,
            OrderId = response.TransactionCode,
            UserId = response.UserId,
            ClassId = response.ClassId,
            SlotId = response.SlotId,
            IsRechargePayment = response.IsRechargePayment,
            Money = response.Money / 100,
            PaymentMethod = response.PaymentMethod,
            OrderDescription = response.OrderDescription,
            VnPayResponseCode = response.VnPayResponseCode,
            returnUrl = response.RedirectResult,
            PaymentStatus = response.Success ? PaymentStatus.Paid : PaymentStatus.Notpaid
        };
        return rs;
    }
}
