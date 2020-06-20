using CoreAccessControl.Api.Service.Fakes;
using CoreAccessControl.DataAccess.Ef.Data;
using CoreAccessControl.Domain.Models;
using CoreAccessControl.Domain.ResponseModels;
using Supra.LittleLogger;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CoreAccessControl.Services.Test
{
    public class AccessHistoryServiceTest : ServiceTestBase
    {
        public AccessHistoryService GetAccessHistoryService(IApiService apiService)
        {
            Logger.Init(GetAppSettings().ConnectionStrings.DefaultConnection, "CoreAccessControl.log", "CoreAccessControl", Severity.Information, mock : true);
            return new AccessHistoryService(apiService);
        }


        [Fact]
        public async Task Get_SuccessRequest()
        {
            var context = GetInMemoryContext();
            var service = GetAccessHistoryService(new FakeSuccessApiService());
            var response = await service.Get(1, new AccessHistorySearchReqModel
            {
                
            });

            var result = Assert.IsType<AccessHistoryRespModel>(response.Result);
            Assert.Equal(11, result.TotalItems);

            var first = result.Items[0];
            Assert.Equal("00", first.OperationCode);
            Assert.Equal(50062591, first.KeySerialNumber);
            Assert.Equal(44491288, first.DeviceSerialNumber);
        }
    }
}
