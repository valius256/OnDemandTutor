using OnDemandTutor.Models.Enum;

namespace OnDemandTutor.Models.Dtos.Payment;

public interface IPaymentResponse
{
    public bool Success { get; }
    public  string OrderId { get; }
    public int UserId { get; }
    public int? SlotId { get; }
    public decimal Money { get; }
    public PaymentStatus PaymentStatus { get; set; }
    public string OrderDescription { get; set;  }
    public bool IsRechargePayment { get; set; }
    public string PaymentMethod { get; set; }   
    public string VnPayResponseCode { get; set; } 
    
}
