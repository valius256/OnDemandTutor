
using OnDemandTutor.Models.Dtos.Slot;
using OnDemandTutor.Models.Enum;

namespace OnDemandTutor.Models.Dtos.Class
{
    public class UpdateClassDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? SubjectId { get; set; }
        public string? Location { get; set; }
        public string? Method { get; set; }
        public int? NumberOfStudents { get; set; }

        public List<CreateClassSlotDto> NewClassSlots { get; set; } = new List<CreateClassSlotDto>();
    }
}
