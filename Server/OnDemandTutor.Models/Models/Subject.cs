namespace OnDemandTutor.Models.Models;

public class Subject : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string SubjectType { get; set; } = string.Empty;
    public int? CreateById { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime? CreateAt { get; set; }
    public bool IsEnable { get; set; }


    public virtual ICollection<TutorDegree> TutorDegree { get; set; } = new List<TutorDegree>();
    public virtual ICollection<Class> Class { get; set; } = new List<Class>();
    public virtual User CreateBy { get; set; } = default!;
    public virtual ICollection<Slot> Slots { get; set; } = new List<Slot>();
    public virtual ICollection<TutorSubject> TutorSubjects { get; set; } = new List<TutorSubject>();
}