using OnDemandTutor.Models.Enum;

namespace OnDemandTutor.Models.Dtos.Payment;

public class VnPayResponse : IPaymentResponse
{
    public bool Success { get; set; }
    public string OrderId { get; set; } = string.Empty;
    public int UserId { get; set; }
    public List<int>? SlotId { get; set; }
    public int? ClassId { get; set; }
    public bool IsRechargePayment { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public PaymentStatus PaymentStatus { get; set; }
    public decimal Money { get; set; }
    public string? returnUrl { get; set; }
    public string OrderDescription { get; set; } = string.Empty;
    public string VnPayResponseCode { get; set; } = string.Empty ;

}
