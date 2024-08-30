namespace OnDemandTutor.Models.Dtos.EmailTemplate;

public class EmailRequestDto
{
    public List<string> ToAddresses { get; set; } = new List<string>();
    public List<string> CcAddresses { get; set; } = new List<string>();
    public string Subject { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public bool IsHtml { get; set; } = true; // Optional: to specify if the body is HTML
}