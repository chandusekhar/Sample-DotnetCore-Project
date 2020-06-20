using CoreAccessControl.DataAccess.Ef.Models;
using CoreAccessControl.Domain.Configuration;
using CoreAccessControl.Domain.Models;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace CoreAccessControl.Services.Common
{
    public class EmailService : IEmailService
    {
        private readonly AppSettings _appSettings;

        public EmailService(AppSettings appSettings)
        {
            this._appSettings = appSettings;
        }

        private async Task<EmailTemplateModel> GetTemplate(string templateName)
        {
            var fileData = await File.ReadAllTextAsync(Path.Combine(_appSettings.ContentRootPath, "EmailTemplates", templateName));
            return JsonConvert.DeserializeObject<EmailTemplateModel>(fileData);
        }

        public async Task<EmailTemplateModel> ConstructEmailVerification(string verificationToken)
        {
            var emailData = await GetTemplate(EmailConstants.EmailVerificationTemplate);
             var url = _appSettings.Domain + $"api/ver1/auth/verifyEmail?token={verificationToken}";
            emailData.Body = emailData.Body.Replace("{{Link}}", url);

            return emailData;
        }

        public async Task<EmailTemplateModel> ConstructResetPassword(string tempPwd)
        {
            var emailData = await GetTemplate(EmailConstants.ResetPasswordTemplate);
            emailData.Body = emailData.Body.Replace("{{Password}}", tempPwd);

            return emailData;
        }

        public async Task<EmailTemplateModel> ConstructAuthCodeSending(string code)
        {
            var emailData = await GetTemplate(EmailConstants.AuthCodeSendingTemplate);
            emailData.Body = emailData.Body.Replace("{{Code}}", code);

            return emailData;
        }
    }
}
