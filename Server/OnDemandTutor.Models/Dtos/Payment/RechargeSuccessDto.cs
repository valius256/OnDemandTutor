using OnDemandTutor.Models.Enum;

namespace OnDemandTutor.Models.Dtos.Payment;

public class RechargeSuccessDto
{
    public int UserId { get; set; }
    public decimal Amount { get; set; }
    public DateTime CreatedDate { get; set; }
    public string? Notes { get; set; }
    public string Method { get; set; }
    public PaymentStatus Status { get; set; }
}