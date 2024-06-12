using OnDemandTutor.Models;

namespace OnDemandTutor.BusinessLogic.Interfaces.Sending
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest);
    }
}
