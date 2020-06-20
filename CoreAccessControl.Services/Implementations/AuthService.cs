using CoreAccessControl.DataAccess.Ef.Data;
using CoreAccessControl.DataAccess.Ef.Models;
using CoreAccessControl.Domain.Configuration;
using CoreAccessControl.Domain.Models;
using CoreAccessControl.Domain.RequestModels;
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
using System.Threading.Tasks;

namespace CoreAccessControl.Services
{
    public class AuthService : IAuthService
    {
        private readonly CoreaccesscontrolContext _context;
        private readonly AppSettings _appSettings;
        private readonly IEmailSender _emailSender;
        private readonly IEmailService _emailService;


        public AuthService(CoreaccesscontrolContext context, AppSettings appSettings, IEmailSender emailSender, IEmailService emailService)
        {
            _context = context;
            _appSettings = appSettings;
            _emailSender = emailSender;
            _emailService = emailService;
        }

        public async Task<bool> IsEmailExists(string email)
        {
            return await _context.User.AnyAsync(x => x.Email == email);
        }

        public async Task<ServiceResponseResult> Register(RegisterModel model)
        {
            Logger.WriteInformation("Registering.");
            if (await _context.User.AnyAsync(x => x.Email == model.Email.ToLower()) || await _context.ChangeEmailRequest.AnyAsync(x => x.Email == model.Email.ToLower()))
            {
                return new ServiceResponseResult
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Result = new { Message = "Email already exists in the system." }
                };
            }

            var user = UserMapper.ToUser(model, _appSettings.Secret);
            _context.User.Add(user);
            await _context.SaveChangesAsync();

            var emailData = await _emailService.ConstructEmailVerification(user.VerificationToken);
            await _emailSender.SendMailViaSmtpClientAsync(new string[] { user.Email }, new string[] { }, new string[] { }, emailData);

            return new ServiceResponseResult
            {
                StatusCode = System.Net.HttpStatusCode.OK
            };
        }

