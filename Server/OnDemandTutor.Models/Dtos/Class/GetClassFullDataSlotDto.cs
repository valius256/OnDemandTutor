using OnDemandTutor.Models.Dtos.Slot;
using OnDemandTutor.Models.Dtos.StudentClass;
using OnDemandTutor.Models.Dtos.Subject;
using OnDemandTutor.Models.Dtos.User;
using OnDemandTutor.Models.Enum;

namespace OnDemandTutor.Models.Dtos.Class;

public class GetClassFullDataSlotDto
{
    public List<GetSimpleSlotDto> Slots = new();
    public List<GetStudentClassWithStudentDto> StudentClasses = new();
    public int Id { get; set; }
    public string? Name { get; set; }
    public int TutorId { get; set; }
    public int SubjectId { get; set; }
    public string? Location { get; set; }
    public string? Method { get; set; }
    public ClassStatus Status { get; set; }

    public GetSubjectDtos Subject { get; set; } = default!;
    public GetProfileUserDtos Tutor { get; set; } = default!;

    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
}