using CoreAccessControl.Domain.ApiResponseModel;
using CoreAccessControl.Domain.Models;
using CoreAccessControl.Domain.ResponseModels;
using CoreAccessControl.Services;
using CoreAccessControl.Services.ApiModel.Response;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CoreAccessControl.Api.Service.Fakes
{
    public class FakeSuccessApiService : IApiService
    {
        public async Task<ServiceResponseResult> GetAccessHistory(long locationId, AccessHistorySearchReqModel searchReqModel)
        {
            return await Task.FromResult(new ServiceResponseResult
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Result = JsonConvert.DeserializeObject<AccessHistorySearchResult>(
                    File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "../../../", "ApiFakeResponseFiles", "GetAccessHistory.json")))
            });
        }

        public async Task<ServiceResponseResult> GetDeviceAuthCode(string deviceSerial)
        {
            return await Task.FromResult(new ServiceResponseResult
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Result = JsonConvert.DeserializeObject<AuthCodeResponse>(
                    File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "../../../", "ApiFakeResponseFiles", "GetDeviceAuthCode.json")))
            });
        }

        public async Task<ServiceResponseResult> GetKeyAuthCode(string keySerialNumber)
        {
            return await Task.FromResult(new ServiceResponseResult
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Result = JsonConvert.DeserializeObject<AuthCodeResponse>(
                    File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "../../../", "ApiFakeResponseFiles", "GetKeyAuthCode.json")))
            });
        }

        public async Task<ServiceResponseResult> SearchKeyholder(long locationId, KeyholderSearchReqModel model)
        {
            return await Task.FromResult(new ServiceResponseResult
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Result = new Mock<KeyholderGetResponseModel>().Object
            });
        }
    }

    public class FakeFailedApiService : IApiService
    {
        public async Task<ServiceResponseResult> GetAccessHistory(long locationId, AccessHistorySearchReqModel searchReqModel)
        {
            return await Task.FromResult(new ServiceResponseResult
            {
                StatusCode = System.Net.HttpStatusCode.BadRequest
            });
        }

        public async Task<ServiceResponseResult> GetDeviceAuthCode(string deviceSerial)
        {
            return await Task.FromResult(new ServiceResponseResult
            {
                StatusCode = System.Net.HttpStatusCode.BadRequest
            });
        }

        public async Task<ServiceResponseResult> GetKeyAuthCode(string keySerialNumber)
        {
            return await Task.FromResult(new ServiceResponseResult
            {
                StatusCode = System.Net.HttpStatusCode.BadRequest
            });
        }

        public async Task<ServiceResponseResult> SearchKeyholder(long locationId, KeyholderSearchReqModel model)
        {
            return await Task.FromResult(new ServiceResponseResult
            {
                StatusCode = System.Net.HttpStatusCode.BadRequest
            });
        }
    }

}
