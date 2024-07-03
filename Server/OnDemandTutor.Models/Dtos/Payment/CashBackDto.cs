namespace OnDemandTutor.Models.Dtos.Payment;

public class CashBackDto
{
    public decimal Money { get; set; }
    public int UserId { get; set; }
    public string Reason { get; set; }
}