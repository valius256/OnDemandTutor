using OnDemandTutor.Models.Enum;

namespace OnDemandTutor.Models.Models;

public class TutorDegree : BaseEntity
{
    public int? TutorId { get; set; }
    public string? DegreeImgUrl { get; set; }
    public string? TutorDegreeName { get; set; }
    public string Description { get; set; } = string.Empty;
    public int SubjectId { get; set; }
    public string DegreeNumber { get; set; } = string.Empty;
    public DateOnly IssuranceDate { get; set; }
    public TutorSubjectDegreeStatus TutorSubjectStatus { get; set; }
    public string? RejectReason { get; set; }

    public virtual Subject Subject { get; set; } = default!;
    public virtual User Tutor { get; set; } = default!;
}