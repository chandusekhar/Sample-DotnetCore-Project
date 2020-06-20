using CoreAccessControl.DataAccess.Ef.Models;
using CoreAccessControl.Domain.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace CoreAccessControl.Services.Common
{
    public class EmailSender : IEmailSender
    {
        private readonly AppSettings _appSettings;

        public EmailSender(AppSettings appSettings)
        {
            this._appSettings = appSettings;
        }

        public async Task SendMailViaSmtpClientAsync(string[] to, string[] cc, string[] bcc, EmailTemplateModel emailData)
        {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient(_appSettings.MailSettings.Host, _appSettings.MailSettings.Port);

            mail.From = new MailAddress(_appSettings.MailSettings.FromAddress);

            if (to != null)
            {
                to.ToList().ForEach(x => mail.To.Add(x));
            }

            if (cc != null)
            {
                cc.ToList().ForEach(x => mail.CC.Add(x));
            }

            if (bcc != null)
            {
                cc.ToList().ForEach(x => mail.Bcc.Add(x));
            }

            mail.Subject = emailData.Subject;
            mail.Body = emailData.Body;

            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential(_appSettings.MailSettings.Username, _appSettings.MailSettings.Password);
            SmtpServer.EnableSsl = _appSettings.MailSettings.EnableSsl;

            await SmtpServer.SendMailAsync(mail);
        }
    }
}
