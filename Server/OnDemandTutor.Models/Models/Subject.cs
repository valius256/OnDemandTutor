using OnDemandTutor.Models.Enum;

namespace OnDemandTutor.Models.Models;

public class Subject : BaseEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string SubjectType { get; set; }
    public int? CreateById { get; set; }
    public string Description { get; set; }
    public DateTime? CreateAt { get; set; }
    public bool IsEnable { get; set; }


    public virtual ICollection<TutorDegree> TutorDegree { get; set; } = new List<TutorDegree>();
    public virtual ICollection<Class> Class { get; set; } = new List<Class>();
    public virtual User CreateBy { get; set; }
    public virtual ICollection<Slot> Slots { get; set; } = new List<Slot>();
    public virtual ICollection<TutorSubject> TutorSubjects { get; set; } = new List<TutorSubject>();
}