using OnDemandTutor.Models.Enum;

namespace OnDemandTutor.Models.Models;

public class TutorDegree : BaseEntity
{
    public int Id { get; set; }
    public int? TutorId { get; set; }
    public string? DegreeImgUrl { get; set; }
    public string Description { get; set; }
    public int SubjectId { get; set; }
    public string DegreeNumber { get; set; }
    public DateOnly IssuranceDate { get; set; }
    public TutorSubjectDegreeStatus TutorSubjectStatus { get; set; }
    public string? RejectReason { get; set; }

    public virtual Subject Subject { get; set; }
    public virtual User Tutor { get; set; }
}