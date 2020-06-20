using CoreAccessControl.Api.Service.Fakes;
using CoreAccessControl.DataAccess.Ef.Data;
using CoreAccessControl.DataAccess.Ef.Models;
using CoreAccessControl.Domain.Models;
using CoreAccessControl.Domain.RequestModels;
using CoreAccessControl.Domain.ResponseModels;
using CoreAccessControl.Services.Common;
using CoreAccessControl.Services.Test.Fakes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Supra.LittleLogger;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CoreAccessControl.Services.Test
{
    public class AuthServiceTest : ServiceTestBase
    {
        public AuthService GetAuthService(CoreaccesscontrolContext context)
        {
            Logger.Init(GetAppSettings().ConnectionStrings.DefaultConnection, "CoreAccessControl.log", "CoreAccessControl", Severity.Information, mock: true);
            return new AuthService(context, GetAppSettings(), new FakeEmailSender(), new FakeEmailService());
        }


        [Fact]
        public async Task Register_SuccessRequest()
        {
            var context = GetInMemoryContext();
            
            context.Add(new Location
            {
                Id = 1,                
                Name = "test"
            });
            context.Add(new KeyHolder
            {
                Id = 1,
                Name = "test",
                KeySerialNumber = "1234",
                Pin = "1234",
                State = 1,
                LocationId = 1
            });

            context.SaveChanges();

            var service = GetAuthService(context);
            var response = await service.Register(new RegisterModel
            {
                Email = "test1@test.com",
                Password = "1234",
                SecurityQuestion = "1234",
                SecurityQuestionAnswer = "1234"
            });

            var user = await context.User.FirstOrDefaultAsync(x => x.Email == "test1@test.com");
            Assert.Equal(HashUtility.CreatePasswordHash("1234", GetAppSettings().Secret), user.PasswordHash);
            Assert.Equal("1234", user.SecurityQuestion);
            Assert.Equal("1234", user.SecurityQuestionAnswer);
            Assert.False(user.IsEmailVerified);
        }

        [Fact]
        public async Task VerifyEmailRegister_Sucess()
        {
            var context = GetInMemoryContext();
            context.Add(new User
            {
                Email = "test@test.com",
                Id = 1,
                IsEmailVerified = false,
                IsTemporaryPassword = false,
                Name = "test",
                VerificationToken = "1234",
                VerificationTokenExpiry = DateTime.UtcNow.AddDays(1)
            });
            context.Add(new Location
            {
                Id = 1,
                Name = "test"
            });
            context.Add(new UserLocation
            {
                Id = 1,
                UserId = 1,
                LocationId = 1,
                State = (int)AdministratorState.Active
            });
            context.Add(new KeyHolder
            {
                Id = 1,
                Name = "test",
                KeySerialNumber = "1234",
                Pin = "1234",
                State = 1,
                LocationId = 1
            });
            context.Add(new UserKeyMapping
            {
                Id = 1,
                UserId = 1,
                KeySerialNumber = "1234",
                LocationId = 1
            });
            context.SaveChanges();

            var service = GetAuthService(context);
            var response = await service.VerifyEmail("1234", false);

            var user = await context.User.FirstOrDefaultAsync(x => x.Email == "test@test.com");

            Assert.True(user.IsEmailVerified);
            Assert.Null(user.VerificationToken);
            Assert.Null(user.VerificationTokenExpiry);
        }

        [Fact]
        public async Task VerifyEmailChange_Sucess()
        {
            var context = GetInMemoryContext();
            context.Add(new User
            {
                Email = "test@test.com",
                Id = 1,
                IsEmailVerified = false,
                IsTemporaryPassword = false,
                Name = "test",
                VerificationToken = "1234",
                VerificationTokenExpiry = DateTime.UtcNow.AddDays(1)
            });
            context.Add(new ChangeEmailRequest
            {
                Email = "test1@test.com",
                Id = 1,
                VerificationToken = "1234",
                VerificationTokenExpiry = DateTime.UtcNow.AddDays(1),
                UserId = 1
            });
            context.Add(new Location
            {
                Id = 1,
                Name = "test"
            });
            context.Add(new UserLocation
            {
                Id = 1,
                UserId = 1,
                LocationId = 1,
                State = (int)AdministratorState.Active
            });
            context.Add(new KeyHolder
            {
                Id = 1,
                Name = "test",
                KeySerialNumber = "1234",
                Pin = "1234",
                State = 1,
                LocationId = 1
            });
            context.Add(new UserKeyMapping
            {
                Id = 1,
                UserId = 1,
                KeySerialNumber = "1234",
                LocationId = 1
            });
            context.SaveChanges();

            var service = GetAuthService(context);
            var response = await service.VerifyEmail("1234", true);

            var user = await context.User.FirstOrDefaultAsync(x => x.Email == "test1@test.com");

            Assert.Equal("test1@test.com", user.Email);
            Assert.True(user.IsEmailVerified);
            Assert.Null(user.VerificationToken);
            Assert.Null(user.VerificationTokenExpiry);

            var changeEmailRequest = await context.ChangeEmailRequest.FirstOrDefaultAsync(x => x.Email == "test1@test.com");

            Assert.Equal("test1@test.com", changeEmailRequest.Email);
            Assert.Null(changeEmailRequest.VerificationToken);
            Assert.Null(changeEmailRequest.VerificationTokenExpiry);
        }

        [Fact]
        public async Task UpdateSecurityQuestion_Success()
        {
            var context = GetInMemoryContext();
            context.Add(new User
            {
                Email = "test@test.com",
                Id = 1,
                IsEmailVerified = true,
                IsTemporaryPassword = false,
                Name = "test"
            });
            context.Add(new Location
            {
                Id = 1,
                Name = "test"
            });
            context.Add(new UserLocation
            {
                Id = 1,
                UserId = 1,
                LocationId = 1,
                State = (int)AdministratorState.Active
            });
            context.Add(new KeyHolder
            {
                Id = 1,
                Name = "test",
                KeySerialNumber = "1234",
                Pin = "1234",
                State = 1,
                LocationId = 1
            });
            context.Add(new UserKeyMapping
            {
                Id = 1,
                UserId = 1,
                KeySerialNumber = "1234",
                LocationId = 1
            });
            context.Add(new UserStatus
            {
                Id = 1,
                Name = "1",
                LocationId = 1,
                IsActive = true,
                IsDefault = true
            });
            context.SaveChanges();

            var service = GetAuthService(context);
            var response = await service.UpdateSecurityQuestion(new UpdateSecurityQuestionReqModel { 
                Answer = "123",
                Question = "123"
            },1);

            var user = await context.User.FirstOrDefaultAsync(x => x.Id == 1);
            Assert.Equal("123", user.SecurityQuestion);
            Assert.Equal("123", user.SecurityQuestionAnswer);

        }

        [Fact]
        public async Task GetSecurityQuestion_Success()
        {
            var context = GetInMemoryContext();
            context.Add(new User
            {
                Email = "test@test.com",
                Id = 1,
                IsEmailVerified = true,
                IsTemporaryPassword = false,
                Name = "test"
            });
            context.Add(new Location
            {
                Id = 1,
                Name = "test"
            });
            context.Add(new UserLocation
            {
                Id = 1,
                UserId = 1,
                LocationId = 1,
                State = (int)AdministratorState.Active
            });
            context.Add(new KeyHolder
            {
                Id = 1,
                Name = "test",
                KeySerialNumber = "1234",
                Pin = "1234",
                State = 1,
                LocationId = 1
            });
            context.Add(new UserKeyMapping
            {
                Id = 1,
                UserId = 1,
                KeySerialNumber = "1234",
                LocationId = 1
            });
            context.Add(new UserStatus
            {
                Id = 1,
                Name = "1",
                LocationId = 1,
                IsActive = true,
                IsDefault = true
            });
            context.SaveChanges();

            var service = GetAuthService(context);
            var response = await service.UpdateSecurityQuestion(new UpdateSecurityQuestionReqModel
            {
                Answer = "123",
                Question = "123"
            }, 1);

            response = await service.GetSecurityQuestion("test@test.com");

            var result = Assert.IsType<string>(response.Result);
            Assert.Equal("123", result);

        }

        [Fact]
        public async Task ForgotPassword_Success()
        {
            var context = GetInMemoryContext();
            context.Add(new User
            {
                Email = "test@test.com",
                Id = 1,
                IsEmailVerified = true,
                IsTemporaryPassword = false,
                Name = "test"
            });
            context.Add(new Location
            {
                Id = 1,
                Name = "test"
            });
            context.Add(new UserLocation
            {
                Id = 1,
                UserId = 1,
                LocationId = 1,
                State = (int)AdministratorState.Active
            });
            context.Add(new KeyHolder
            {
                Id = 1,
                Name = "test",
                KeySerialNumber = "1234",
                Pin = "1234",
                State = 1,
                LocationId = 1
            });
            context.Add(new UserKeyMapping
            {
                Id = 1,
                UserId = 1,
                KeySerialNumber = "1234",
                LocationId = 1
            });
            context.Add(new UserStatus
            {
                Id = 1,
                Name = "1",
                LocationId = 1,
                IsActive = true,
                IsDefault = true
            });
            context.SaveChanges();

            var service = GetAuthService(context);
            var response = await service.UpdateSecurityQuestion(new UpdateSecurityQuestionReqModel
            {
                Answer = "123",
                Question = "123"
            }, 1);
            response = await service.ForgotPassword(new PasswordRecoverReqModel
            {
                SecurityQuestion = "123",
                SecurityQuestionReply = "123",
                Email = "test@test.com"
            });

            var user = await context.User.FirstOrDefaultAsync(x => x.Id == 1);
            Assert.True(user.IsTemporaryPassword);

        }

        [Fact]
        public async Task ChangePassword_Success()
        {
            var context = GetInMemoryContext();
            context.Add(new User
            {
                Email = "test@test.com",
                Id = 1,
                IsEmailVerified = true,
                IsTemporaryPassword = false,
                Name = "test"
            });
            context.Add(new Location
            {
                Id = 1,
                Name = "test"
            });
            context.Add(new UserLocation
            {
                Id = 1,
                UserId = 1,
                LocationId = 1,
                State = (int)AdministratorState.Active
            });
            context.Add(new KeyHolder
            {
                Id = 1,
                Name = "test",
                KeySerialNumber = "1234",
                Pin = "1234",
                State = 1,
                LocationId = 1
            });
            context.Add(new UserKeyMapping
            {
                Id = 1,
                UserId = 1,
                KeySerialNumber = "1234",
                LocationId = 1
            });
            context.Add(new UserStatus
            {
                Id = 1,
                Name = "1",
                LocationId = 1,
                IsActive = true,
                IsDefault = true
            });
            context.SaveChanges();

            var service = GetAuthService(context);
            var response = await service.ChangePassword(new ChangePasswordReqModel
            {
               Password = "1234567"
            }, 1);

            var user = await context.User.FirstOrDefaultAsync(x => x.Id == 1);
            Assert.False(user.IsTemporaryPassword);
            Assert.Equal(HashUtility.CreatePasswordHash("1234567", GetAppSettings().Secret), user.PasswordHash);

        }


    }
}
