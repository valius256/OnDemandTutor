namespace OnDemandTutor.Models.Dtos.Notification
{
    public class NotificationCreateDto
    {
        public string? Content { get; set; }
        public List<int>? ReceiverId { get; set; }
        public string? RefUrl { get; set; }
        public string? RefImageUrl { get; set; }
        public bool IsViewed { get; set; }
    }
}

