using CoreAccessControl.DataAccess.Ef.Data;
using CoreAccessControl.DataAccess.Ef.Models;
using CoreAccessControl.Domain.Models;
using CoreAccessControl.Domain.RequestModels;
using CoreAccessControl.Domain.ResponseModels;
using CoreAccessControl.Services.Common;
using CoreAccessControl.Services.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Supra.LittleLogger;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CoreAccessControl.Services
{
    public class AuthenticationCodeService : IAuthenticationCodeService
    {
        private readonly CoreaccesscontrolContext _context;
        private readonly IEmailSender _emailSender;
        private readonly IEmailService _emailService;
        private readonly IApiService _apiService;

        public AuthenticationCodeService(CoreaccesscontrolContext context, IEmailSender emailSender, IEmailService emailService, IApiService apiService)
        {
            _context = context;
            _emailSender = emailSender;
            _emailService = emailService;
            _apiService = apiService;
        }

        public async Task<ServiceResponseResult> GetCode(long locationId, long userId, string keySerialNumber, string deviceSerialNumber)
        {
            Logger.WriteInformation("Getting auth code data.");
            if (!string.IsNullOrEmpty(keySerialNumber) && !string.IsNullOrEmpty(deviceSerialNumber))
            {
                return new ServiceResponseResult
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Result = "Only one type of serial can be fetched at a time."
                };
            }

            if(!string.IsNullOrEmpty(keySerialNumber))
            {
                return await _apiService.GetKeyAuthCode(keySerialNumber);
            }

            if (!string.IsNullOrEmpty(deviceSerialNumber))
            {
                return await _apiService.GetDeviceAuthCode(deviceSerialNumber);
            }

            return new ServiceResponseResult
            {
                StatusCode = System.Net.HttpStatusCode.BadRequest,
                Result = "Provide at least keySerialNumber or deviceSerialNumber in query."
            };
        }

        public async Task<ServiceResponseResult> SendCode(AuthCodeCreateReqModel model, long userId, long locationId)
        {
            Logger.WriteInformation("Sending auth code data.");
            var user = await _context.User.FirstOrDefaultAsync(x => x.Id == userId);

            var emailData = await _emailService.ConstructAuthCodeSending(model.Code);
            await _emailSender.SendMailViaSmtpClientAsync(new string[] { user.Email }, new string[] { }, new string[] { }, emailData);

            return new ServiceResponseResult
            {
                StatusCode = System.Net.HttpStatusCode.OK
            };
        }
    }
}
