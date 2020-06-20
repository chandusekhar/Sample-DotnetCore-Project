using CoreAccessControl.DataAccess.Ef.Models;
using CoreAccessControl.Domain.Models;
using CoreAccessControl.Domain.ResponseModels;
using CoreAccessControl.Services;
using CoreAccessControl.Services.ApiModel.Response;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CoreAccessControl.Api.Service.Fakes
{
    public class FakeEmailService : IEmailService
    {
        public async Task<EmailTemplateModel> ConstructAuthCodeSending(string code)
        {
            return await Task.FromResult(new Mock<EmailTemplateModel>().Object);
        }

        public async Task<EmailTemplateModel> ConstructEmailVerification(string verificationToken)
        {
            return await Task.FromResult(new Mock<EmailTemplateModel>().Object);
        }

        public async Task<EmailTemplateModel> ConstructResetPassword(string tempPwd)
        {
            return await Task.FromResult(new Mock<EmailTemplateModel>().Object);
        }
    }



}
