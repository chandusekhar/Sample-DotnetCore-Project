using CoreAccessControl.Api.Service.Fakes;
using CoreAccessControl.DataAccess.Ef.Data;
using CoreAccessControl.Domain.Models;
using CoreAccessControl.Domain.ResponseModels;
using CoreAccessControl.Services.ApiModel.Response;
using CoreAccessControl.Services.Test.Fakes;
using Supra.LittleLogger;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CoreAccessControl.Services.Test
{
    public class AuthenticationCodeServiceTest : ServiceTestBase
    {
        public AuthenticationCodeService GetAuthenticationCodeService(CoreaccesscontrolContext context, IApiService apiService)
        {
            Logger.Init(GetAppSettings().ConnectionStrings.DefaultConnection, "CoreAccessControl.log", "CoreAccessControl", Severity.Information, mock: true);
            return new AuthenticationCodeService(context, new FakeEmailSender(), new FakeEmailService(), apiService);
        }


        [Fact]
        public async Task Get_KeyCOde_SuccessRequest()
        {
            var context = GetInMemoryContext();
            var service = GetAuthenticationCodeService(context, new FakeSuccessApiService());
            var response = await service.GetCode(1, 1, "123", "");

            var result = Assert.IsType<AuthCodeResponse>(response.Result);

            Assert.Equal("6WZPFDUDYPRUXJUDRYO6DBTJRM", result.Code);
        }

        [Fact]
        public async Task Get_DeviceCOde_SuccessRequest()
        {
            var context = GetInMemoryContext();
            var service = GetAuthenticationCodeService(context, new FakeSuccessApiService());
            var response = await service.GetCode(1, 1, "", "123");

            var result = Assert.IsType<AuthCodeResponse>(response.Result);

            Assert.Equal("6WZPFDUDYPRUXJUDRYO6DBTJRM", result.Code);
        }
    }
}
