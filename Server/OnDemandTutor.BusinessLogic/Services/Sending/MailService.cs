using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using OnDemandTutor.Models;
using IMailService = OnDemandTutor.BusinessLogic.Interfaces.Sending.IMailService;

namespace OnDemandTutor.BusinessLogic.Services.Sending
{
    public class MailService : IMailService
    {
        private readonly SmtpSettings _mailSettings;
        public MailService(IOptions<SmtpSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }
        public async Task SendEmailAsync(MailRequest mailRequest)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.User);
            email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
            email.Subject = mailRequest.Subject;
            var builder = new BodyBuilder();
            // if (mailRequest.Attachments != null)
            // {
            //     byte[] fileBytes;
            //     foreach (var file in mailRequest.Attachments)
            //     {
            //         if (file.Length > 0)
            //         {
            //             using (var ms = new MemoryStream())
            //             {
            //                 file.CopyTo(ms);
            //                 fileBytes = ms.ToArray();
            //             }
            //             builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
            //         }
            //     }
            // }
            builder.HtmlBody = mailRequest.Body;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.User, _mailSettings.Pass);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
}
