using OnDemandTutor.Models.Enum;

namespace OnDemandTutor.Models.Models;

public class Transaction : BaseEntity
{
    public string TransactionCode { get; set; } = string.Empty;
    public string PaymentMethod { get; set; } = string.Empty ;
    public TransactionType TransactionType { get; set; }
    public decimal Amount { get; set; }
    public PaymentStatus Status { get; set; }
    public string? Notes { get; set; }
    public int? SlotId { get; set; }
    public int CreatedById { get; set; }
    public virtual User CreatedBy { get; set; } = default!;
    public virtual Slot? Slot { get; set; }
}