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

namespace CoreAccessControl.Services.Test.Fakes
{
    public class FakeEmailSender : IEmailSender
    {
        public async Task SendMailViaSmtpClientAsync(string[] to, string[] cc, string[] bcc, EmailTemplateModel emailData)
        {
            await Task.Delay(1);
        }
    }


}
