namespace OnDemandTutor.BusinessLogic.Interfaces.Mail;

public interface IEmailServices
{
    Task SendAsync(string templateName, List<string> toAddress, List<string> ccAddresses, Dictionary<string, string> param, bool isInQueue = false);
    Task SendEmailAsync(List<string> toAddresses, List<string> ccAddresses, string subject, string body, bool isHtml,
        bool isEnque);
}