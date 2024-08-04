namespace OnDemandTutor.Models.Dtos.EmailTemplate;

public class EmailTemplateDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool Status { get; set; }
    public string Body { get; set; }
    public string? Params { get; set; }
    public string Subject { get; set; }
    public string? Description { get; set; }
}