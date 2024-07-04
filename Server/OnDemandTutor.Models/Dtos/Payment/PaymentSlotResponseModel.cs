using OnDemandTutor.Models.Enum;

namespace OnDemandTutor.Models.Dtos.Payment;

public class PaymentSlotResponseModel
{
    public string OrderDescription { get; set; }
    public int TransactionId { get; set; }
    public string TransactionCode { get; set; }
    public decimal Money { get; set; }
    public string PaymentMethod { get; set; }
    public bool Success { get; set; }
    public string Token { get; set; }
    public string VnPayResponseCode { get; set; }
    public int? SlotId { get; set; }
    public int UserId { get; set; }
    public bool IsRechargePayment { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
}