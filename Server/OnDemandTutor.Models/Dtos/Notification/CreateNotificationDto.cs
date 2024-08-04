namespace OnDemandTutor.Models.Dtos.Notification;

public class CreateNotificationDto
{
    public string? Content { get; set; }
    public List<int> ReceiverIds { get; set; } = new();
    public string? RefUrl { get; set; }
    public string? RefImageUrl { get; set; }
}