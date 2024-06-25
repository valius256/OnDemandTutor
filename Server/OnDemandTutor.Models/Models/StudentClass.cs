namespace OnDemandTutor.Models.Models;

public class StudentClass : BaseEntity
{
    public int StudentId { get; set; }
    public virtual User Student { get; set; }

    public int ClassId { get; set; }
    public virtual Class Class { get; set; }
}