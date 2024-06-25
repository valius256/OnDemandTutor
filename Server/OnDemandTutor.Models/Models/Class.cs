namespace OnDemandTutor.Models.Models;

public class Class : BaseEntity
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int TutorId { get; set; }
    public int SubjectId { get; set; }
    public string? StudentName { get; set; }
    public int SlotId { get; set; }

    public virtual ICollection<User> Students { get; set; } = new List<User>();
    public virtual Subject Subject { get; set; }
    public virtual User User { get; set; }
    public virtual ICollection<StudentClass> StudentClasses { get; set; } = new List<StudentClass>();
    public virtual ICollection<Slot> Slots { get; set; } = new List<Slot>();
}