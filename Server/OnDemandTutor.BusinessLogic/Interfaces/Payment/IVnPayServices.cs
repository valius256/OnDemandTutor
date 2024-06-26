using Microsoft.AspNetCore.Http;
using OnDemandTutor.Models.Dtos.Payment;

namespace OnDemandTutor.BusinessLogic.Interfaces.Payment;

public interface IVnPayServices
{
    string CreatePaymentUrl(PaymentInformationModel model, HttpContext context);
    Task<PaymentResponseModel> PaymentExecute(IQueryCollection collections);
}