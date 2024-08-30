namespace OnDemandTutor.Models.Dtos.WithDrawDto;

public class CreateRequestWithdrawDto
{
    public decimal Amount { get; set; }
    public string BankAccountNumber { get; set; } = string.Empty;
    public string BankName { get; set; } = string.Empty;
    public string? Description { get; set; }
}