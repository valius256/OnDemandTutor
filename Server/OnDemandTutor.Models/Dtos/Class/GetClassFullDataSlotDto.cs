using OnDemandTutor.Models.Dtos.Slot;
using OnDemandTutor.Models.Dtos.StudentClass;
using OnDemandTutor.Models.Dtos.Subject;
using OnDemandTutor.Models.Dtos.User;
using OnDemandTutor.Models.Enum;

namespace OnDemandTutor.Models.Dtos.Class;

public class GetClassFullDataSlotDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int TutorId { get; set; }
    public int SubjectId { get; set; }
    public string? Location { get; set; }
    public string? Method { get; set; }
    public int NumberOfStudents { get; set; }
    public ClassStatus Status { get; set; }

    public GetSubjectDtos Subject { get; set; } = default!;
    public GetProfileUserDtos Tutor { get; set; } = default!;

    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }

    public List<GetSimpleSlotDto> Slots = new List<GetSimpleSlotDto>();
    public List<GetStudentClassWithStudentDto> StudentClasses = new List<GetStudentClassWithStudentDto>();
}