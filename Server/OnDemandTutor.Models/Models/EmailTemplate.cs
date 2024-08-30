namespace OnDemandTutor.Models.Models;

public class EmailTemplate : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public bool Status { get; set; }
    public string Body { get; set; } = string.Empty;
    public string? Params { get; set; }
    public string Subject { get; set; } = string.Empty ;
    public string? Description { get; set; }
}