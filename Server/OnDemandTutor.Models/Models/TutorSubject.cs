using OnDemandTutor.Models.Enum;

namespace OnDemandTutor.Models.Models;

public class TutorSubject : BaseEntity
{
    public int UserId { get; set; }
    public virtual User User { get; set; }
    public int SubjectId { get; set; }
    public string Description { get; set; } = string.Empty;
    public virtual Subject Subject { get; set; }
    public TutorSubjectStatus Status { get; set; }

}