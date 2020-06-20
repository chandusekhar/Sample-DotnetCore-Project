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
    public class KeyholderControllerTests : ControllerTestBase
    {
        public KeyholderController GetKeyholderController(CoreaccesscontrolContext context, IApiService apiService)
        {
            Logger.Init("", "CoreAccessControl.log", "CoreAccessControl", Severity.Information, mock: true);
            var authHelpers = new AuthHelpers(GetMockHttpContextAccessor().Object);
            var service = new KeyholderService(context, apiService);
            return new KeyholderController(service, authHelpers);
        }


        [Fact]
        public async Task Get_SuccessRequest()
        {
            var context = GetInMemoryContext();
            var сontroller = GetKeyholderController(context, new FakeSuccessApiService());
            var response = await сontroller.Get(1, new KeyholderSearchReqModel { });

            var okResult = Assert.IsType<ObjectResult>(response);
            Assert.Equal((int)HttpStatusCode.OK, (int)okResult.StatusCode.Value);
        }

        [Fact]
        public async Task Get_FailedRequest()
        {
            var context = GetInMemoryContext();
            var сontroller = GetKeyholderController(context, new FakeFailedApiService());
            var response = await сontroller.Get(1, new KeyholderSearchReqModel { });

            var okResult = Assert.IsType<ObjectResult>(response);
            Assert.NotEqual((int)HttpStatusCode.OK, (int)okResult.StatusCode.Value);
        }
       
    }
}
