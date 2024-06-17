using OnDemandTutor.BusinessLogic.Services.Sending;
using OnDemandTutor.Models;

namespace OnDemandTutor.BusinessLogic.Services.BackGroundJob;

public class EmailJob
{
    private readonly MailService _emailService;

    public EmailJob(MailService emailService)
    {
        _emailService = emailService;
    }

    public async Task SendEmailAsync(string jobType, string startTime, MailRequest mailRequest)
    {
        Console.WriteLine(jobType + "-" + startTime + "- Email Successfully Sent" + DateTime.UtcNow.ToLongTimeString());
        await _emailService.SendEmailAsync(mailRequest);
    }
}