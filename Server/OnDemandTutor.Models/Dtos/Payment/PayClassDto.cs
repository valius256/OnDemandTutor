namespace OnDemandTutor.Models.Dtos.Payment;

public class PayClassDto
{
    public string? OrderDescription { get; set; } = string.Empty;
    public int ClassId { get; set; }
    public bool IsFullPay { get; set; } // cọc 20%
    public string? returnPage { get; set; }
}