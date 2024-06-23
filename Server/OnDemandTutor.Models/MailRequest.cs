namespace OnDemandTutor.Models;

public class MailRequest
{
    public string? ToEmail { get; set; }
    public string? Subject { get; set; }

    public string? Body { get; set; }
    // public List<IFormFile>? Attachments { get; set; }  // implement when the hangfire storage is configured
}