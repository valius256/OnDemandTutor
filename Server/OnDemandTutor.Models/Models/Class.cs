using OnDemandTutor.Models.Enum;

namespace OnDemandTutor.Models.Models;

public class Class : BaseEntity
{
    public string? Name { get; set; }
    public int TutorId { get; set; }
    public int SubjectId { get; set; }
    public string? Location { get; set; }
    public string? Method { get; set; }
    public int NumberOfStudents { get; set; }
    public ClassStatus Status { get; set; }
    public virtual Subject Subject { get; set; } = default!;
    public virtual User Tutor { get; set; } = default!;
    public virtual ICollection<StudentClass> StudentClasses { get; set; } = new List<StudentClass>();
    public virtual ICollection<Slot> Slots { get; set; } = new List<Slot>();
}