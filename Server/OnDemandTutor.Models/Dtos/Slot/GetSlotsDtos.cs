using OnDemandTutor.Models.Dtos.Class;
using OnDemandTutor.Models.Dtos.Subject;
using OnDemandTutor.Models.Dtos.User;
using OnDemandTutor.Models.Enum;

namespace OnDemandTutor.Models.Dtos.Slot
{
    public class GetSlotsDtos
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public int CreateById { get; set; }
        public DateTime EndTime { get; set; }
        public string? TeachAddress { get; set; }
        public int? ClassId { get; set; }
        public int? SubjectId { get; set; }
        public bool IsOnline { get; set; }
        public int NumberOfStudents { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public DateTime? ActualEndTime { get; set; }
        public SlotStatus SlotStatus { get; set; }
        public GetSubjectDtos Subject { get; set; } = new GetSubjectDtos();
        public GetClassDtos Class { get; set; } = new GetClassDtos();
        public GetTutorDto CreatedBy { get; set; } = new GetTutorDto();
    }
}
