namespace OnDemandTutor.Models.Dtos.Notification
{
    public class NotificationGetDto
    {
        public int Id { get; set; }
        public string? Content { get; set; }
        public int? ReceiverId { get; set; }
        public string? RefUrl { get; set; }
        public string? RefImageUrl { get; set; }
        public bool IsViewed { get; set; }
        public string? ReceiverName { get; set; } // Assuming User has a Name property

        public DateTime CreatedDate { get; set; }
    }
}

