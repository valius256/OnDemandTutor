using Microsoft.AspNetCore.Http;
using OnDemandTutor.Models.Dtos.Payment;

namespace OnDemandTutor.BusinessLogic.Interfaces.Payment;

public interface IPaymentProcessor
{
    IPaymentResponse ProcessPaymentResponse(IQueryCollection collections);
}