namespace OnDemandTutor.Models.Models;

public class TutorVideo : BaseEntity
{
    public int Id { get; set; }
    public int? TutorId { get; set; }
    public string VideoUrl { get; set; }
    public string Description { get; set; }
    public virtual User Tutor { get; set; }
}