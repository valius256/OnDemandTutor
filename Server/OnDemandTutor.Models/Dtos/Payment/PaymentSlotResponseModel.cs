using OnDemandTutor.Models.Enum;

namespace OnDemandTutor.Models.Dtos.Payment;

public class PaymentSlotResponseModel
{
    public string OrderDescription { get; set; } = string.Empty;
    public int TransactionId { get; set; }
    public string TransactionCode { get; set; } = string.Empty;
    public decimal Money { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public bool Success { get; set; }
    public string Token { get; set; } = string.Empty;
    public string VnPayResponseCode { get; set; } = string.Empty;
    public List<int>? SlotId { get; set; }
    public int? ClassId { get; set; }
    public int UserId { get; set; }
    public bool IsRechargePayment { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
    public string RedirectResult { get; set; } = string.Empty;
}