        public async Task<ServiceResponseResult> VerifyEmail(string token, bool update)
        {
            Logger.WriteInformation("Verifying email.");
            var now = DateTime.UtcNow;
            if (update)
            {
                var changeEmailRequest = await _context.ChangeEmailRequest.FirstOrDefaultAsync(x => x.VerificationToken == token && now <= x.VerificationTokenExpiry);
                if (changeEmailRequest != null)
                {
                    changeEmailRequest = changeEmailRequest.UpdateVerificationToken(null)
                        .UpdateVerificationTokenExpiry(null);
                    _context.ChangeEmailRequest.Update(changeEmailRequest);                    

                    var user = await _context.User.FirstOrDefaultAsync(x => x.Id == changeEmailRequest.UserId);
                    user = user.UpdateIsEmailVerified(true)
                        .UpdateVerificationToken(null)
                        .UpdateVerificationTokenExpiry(null)
                        .UpdateEmail(changeEmailRequest.Email)
                        .UpdateLastUpdatedOn(DateTime.UtcNow)
                        .UpdateLastUpdatedBy(user.Id);

                    _context.User.Update(user);
                    await _context.SaveChangesAsync();

                    return new ServiceResponseResult
                    {
                        StatusCode = System.Net.HttpStatusCode.OK
                    };
                }
                else
                {
                    return new ServiceResponseResult
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Result = new { Message = "Not a valid token." }
                    };
                }
            }
            else
            {
                var user = await _context.User.FirstOrDefaultAsync(x => x.VerificationToken == token && now <= x.VerificationTokenExpiry);
                if (user != null)
                {
                    user = user.UpdateIsEmailVerified(true)
                        .UpdateVerificationToken(null)
                        .UpdateVerificationTokenExpiry(null)
                        .UpdateLastUpdatedOn(DateTime.UtcNow)
                        .UpdateLastUpdatedBy(user.Id);

                    _context.User.Update(user);
                    await _context.SaveChangesAsync();

                    return new ServiceResponseResult
                    {
                        StatusCode = System.Net.HttpStatusCode.OK
                    };
                }
                else
                {
                    return new ServiceResponseResult
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Result = new { Message = "Not a valid token." }
                    };
                }
            }
        }

        public async Task<ServiceResponseResult> Login(LoginModel model)
        {
            Logger.WriteInformation("Performing login.");
            var pwdHash = HashUtility.CreatePasswordHash(model.Password, _appSettings.Secret);
            var user = await _context.User.FirstOrDefaultAsync(x => x.Email == model.Email.ToLower() && x.PasswordHash == pwdHash);

            if (user == null)
            {
                return new ServiceResponseResult
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Result = new { Message = "Email and/or password does not match." }
                };
            }

            if (user.IsEmailVerified.HasValue && !user.IsEmailVerified.Value)
            {
                return new ServiceResponseResult
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Result = new { Message = "Email is not verified." }
                };
            }

            var locations = await _context.UserLocation.Where(x => x.UserId == user.Id).ToListAsync();
            var isLoggedIn = !user.IsTemporaryPassword.HasValue || user.IsTemporaryPassword.Value;
            var locationIds = locations.Select(x => x.LocationId).ToArray();

            var jwtPayload = new JWTPayload(isLoggedIn, locationIds, user.Email, user.Id, user.Name);

            var userLocations = locations.Select(x => x.Id).ToArray();
            var permissions = await _context.UserPermission.Include(x => x.UserLocation).Where(x => userLocations.Contains(x.UserLocationId)).ToListAsync();

            jwtPayload.Permissions = permissions.Select(x => new Permission
            {
                HasAdminEdit = x.HasAdminEdit.HasValue && x.HasAdminEdit.Value,
                HasAdminRead = x.HasAdminRead.HasValue && x.HasAdminRead.Value,
                HasConfigEdit = x.HasConfigEdit.HasValue && x.HasConfigEdit.Value,
                HasConfigRead = x.HasConfigRead.HasValue && x.HasConfigRead.Value,
                HasDeviceEdit = x.HasDeviceEdit.HasValue && x.HasDeviceEdit.Value,
                HasDeviceRead = x.HasDeviceRead.HasValue && x.HasDeviceRead.Value,
                HasKeyholderEdit = x.HasKeyholderEdit.HasValue && x.HasKeyholderEdit.Value,
                HasKeyholderRead = x.HasKeyholderRead.HasValue && x.HasKeyholderRead.Value,
                HasSpaceEdit = x.HasSpaceEdit.HasValue && x.HasSpaceEdit.Value,
                HasSpaceRead = x.HasSpaceRead.HasValue && x.HasSpaceRead.Value,
                LocationId = x.UserLocation.LocationId
            }).ToArray();

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, user.Email.ToString()),
                    new Claim(Constants.JWTPayloadClaim,JsonConvert.SerializeObject(jwtPayload))
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new ServiceResponseResult
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Result = new { Token = tokenHandler.WriteToken(token) }
            };
        }

        public async Task<ServiceResponseResult> UpdateSecurityQuestion(UpdateSecurityQuestionReqModel model, long? userId)
        {
            Logger.WriteInformation("Updating securrity questions.");
            var user = await _context.User.FirstOrDefaultAsync(x => x.Id == userId);

            user = user.UpdateSecurityQuestion(model.Question)
                .UpdateSecurityQuestionAnswer(model.Answer)
                .UpdateLastUpdatedOn(DateTime.UtcNow)
                .UpdateLastUpdatedBy(user.Id);

            _context.User.Update(user);
            await _context.SaveChangesAsync();

            return new ServiceResponseResult
            {
                StatusCode = System.Net.HttpStatusCode.OK
            };
        }

        public async Task<ServiceResponseResult> GetSecurityQuestion(string email)
        {
            Logger.WriteInformation("Getting securrity questions.");
            var user = await _context.User.FirstOrDefaultAsync(x => x.Email == email);

            if (user == null)
            {
                return new ServiceResponseResult
                {
                    Result = new { Message = "No user exists with email" },
                    StatusCode = System.Net.HttpStatusCode.NotFound
                };
            }

            if (string.IsNullOrEmpty(user.SecurityQuestion))
            {
                return new ServiceResponseResult
                {
                    Result = new { Message = "No security question set for user" },
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };
            }

            return new ServiceResponseResult
            {
                Result = user.SecurityQuestion,
                StatusCode = System.Net.HttpStatusCode.OK
            };
        }

        public async Task<ServiceResponseResult> ForgotPassword(PasswordRecoverReqModel model)
        {
            Logger.WriteInformation("Requesting forgot password.");
            var user = await _context.User.FirstOrDefaultAsync(x => x.Email == model.Email.ToLower());

            if (user == null)
            {
                return new ServiceResponseResult
                {
                    Result = new { Message = "No user exists with email" },
                    StatusCode = System.Net.HttpStatusCode.NotFound
                };
            }

            if (!(user.SecurityQuestion == model.SecurityQuestion && user.SecurityQuestionAnswer == model.SecurityQuestionReply))
            {
                return new ServiceResponseResult
                {
                    Result = new { Message = "Security question and answer does not match" },
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };
            }

            if (!user.IsEmailVerified.HasValue || !user.IsEmailVerified.Value)
            {
                return new ServiceResponseResult
                {
                    Result = new { Message = "Email not verified" },
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };
            }

            var tempPwd = Guid.NewGuid().ToString();
            var pwdHash = HashUtility.CreatePasswordHash(tempPwd, _appSettings.Secret);

            user = user.UpdateIsTemporaryPassword(true)
                .UpdatePasswordHash(pwdHash)
                .UpdateLastUpdatedOn(DateTime.UtcNow)
                .UpdateLastUpdatedBy(user.Id);

            _context.User.Update(user);
            await _context.SaveChangesAsync();

            var emailData = await _emailService.ConstructResetPassword(tempPwd);
            await _emailSender.SendMailViaSmtpClientAsync(new string[] { user.Email }, new string[] { }, new string[] { }, emailData);

            return new ServiceResponseResult
            {
                StatusCode = System.Net.HttpStatusCode.OK
            };
        }

        public async Task<ServiceResponseResult> ChangePassword(ChangePasswordReqModel model, long? userid)
        {
            Logger.WriteInformation("Requesting changing password.");
            var user = await _context.User.FirstOrDefaultAsync(x => x.Id == userid);
            var pwdHash = HashUtility.CreatePasswordHash(model.Password, _appSettings.Secret);

            user = user.UpdateIsTemporaryPassword(false)
                        .UpdatePasswordHash(pwdHash)
                        .UpdateLastUpdatedOn(DateTime.UtcNow)
                        .UpdateLastUpdatedBy(user.Id);

            _context.User.Update(user);
            await _context.SaveChangesAsync();

            return new ServiceResponseResult
            {
                StatusCode = System.Net.HttpStatusCode.OK
            };
        }
    }
}
