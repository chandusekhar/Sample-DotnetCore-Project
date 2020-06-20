using CoreAccessControl.DataAccess.Ef.Models;
using System.Threading.Tasks;

namespace CoreAccessControl.Services
{
    public interface IEmailSender
    {
        Task SendMailViaSmtpClientAsync(string[] to, string[] cc, string[] bcc, EmailTemplateModel emailData);
    }
}
