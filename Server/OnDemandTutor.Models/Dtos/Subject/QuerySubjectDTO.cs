namespace OnDemandTutor.Models.Dtos.Subject;

public class QuerySubjectDTO
{
    public string? Name { get; set; }
    public string? Type { get; set; }
    public string? Description { get; set; }
    public string? Status { get; set; }
    public DateTime? CreateFrom { get; set; }
    public DateTime? CreateTo { get; set; }
    public DateTime? UpdateFrom { get; set; }
    public DateTime? UpdateTo { get; set; }
}