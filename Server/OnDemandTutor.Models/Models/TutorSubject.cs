using OnDemandTutor.Models.Enum;

namespace OnDemandTutor.Models.Models;

public class TutorSubject : BaseEntity
{
    public int UserId { get; set; }
    public virtual User User { get; set; }

    public int SubjectId { get; set; }
    public virtual Subject Subject { get; set; }
    
    public SubjectStatus Status { get; set; }

}