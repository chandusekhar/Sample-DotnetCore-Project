using CoreAccessControl.Api.Test.Fakes;
using CoreAccessControl.API.Controllers;
using CoreAccessControl.API.Helpers;
using CoreAccessControl.DataAccess.Ef.Data;
using CoreAccessControl.DataAccess.Ef.Models;
using CoreAccessControl.Domain.Models;
using CoreAccessControl.Domain.RequestModels;
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
    public class AuthenticationCodeControllerTests : ControllerTestBase
    {
        public AuthenticationCodeController GetAuthenticationCodeController(CoreaccesscontrolContext context, IApiService apiService)
        {
            Logger.Init(GetAppSettings().ConnectionStrings.DefaultConnection, "CoreAccessControl.log", "CoreAccessControl", Severity.Information, mock: true);
            var authHelpers = new AuthHelpers(GetMockHttpContextAccessor().Object);
            var service = new AuthenticationCodeService(context, new FakeEmailSender(), new FakeEmailService(), apiService);
            return new AuthenticationCodeController(service, authHelpers);
        }


        [Fact]
        public async Task Get_SuccessRequest()
        {
            var context = GetInMemoryContext();
            var сontroller = GetAuthenticationCodeController(context, new FakeSuccessApiService());
            var response = await сontroller.Get(1, "123", "");
            var response2 = await сontroller.Get(1, "", "123");

            var okResult = Assert.IsType<ObjectResult>(response);
            Assert.Equal((int)HttpStatusCode.OK, (int)okResult.StatusCode.Value);

            var okResult2 = Assert.IsType<ObjectResult>(response2);
            Assert.Equal((int)HttpStatusCode.OK, (int)okResult2.StatusCode.Value);
        }

        [Fact]
        public async Task Get_FailedRequest_ForAPI()
        {
            var context = GetInMemoryContext();
            var сontroller = GetAuthenticationCodeController(context, new FakeFailedApiService());
            var response = await сontroller.Get(1, "123", "");

            var okResult = Assert.IsType<ObjectResult>(response);
            Assert.NotEqual((int)HttpStatusCode.OK, (int)okResult.StatusCode.Value);
        }

        [Fact]
        public async Task Get_BadRequest_BothParamEmpty()
        {
            var context = GetInMemoryContext();
            var сontroller = GetAuthenticationCodeController(context, new FakeSuccessApiService());
            var response = await сontroller.Get(1, "", "");

            var okResult = Assert.IsType<ObjectResult>(response);
            Assert.Equal((int)HttpStatusCode.BadRequest, (int)okResult.StatusCode.Value);
        }

        [Fact]
        public async Task Get_BadRequest_BothParamHasValue()
        {
            var context = GetInMemoryContext();
            var сontroller = GetAuthenticationCodeController(context, new FakeSuccessApiService());
            var response = await сontroller.Get(1, "123", "123");

            var okResult = Assert.IsType<ObjectResult>(response);
            Assert.Equal((int)HttpStatusCode.BadRequest, (int)okResult.StatusCode.Value);        }

        [Fact]
        public async Task Post_SuccessRequest()
        {
            var context = GetInMemoryContext();
            context.Add(new User
            {
                Id = 1,
                Email = "test@test.com"
            });
            context.SaveChanges();
            var сontroller = GetAuthenticationCodeController(context, new FakeSuccessApiService());
            var response = await сontroller.Post(1, new AuthCodeCreateReqModel
            {
                Code = "123"
            });

            var okResult = Assert.IsType<ObjectResult>(response);
            Assert.Equal((int)HttpStatusCode.OK, (int)okResult.StatusCode.Value);
        }

        [Fact]
        public async Task Post_BadRequest_ModelState()
        {
            var context = GetInMemoryContext();

            var сontroller = GetAuthenticationCodeController(context, new FakeSuccessApiService());
            сontroller.ModelState.AddModelError("Code", "Required");
            var response = await сontroller.Post(1, new AuthCodeCreateReqModel { 
                Code = "123"
            });

            Assert.IsType<BadRequestObjectResult>(response);            
        }
    }
}
