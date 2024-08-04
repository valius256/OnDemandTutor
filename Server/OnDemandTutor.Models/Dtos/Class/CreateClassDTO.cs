using OnDemandTutor.Models.Dtos.Slot;

namespace OnDemandTutor.Models.Dtos.Class
{
    public class CreateClassDTO
    {
        public string? Name { get; set; }
        public int SubjectId { get; set; }
        public string? Location { get; set; }
        public string? Method { get; set; }
        public int NumberOfStudents { get; set; }
        public List<CreateClassSlotDto> SlotList { get; set; } = new List<CreateClassSlotDto>();
    }
}

