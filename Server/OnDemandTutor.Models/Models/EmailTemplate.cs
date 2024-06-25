namespace OnDemandTutor.Models.Models;

public class EmailTemplate : BaseEntity
{
    public string Name { get; set; }
    public bool Status { get; set; }
    public string Body { get; set; }
    public string? Params { get; set; }
    public string Subject { get; set; }
    public string? Description { get; set; }
}