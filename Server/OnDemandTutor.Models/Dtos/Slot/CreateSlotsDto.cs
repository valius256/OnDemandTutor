using OnDemandTutor.Models.Enum;

namespace OnDemandTutor.Models.Dtos.Slot
{
    public class CreateSlotsDto
    {
        public DateTime StartTime { get; set; }
        public int CreateById { get; set; }
        public DateTime EndTime { get; set; }
        public string TeachAddress { get; set; }
        public int? ClassId { get; set; }
        public int? SubjectId { get; set; }
        public bool IsOnline { get; set; }
        public int NumberOfStudents { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public DateTime? ActualEndTime { get; set; }
    }
}

