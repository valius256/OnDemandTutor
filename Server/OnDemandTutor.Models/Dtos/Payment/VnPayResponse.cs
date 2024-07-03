using OnDemandTutor.Models.Enum;

namespace OnDemandTutor.Models.Dtos.Payment;

public class VnPayResponse : IPaymentResponse
{
    public bool Success { get; set; }
    public string OrderId { get; set; }
    public int UserId { get; set; }
    public int? SlotId { get; set; }
    public bool IsRechargePayment { get; set; } 
    public string PaymentMethod { get; set; }   
    public PaymentStatus PaymentStatus { get; set; }
    public decimal Money { get; set; }
    public string OrderDescription { get; set; }
    public string VnPayResponseCode { get; set; }
 
}
