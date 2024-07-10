namespace OnDemandTutor.Models.Dtos.Payment;

public class PaySlotDto
{
    public string? OrderDescription { get; set; } = string.Empty;
    public double Price { get; set; }
    public double Time { get; set; }
    public int? SlotId { get; set; }
    public string? ReturnUrl { get; set; }

}