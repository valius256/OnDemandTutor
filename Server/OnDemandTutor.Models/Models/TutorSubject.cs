using OnDemandTutor.Models.Enum;

namespace OnDemandTutor.Models.Models;

public class TutorSubject : BaseEntity
{
    public int Id;
    public int UserId { get; set; }
    public virtual User User { get; set; }
    public int SubjectId { get; set; }
    public string Description { get; set; }
    public virtual Subject Subject { get; set; }
    public TutorSubjectStatus Status { get; set; }

}