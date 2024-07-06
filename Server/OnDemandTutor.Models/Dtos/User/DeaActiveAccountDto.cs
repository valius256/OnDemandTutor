namespace OnDemandTutor.Models.Dtos.User;

public class DeaActiveAccountDto
{
    public int Id;
    public bool IsActive;
    public string? DeaActiveReason { get; set; }
}   