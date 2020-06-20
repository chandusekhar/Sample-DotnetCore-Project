using CoreAccessControl.Api.Service.Fakes;
using CoreAccessControl.DataAccess.Ef.Data;
using CoreAccessControl.DataAccess.Ef.Models;
using CoreAccessControl.Domain.Models;
using CoreAccessControl.Domain.ResponseModels;
using CoreAccessControl.Services.Test.Fakes;
using Microsoft.EntityFrameworkCore;
using Supra.LittleLogger;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CoreAccessControl.Services.Test
{
    public class AccountServiceTest : ServiceTestBase
    {
        public AccountService GetAccountService(CoreaccesscontrolContext context)
        {
            Logger.Init(GetAppSettings().ConnectionStrings.DefaultConnection, "CoreAccessControl.log", "CoreAccessControl", Severity.Information, mock: true);
            return new AccountService(context, new FakeEmailSender(), new FakeEmailService());
        }


        [Fact]
        public async Task GetAdminProfile_SuccessRequest()
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

            var service = GetAccountService(context);
            var response = await service.GetAdminProfile(1, 1);

            var result = Assert.IsType<AdminProfileRespModel>(response.Result);
            Assert.Equal(1, result.Id);
            Assert.Equal(1, result.LocationId);
            Assert.Equal("test@test.com", result.Email);
            Assert.Equal("test", result.Name);

            var tookit = Assert.IsType<ToolkitInfoRespModel>(result.ToolkitInfo);

            Assert.Equal(1, tookit.Id);
            Assert.Equal("1234", tookit.KeySerialNumber);
            Assert.Equal("1234", tookit.Pin);
        }

        [Fact]
        public async Task UpdateAdminProfile_Sucess()
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

            var service = GetAccountService(context);
            var response = await service.UpdateAdminProfile(new AdminProfileUpdateReqModel
            { 
                Email = "test1@test.com",
                Name = "test1"
            },1, 1);

            var result = Assert.IsType<AdminProfileRespModel>(response.Result);
            Assert.Equal("test1", result.Name);

            var changeEmail = await context.ChangeEmailRequest.FirstAsync(x => x.UserId == 1);
            Assert.Equal("test1@test.com", changeEmail.Email);
            Assert.NotNull(changeEmail.VerificationToken);
        }
    }
}
