using OnDemandTutor.Models.Dtos.TutorDegree;

namespace OnDemandTutor.Models.Dtos.TutorSubject;

public class CreateTutorSubjectDto
{
    public int UserId { get; set; }
    public int SubjectId { get; set; }
    public string? Description { get; set; }
    public List<CreateTutorDegreeSimpleDto> Degrees { get; set; } = new();
}