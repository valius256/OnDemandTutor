namespace OnDemandTutor.Models.Dtos.WithDrawDto;

public class RequestWithDrawDto
{
    public decimal Amount { get; set; }
    public string BankAccountNumber { get; set; }
    public string BankName { get; set; }
    public string? Reason { get; set; }
}