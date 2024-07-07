namespace OnDemandTutor.Models.Dtos.User;

public class DeaActiveAccountDto
{
    public int Id { get; set; }
    public bool IsActive { get; set; }
    public string? DeaActiveReason { get; set; }
}   