using CoreAccessControl.Domain.ApiResponseModel;
using CoreAccessControl.Domain.Models;
using CoreAccessControl.Domain.ResponseModels;
using CoreAccessControl.Services;
using CoreAccessControl.Services.ApiModel.Response;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CoreAccessControl.Api.Test.Fakes
{
    public class FakeSuccessApiService : IApiService
    {
        public async Task<ServiceResponseResult> GetAccessHistory(long locationId, AccessHistorySearchReqModel searchReqModel)
        {
            return await Task.FromResult(new ServiceResponseResult
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Result = new Mock<AccessHistorySearchResult>().Object
            }); ;
        }

        public async Task<ServiceResponseResult> GetDeviceAuthCode(string deviceSerial)
        {
            return await Task.FromResult(new ServiceResponseResult
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Result = new AuthCodeResponse()
            });
        }

        public async Task<ServiceResponseResult> GetKeyAuthCode(string keySerialNumber)
        {
            return await Task.FromResult(new ServiceResponseResult
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Result = new AuthCodeResponse()
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
