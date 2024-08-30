namespace OnDemandTutor.Models.Models;

public class TutorVideo : BaseEntity
{
    public int? TutorId { get; set; }
    public string VideoUrl { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public virtual User Tutor { get; set; } = default!;
}