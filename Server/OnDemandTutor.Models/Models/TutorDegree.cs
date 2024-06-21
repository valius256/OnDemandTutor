namespace OnDemandTutor.Models.Models;

public class TutorDegree : IBaseEntity
{
    public int Id { get; set; }
    public int? TutorId { get; set; }
    public int? DegreeImgID { get; set; }
    public string? Description { get; set; }
    public int SubjectId { get; set; }
    public virtual Subject Subject { get; set; }
    public virtual User Tutor { get; set; }
}