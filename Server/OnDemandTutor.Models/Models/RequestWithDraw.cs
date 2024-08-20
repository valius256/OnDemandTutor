using OnDemandTutor.Models.Enum;

namespace OnDemandTutor.Models.Models;

public class RequestWithDraw : BaseEntity
{
    public int UserId { get; set; }
    public decimal Amount { get; set; }
    public string BankAccountNumber { get; set; } = string.Empty;
    public string BankName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? OperatorId { get; set; }
    public string? Reply { get; set; }
    public WithDrawStatus Status { get; set; }

    public virtual User User { get; set; } = default!;
    public virtual User? Operator { get; set; }
}