using CoreAccessControl.Domain.Models;
using CoreAccessControl.Domain.RequestModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CoreAccessControl.Services
{
    public interface IAuthService
    {
        Task<bool> IsEmailExists(string email);
        Task<ServiceResponseResult> Register(RegisterModel model);
        Task<ServiceResponseResult> VerifyEmail(string token, bool update);
        Task<ServiceResponseResult> Login(LoginModel model);
        Task<ServiceResponseResult> UpdateSecurityQuestion(UpdateSecurityQuestionReqModel model, long? userid);
        Task<ServiceResponseResult> GetSecurityQuestion(string email);
        Task<ServiceResponseResult> ForgotPassword(PasswordRecoverReqModel model);
        Task<ServiceResponseResult> ChangePassword(ChangePasswordReqModel model, long? userid);
    }
}
