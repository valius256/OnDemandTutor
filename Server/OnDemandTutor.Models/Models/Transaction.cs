using System.Transactions;
using OnDemandTutor.Models.Enum;

namespace OnDemandTutor.Models.Models;

public class Transaction : BaseEntity
{
    public int Id { get; set; }
    public required string TransactionCode { get; set; }
    public string PaymentMethod { get; set; }
    public decimal Amount { get; set; }
    public DateTime CreatedDate { get; set; }
    public PaymentStatus Status { get; set; }
    public string? Notes { get; set; }
    public int SlotId { get; set; }
    public int CreatedById { get; set; }
    public virtual User CreatedBy { get; set; }
    public virtual Slot Slot { get; set; }
}