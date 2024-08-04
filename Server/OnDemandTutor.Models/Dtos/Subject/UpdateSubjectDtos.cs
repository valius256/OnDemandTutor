namespace OnDemandTutor.Models.Dtos.Subject;

public class UpdateSubjectDtos
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string SubjectType { get; set; }
    public string Description { get; set; }
    public bool IsEnable { get; set; }
}