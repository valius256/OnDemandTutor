using OnDemandTutor.Models.Enum;

namespace OnDemandTutor.Models.Dtos.Transaction;

public class TransactionDtos
{
    public required string TransactionCode { get; set; }
    public string PaymentMethod { get; set; }
    public decimal Amount { get; set; }
    public DateTime CreatedDate { get; set; }
    public PaymentStatus? Status { get; set; }
    public string? Notes { get; set; }
    public int SlotId { get; set; }
    public int CreatedById { get; set; }
}