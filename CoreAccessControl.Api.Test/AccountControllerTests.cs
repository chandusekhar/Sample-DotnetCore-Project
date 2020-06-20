using CoreAccessControl.Api.Test.Fakes;
using CoreAccessControl.API.Controllers;
using CoreAccessControl.API.Helpers;
using CoreAccessControl.DataAccess.Ef.Data;
using CoreAccessControl.DataAccess.Ef.Models;
using CoreAccessControl.Domain.Models;
using CoreAccessControl.Domain.ResponseModels;
using CoreAccessControl.Services;
using CoreAccessControl.Services.ApiModel.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Supra.LittleLogger;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CoreAccessControl.Api.Test
{
    public class AccountControllerTests : ControllerTestBase
    {
        public AccountController GetAccountController(CoreaccesscontrolContext context)
        {
            Logger.Init(GetAppSettings().ConnectionStrings.DefaultConnection, "CoreAccessControl.log", "CoreAccessControl", Severity.Information, mock: true);
            var authHelpers = new AuthHelpers(GetMockHttpContextAccessor().Object);
            var service = new AccountService(context, new FakeEmailSender(), new FakeEmailService());
            return new AccountController(service, authHelpers);
        }

        [Fact]
        public async Task GetProfile_SuccessRequest()
        {
            var context = GetInMemoryContext();
            context.Location.Add(new Location
            {
                Id = 1
            });
            context.UserLocation.Add(new UserLocation
            {
                Id = 1,
                UserId = 1,
                LocationId = 1
            });
            context.User.Add(new User
            {
                Id = 1,
                Email = "test@test.com"
            });
            context.SaveChanges();
            var сontroller = GetAccountController(context);
            var response = await сontroller.GetProfile(1);

            var okResult = Assert.IsType<ObjectResult>(response);
            Assert.Equal((int)HttpStatusCode.OK, (int)okResult.StatusCode.Value);
        }

        [Fact]
        public async Task GetProfile_SuccessRequest_DifferentKey()
        {
            var context = GetInMemoryContext();
            context.Location.Add(new Location
            {
                Id = 1
            });
            context.UserLocation.Add(new UserLocation
            {
                Id = 1,
                UserId = 1,
                LocationId = 1
            });
            context.User.Add(new User
            {
                Id = 1,
                Email = "test@test.com"
            });
            context.KeyHolder.Add(new KeyHolder
            {
                Id = 1,
                KeySerialNumber = "123"
            });
            context.UserKeyMapping.Add(new UserKeyMapping
            {
                Id = 1,
                KeySerialNumber = "123",
                UserId = 1,
                LocationId = 2,
            });
            context.SaveChanges();
            var сontroller = GetAccountController(context);
            var response = await сontroller.GetProfile(1);

            var okResult = Assert.IsType<ObjectResult>(response);
            Assert.Equal((int)HttpStatusCode.OK, (int)okResult.StatusCode.Value);
        }

        [Fact]
        public async Task UpdateProfile_SuccessRequest_UpdateName()
        {
            var context = GetInMemoryContext();
            context.Location.Add(new Location
            {
                Id = 1
            });
            context.UserLocation.Add(new UserLocation
            {
                Id = 1,
                UserId = 1,
                LocationId = 1
            });
            context.User.Add(new User
            {
                Id = 1,
                Email = "test@test.com"
            });
            context.SaveChanges();
            var сontroller = GetAccountController(context);
            var response = await сontroller.UpdateProfile(1, new AdminProfileUpdateReqModel { 
                Name = "test"
            });

            var okResult = Assert.IsType<ObjectResult>(response);
            Assert.Equal((int)HttpStatusCode.OK, (int)okResult.StatusCode.Value);
        }

        [Fact]
        public async Task UpdateProfile_SuccessRequest_UpdateAll()
        {
            var context = GetInMemoryContext();
            context.Location.Add(new Location
            {
                Id = 1
            });
            context.UserLocation.Add(new UserLocation
            {
                Id = 1,
                UserId = 1,
                LocationId = 1
            });
            context.User.Add(new User
            {
                Id = 1,
                Email = "test@test.com"
            });
            context.SaveChanges();
            var сontroller = GetAccountController(context);
            var response = await сontroller.UpdateProfile(1, new AdminProfileUpdateReqModel
            {
                Name = "test",
                Email = "test@test1.com"
            });

            var okResult = Assert.IsType<ObjectResult>(response);
            Assert.Equal((int)HttpStatusCode.OK, (int)okResult.StatusCode.Value);
        }

        [Fact]
        public async Task UpdateProfile_BadRequest_DuplicateEmailSameUser()
        {
            var context = GetInMemoryContext();
            context.Location.Add(new Location
            {
                Id = 1
            });
            context.UserLocation.Add(new UserLocation
            {
                Id = 1,
                UserId = 1,
                LocationId = 1
            });
            context.User.Add(new User
            {
                Id = 1,
                Email = "test@test.com"
            });
            context.SaveChanges();
            var сontroller = GetAccountController(context);
            var response = await сontroller.UpdateProfile(1, new AdminProfileUpdateReqModel
            {
                Name = "test",
                Email = "test@test.com"
            });

            var okResult = Assert.IsType<ObjectResult>(response);
            Assert.Equal((int)HttpStatusCode.BadRequest, (int)okResult.StatusCode.Value);
        }

        [Fact]
        public async Task UpdateProfile_BadRequest_DuplicateEmailDifferentUser()
        {
            var context = GetInMemoryContext();
            context.Location.Add(new Location
            {
                Id = 1
            });
            context.UserLocation.Add(new UserLocation
            {
                Id = 1,
                UserId = 1,
                LocationId = 1
            });
            context.User.Add(new User
            {
                Id = 1,
                Email = "test@test.com"
            });
            context.User.Add(new User
            {
                Id = 2,
                Email = "test@tes1.com"
            });
            context.SaveChanges();
            var сontroller = GetAccountController(context);
            var response = await сontroller.UpdateProfile(1, new AdminProfileUpdateReqModel
            {
                Name = "test",
                Email = "test@tes1.com"
            });

            var okResult = Assert.IsType<ObjectResult>(response);
            Assert.Equal((int)HttpStatusCode.BadRequest, (int)okResult.StatusCode.Value);
        }

        [Fact]
        public async Task UpdateProfile_BadRequest_DuplicateEmailDifferentUserChangeEmail()
        {
            var context = GetInMemoryContext();
            context.Location.Add(new Location
            {
                Id = 1
            });
            context.UserLocation.Add(new UserLocation
            {
                Id = 1,
                UserId = 1,
                LocationId = 1
            });
            context.User.Add(new User
            {
                Id = 1,
                Email = "test@test.com"
            });
            context.ChangeEmailRequest.Add(new ChangeEmailRequest
            {
                Id = 2,
                Email = "test@tes1.com"
            });
            context.SaveChanges();
            var сontroller = GetAccountController(context);
            var response = await сontroller.UpdateProfile(1, new AdminProfileUpdateReqModel
            {
                Name = "test",
                Email = "test@tes1.com"
            });

            var okResult = Assert.IsType<ObjectResult>(response);
            Assert.Equal((int)HttpStatusCode.BadRequest, (int)okResult.StatusCode.Value);
        }

    }
}
