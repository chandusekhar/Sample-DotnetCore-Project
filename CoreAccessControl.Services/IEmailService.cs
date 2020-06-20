using CoreAccessControl.DataAccess.Ef.Models;
using System.Threading.Tasks;

namespace CoreAccessControl.Services
{
    public interface IEmailService
    {
        Task<EmailTemplateModel> ConstructAuthCodeSending(string code);
        Task<EmailTemplateModel> ConstructEmailVerification(string verificationToken);
        Task<EmailTemplateModel> ConstructResetPassword(string tempPwd);
    }
}
