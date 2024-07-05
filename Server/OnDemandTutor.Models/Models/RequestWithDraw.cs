using OnDemandTutor.Models.Enum;

namespace OnDemandTutor.Models.Models;

public class RequestWithDraw : BaseEntity
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public decimal Amount { get; set; }
    public string BankAccountNumber { get; set; }
    public string BankName { get; set; }
    public string? Description { get; set; }
    public int? OperatorId { get; set; }
    public string? Reply { get; set; }
    public WithDrawStatus Status { get; set; }

    public virtual User User { get; set; }
    public virtual User? Operator { get; set; }
}