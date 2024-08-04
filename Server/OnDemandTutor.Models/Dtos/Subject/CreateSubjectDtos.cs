namespace OnDemandTutor.Models.Dtos.Subject;

public class CreateSubjectDtos
{
    public string Name { get; set; }
    public string SubjectType { get; set; }
    public int? CreateById { get; set; }
    public string Description { get; set; }
    public DateTime? CreateAt { get; set; }
    public bool IsEnable { get; set; }
}