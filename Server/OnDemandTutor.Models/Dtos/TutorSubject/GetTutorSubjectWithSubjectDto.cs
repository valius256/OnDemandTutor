using OnDemandTutor.Models.Dtos.Subject;
using OnDemandTutor.Models.Enum;

namespace OnDemandTutor.Models.Dtos.TutorSubject;

public class GetTutorSubjectWithSubjectDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int SubjectId { get; set; }
    public GetSubjectDtos Subject { get; set; } = default!;
    public string Description { get; set; } = string.Empty;

    public DateTime? CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public TutorSubjectStatus Status { get; set; }
}