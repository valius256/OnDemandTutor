using OnDemandTutor.Models.Enum;

namespace OnDemandTutor.Models.Dtos.Transaction;

public class TransactionDto
{
    public string TransactionCode { get; set; }
    public string PaymentMethod { get; set; }
    public decimal Amount { get; set; }
    public DateTime CreatedDate { get; set; }
    public PaymentStatus? Status { get; set; }
    public TransactionType TransactionType { get; set; }
    public string? Notes { get; set; }
    public int? SlotId { get; set; }
    public int? ClassId { get; set; }
    public int CreatedById { get; set; }
}