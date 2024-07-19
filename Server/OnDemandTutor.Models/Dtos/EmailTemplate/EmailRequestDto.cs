namespace OnDemandTutor.Models.Dtos.EmailTemplate;

public class EmailRequestDto
{
    public List<string> ToAddresses { get; set; }
    public List<string> CcAddresses { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    public bool IsHtml { get; set; } = true; // Optional: to specify if the body is HTML
}