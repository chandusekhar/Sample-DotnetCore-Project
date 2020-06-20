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
    public class AccountService : IAccountService
    {
        private readonly CoreaccesscontrolContext _context;
        private readonly IEmailSender _emailSender;
        private readonly IEmailService _emailService;

        public AccountService(CoreaccesscontrolContext context, IEmailSender emailSender, IEmailService emailService)
        {
            _context = context;
            _emailSender = emailSender;
            _emailService = emailService;
        }

        public async Task<ServiceResponseResult> GetAdminProfile(long locationId, long userId)
        {
            Logger.WriteInformation("Geting admin profile data.");
            var user = await _context.User.FindAsync(userId);
            var userKeyMapping =  await _context.UserKeyMapping.Where(x => x.LocationId == locationId && x.UserId == userId).FirstOrDefaultAsync();

            var response = new AdminProfileRespModel
            {
                Id = userId,
                Email = user.Email,
                Name = user.Name,
                LocationId = locationId               
            };

            if (userKeyMapping != null)
            {
                var keyHolder = await _context.KeyHolder.Where(x => x.KeySerialNumber == userKeyMapping.KeySerialNumber && x.LocationId == locationId)
                                                           .FirstOrDefaultAsync();
                if (keyHolder != null)
                {
                    response.ToolkitInfo = new ToolkitInfoRespModel
                    {
                        Id = keyHolder.Id,
                        KeySerialNumber = keyHolder.KeySerialNumber,
                        Pin = keyHolder.Pin
                    };
                }
            }

            Logger.WriteInformation("Geting admin profile data completed.");
            return new ServiceResponseResult
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Result = response,
            };
        }

        public async Task<ServiceResponseResult> UpdateAdminProfile(AdminProfileUpdateReqModel model, long userId, long locationId)
        {
            Logger.WriteInformation("Updating admin profile data.");
            var user = await _context.User.FirstOrDefaultAsync(x => x.Id == userId);

            var token = Guid.NewGuid().ToString();
            var now = DateTime.UtcNow;

            if (!string.IsNullOrEmpty(model.Email))
            {
                if(await _context.User.AnyAsync(x => x.Email == model.Email.ToLower()) || await _context.ChangeEmailRequest.AnyAsync(x => x.Email == model.Email.ToLower()))
                {
                    return new ServiceResponseResult
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Result = new { Message = "An user with the email already exists." },
                    };                    
                }


                var emailChange = new ChangeEmailRequest
                {
                    Email = model.Email,
                    RequestedOn = DateTime.UtcNow,
                    UserId = userId,
                    VerificationToken = token,
                    VerificationTokenExpiry = now.AddHours(24)
                };

                _context.ChangeEmailRequest.Add(emailChange);

                var emailData = await _emailService.ConstructEmailVerification(user.VerificationToken);
                await _emailSender.SendMailViaSmtpClientAsync(new string[] { model.Email }, new string[] { }, new string[] { }, emailData);
            }

            if (!string.IsNullOrEmpty(model.Name))
            {
                user = user.UpdateName(model.Name);
            }

            user = user.UpdateVerificationToken(token)
                   .UpdateVerificationTokenExpiry(now.AddHours(24))
                   .UpdateLastUpdatedOn(DateTime.UtcNow)
                   .UpdateLastUpdatedBy(userId);

            _context.User.Update(user);
            await _context.SaveChangesAsync();

            Logger.WriteInformation("Updating admin profile data completed.");
            return await GetAdminProfile(locationId, userId);
        }
    }
}
