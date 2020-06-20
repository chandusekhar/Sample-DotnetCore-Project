using CoreAccessControl.Api.Test.Fakes;
using CoreAccessControl.API.Controllers;
using CoreAccessControl.API.Helpers;
using CoreAccessControl.DataAccess.Ef.Data;
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
    public class AccessHistoryControllerTests : ControllerTestBase
    {
        public AccessHistoryController GetAccessHistoryController(CoreaccesscontrolContext context, IApiService apiService)
        {
            Logger.Init(GetAppSettings().ConnectionStrings.DefaultConnection, "CoreAccessControl.log", "CoreAccessControl", Severity.Information, mock: true);
            var authHelpers = new AuthHelpers(GetMockHttpContextAccessor().Object);
            var service = new AccessHistoryService(apiService);
            return new AccessHistoryController(service, authHelpers);
        }


        [Fact]
        public async Task Get_SuccessRequest()
        {
            var context = GetInMemoryContext();
            var сontroller = GetAccessHistoryController(context, new FakeSuccessApiService());
            var response = await сontroller.Get(1, new AccessHistorySearchReqModel
            {

            });

            var okResult = Assert.IsType<ObjectResult>(response);
            Assert.Equal((int)HttpStatusCode.OK, (int)okResult.StatusCode.Value);
        }

        [Fact]
        public async Task Get_FailedRequest_FailedApi()
        {
            var context = GetInMemoryContext();
            var сontroller = GetAccessHistoryController(context, new FakeFailedApiService());
            var response = await сontroller.Get(1, new AccessHistorySearchReqModel
            {

            });

            var okResult = Assert.IsType<ObjectResult>(response);
            Assert.NotEqual((int)HttpStatusCode.OK, (int)okResult.StatusCode.Value);
        }

        [Fact]
        public async Task Export_SuccessRequest()
        {
            var context = GetInMemoryContext();
            var сontroller = GetAccessHistoryController(context, new FakeSuccessApiService());
            var response = await сontroller.Export(1, new AccessHistorySearchReqModel
            {

            }, "excel");

            Assert.IsType<FileContentResult>(response);
        }

        [Fact]
        public async Task Export_FailedReuest_FromApi()
        {
            var context = GetInMemoryContext();
            var сontroller = GetAccessHistoryController(context, new FakeFailedApiService());
            var response = await сontroller.Export(1, new AccessHistorySearchReqModel
            {

            }, "excel");

            var okResult = Assert.IsType<ObjectResult>(response);
            Assert.NotEqual((int)HttpStatusCode.OK, (int)okResult.StatusCode.Value);
        }

        [Fact]
        public async Task Export_BadRequest_DifferentTYpeThanExcel()
        {
            var context = GetInMemoryContext();
            var сontroller = GetAccessHistoryController(context, new FakeFailedApiService());
            var response = await сontroller.Export(1, new AccessHistorySearchReqModel
            {

            }, "");

            Assert.IsType<BadRequestObjectResult>(response);
        }
    }
}
