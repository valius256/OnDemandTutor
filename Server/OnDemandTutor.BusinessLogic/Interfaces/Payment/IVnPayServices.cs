using Microsoft.AspNetCore.Http;
using OnDemandTutor.Models.Dtos.Class;
using OnDemandTutor.Models.Dtos.Payment;
using OnDemandTutor.Models.Dtos.Slot;

namespace OnDemandTutor.BusinessLogic.Interfaces.Payment;

public interface IVnPayServices
{
    Task<string> CreatePaymentForSlotUrl(PaySlotDto model, HttpContext context, GetSlotsDtos slot);
    Task<PaymentSlotResponseModel> PaymentExecute(IQueryCollection collections);

    Task<string> RechargePaymentAsync(RechargeDto model, HttpContext context);

    // Task<bool> ProcessCashbackAsync(CashBackDto cashbackDto, HttpContext context);
    Task<string> CreatePaymentForClassUrl(PayClassDto model, HttpContext context, GetClassFullDataSlotDto classDto);
    Task CreatePaymentForSlotByUserBalance(PaySlotDto model, HttpContext context);
}