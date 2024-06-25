namespace OnDemandTutor.BusinessLogic.Interfaces.Mail;

public interface IMailServices
{
    Task SendAsync(string templateName, List<string> toAddress, List<string> ccAddresses, Dictionary<string, string> param, bool isInQueue = false);
}