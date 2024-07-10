namespace OnDemandTutor.Models.Dtos.WithDrawDto;

public class CreateRequestWithdrawDto
{
    public decimal Amount { get; set; }
    public string BankAccountNumber { get; set; }
    public string BankName { get; set; }
    public string? Description { get; set; }
}