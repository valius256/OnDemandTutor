using Microsoft.AspNetCore.Http;
using OnDemandTutor.Models.Dtos.Payment;
using OnDemandTutor.Models.Dtos.Slot;

namespace OnDemandTutor.BusinessLogic.Interfaces.Payment;

public interface IVnPayServices
{
    Task<string> CreatePaymentForSlotUrl(PaySlotDto model, HttpContext context, GetSlotsDtos slot);
    Task<PaymentSlotResponseModel> PaymentExecute(IQueryCollection collections);
}