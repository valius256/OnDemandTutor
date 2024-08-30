using OnDemandTutor.Models.Enum;

namespace OnDemandTutor.Models.Dtos.Transaction;

public class GetTransactionDto
{
    public string TransactionCode { get; set; } = string.Empty;
    public string PaymentMethod { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime CreatedDate { get; set; }
    public PaymentStatus? Status { get; set; }
    public TransactionType TransactionType { get; set; }
    public string? Notes { get; set; }
    public int? SlotId { get; set; }
    public int? ClassId { get; set; }
    public int CreatedById { get; set; }
}