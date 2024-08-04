using System.Net;
using System.Net.Mail;
using System.Text;
using LinqKit;
using Microsoft.Extensions.Options;
using OnDemandTutor.BusinessLogic.Interfaces;
using OnDemandTutor.BusinessLogic.Interfaces.Mail;
using OnDemandTutor.DataAccess.IRepository;
using OnDemandTutor.Models;
using OnDemandTutor.Models.Models;

namespace OnDemandTutor.BusinessLogic.Services.Mail;

public class EmailServices : IEmailServices
{
    private readonly IDefaultScheduleJob _defaultScheduleJob;
    private readonly IEmailTemplateRepository _emailTemplateRepository;
    private readonly SmtpAppSetting _smtpAppSetting;

    public EmailServices(
        IDefaultScheduleJob defaultScheduleJob,
        IEmailTemplateRepository emailTemplateRepository,
        IOptions<SmtpAppSetting> appSetting
    )
    {
        _defaultScheduleJob = defaultScheduleJob;
        _emailTemplateRepository = emailTemplateRepository;
        _smtpAppSetting = appSetting.Value;
    }


    public async Task SendAsync(string name, List<string> toAddress, List<string> ccAddresses,
        Dictionary<string, string> param, bool isInQueue = false)
    {
        if (!isInQueue)
        {
            _defaultScheduleJob.Enqueue<IEmailServices>(m => m.SendAsync(name, toAddress, ccAddresses, param, true));
            return;
        }

        var template = await _emailTemplateRepository.FirstOrDefaultAsync(e => e.Name == name && e.Status == true);
        await SendAsync(template, toAddress, ccAddresses, param);
    }

    public async Task SendEmailAsync(List<string> toAddresses, List<string> ccAddresses, string subject, string body,
        bool isHtml, bool isInQueue = false)
    {
        if (!isInQueue)
        {
            _defaultScheduleJob.Enqueue<IEmailServices>(m =>
                m.SendEmailAsync(toAddresses, ccAddresses, subject, body, isHtml, true));
            return;
        }

        using (var client = new SmtpClient(_smtpAppSetting.SmtpHost, _smtpAppSetting.SmtpPort))
        {
            client.EnableSsl = _smtpAppSetting.EnableSsl;
            client.Credentials = new NetworkCredential(_smtpAppSetting.SmtpUserName, _smtpAppSetting.AppVerify);
            client.Port = _smtpAppSetting.SmtpPort;

            using (var message = new MailMessage())
            {
                try
                {
                    message.From = new MailAddress(_smtpAppSetting.SmtpUserName);

                    toAddresses?.ForEach(to => message.To.Add(to));
                    ccAddresses?.ForEach(cc => message.CC.Add(cc));

                    message.Subject = subject;
                    message.Body = body;
                    message.IsBodyHtml = isHtml;

                    message.BodyEncoding = Encoding.UTF8;
                    message.SubjectEncoding = Encoding.UTF8;

                    await client.SendMailAsync(message);
                }
                catch (Exception ex)
                {
                    throw new ApplicationException($"Failed to send Email: {ex.Message}", ex);
                }
            }
        }
    }


    private async Task SendAsync(EmailTemplate template, List<string> toAddress, List<string> ccAddresses,
        Dictionary<string, string> param)
    {
        var smtpAppSetting = new SmtpAppSetting
        {
            SmtpHost = _smtpAppSetting.SmtpHost,
            SmtpPort = _smtpAppSetting.SmtpPort,
            SmtpUserName = _smtpAppSetting.SmtpUserName,
            SmtpPassword = _smtpAppSetting.SmtpPassword,
            AppVerify = _smtpAppSetting.AppVerify
        };

        using (var client = new SmtpClient(smtpAppSetting.SmtpHost, smtpAppSetting.SmtpPort))
        {
            client.EnableSsl = _smtpAppSetting.EnableSsl;
            client.Credentials = new NetworkCredential(smtpAppSetting.SmtpUserName, smtpAppSetting.AppVerify);
            client.Port = smtpAppSetting.SmtpPort;

            using (var message = new MailMessage())
            {
                try
                {
                    message.From = new MailAddress(smtpAppSetting.SmtpUserName);

                    toAddress?.ForEach(to => message.To.Add(to));
                    ccAddresses?.ForEach(cc => message.CC.Add(cc));

                    message.Subject = ReplaceParam(template.Subject, param);
                    message.Body = ReplaceParam(template.Body, param);
                    message.IsBodyHtml = true;

                    message.BodyEncoding = Encoding.UTF8;
                    message.SubjectEncoding = Encoding.UTF8;

                    await client.SendMailAsync(message);
                }
                catch (Exception ex)
                {
                    throw new ApplicationException($"Failed to send Email: {ex.Message}", ex);
                }
            }
        }
    }

    private static string ReplaceParam(string data, Dictionary<string, string> parameters)
    {
        parameters.ForEach(k => data = data.Replace($"[{k.Key}]", k.Value));
        return data;
    }
}