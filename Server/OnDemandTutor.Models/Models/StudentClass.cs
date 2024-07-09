using System.ComponentModel.DataAnnotations;

namespace OnDemandTutor.Models.Models;

public class StudentClass : BaseEntity
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public virtual User Student { get; set; }

    public int ClassId { get; set; }
    public virtual Class Class { get; set; }
    public int? TutorId { get; set; }
    public virtual User Tutor { get; set; }
    [Range(1, 5)]
    public int? Rating { get; set; }
}