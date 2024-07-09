using OnDemandTutor.Models.Enum;

namespace OnDemandTutor.Models.Models;

public class Class : BaseEntity
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int TutorId { get; set; }
    public int SubjectId { get; set; }
    public string? Location { get; set; }
    public string? Method { get; set; }
    public ClassStatus Status { get; set; }
    
    public virtual Subject Subject { get; set; }
    public virtual User User { get; set; }
    public virtual ICollection<StudentClass> StudentClasses { get; set; } = new List<StudentClass>();
    public virtual ICollection<Slot> Slots { get; set; } = new List<Slot>();
}