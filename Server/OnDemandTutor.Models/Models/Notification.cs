namespace OnDemandTutor.Models.Models;

public class Notification : BaseEntity
{
    public int Id { get; set; }
    public string? Content { get; set; }
    public int? ReceiverId { get; set; }
    public string? RefUrl { get; set; }
    public string? RefImageUrl { get; set; }
    public bool IsViewed { get; set; }
    public virtual User Receiver { get; set; }
}