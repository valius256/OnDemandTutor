using OnDemandTutor.Models.Enum;

namespace OnDemandTutor.Models.Dtos.TutorSubject;

public class QueryTutorSubjectDto
{
    public int? TutorId { get; set; }
    public string? TutorName { get; set; }

    public DateTime? CreateFrom { get; set; }

    public DateTime? CreateTo { get; set; }

    public TutorSubjectStatus? Status { get; set; }
    public List<int> SubjectIds { get; set; } = new();
}