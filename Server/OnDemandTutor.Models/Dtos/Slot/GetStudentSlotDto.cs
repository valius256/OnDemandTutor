using OnDemandTutor.Models.Enum;

namespace OnDemandTutor.Models.Dtos.Slot
{
    public class GetStudentSlotDto
    {
        public int Id { get; set; }
        public int SlotId { get; set; }
        public int UserId { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public decimal? Rating { get; set; }
        public string? Feedback { get; set; }
    }
}
