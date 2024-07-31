using OnDemandTutor.Models.Enum;

namespace OnDemandTutor.Models.Dtos.Slot
{
    public class UpdateSlotDto
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string TeachAddress { get; set; } = string.Empty;
        public int? SubjectId { get; set; }
        public bool IsOnline { get; set; }
        public int NumberOfStudents { get; set; }
    }
}

