using CoreAccessControl.DataAccess.Ef.Data;
using CoreAccessControl.DataAccess.Ef.Models;
using CoreAccessControl.Domain.Models;
using CoreAccessControl.Domain.RequestModels;
using CoreAccessControl.Domain.ResponseModels;
using CoreAccessControl.Services.ApiModel.Response;
using CoreAccessControl.Services.Common;
using CoreAccessControl.Services.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Supra.LittleLogger;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CoreAccessControl.Services
{
    public class AccessHistoryService : IAccessHistoryService
    {

        private readonly IApiService _apiService;

        public AccessHistoryService(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<ServiceResponseResult> Get(long locationId, AccessHistorySearchReqModel searchReqModel)
        {
            Logger.WriteInformation("Geting access history data.");
            var result = await _apiService.GetAccessHistory(locationId, searchReqModel);

            if(result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var searchResult = ((AccessHistorySearchResult)result.Result);
                result.Result = new AccessHistoryRespModel
                {
                    TotalItems = searchResult.TotalRecords,
                    Items = searchResult.Data.Select(x => new AccessHistorySearchRespModel
                    {
                        DeviceName = x.DeviceName,
                        DeviceNameId = x.DeviceNameId,
                        KeyHolderName = x.KeyName,
                        KeySerialNumber = x.KeySerial,
                        OperationCode = x.OperationCode,
                        OperationDescription = x.OperationDescription,
                        TransDate = x.TransDate,
                        DeviceSerialNumber = x.DeviceSerial,
                        OperationState = x.KeyboxErrorCode,
                        OperationErrorCode = x.KeyboxErrorCode,
                        ErrorCodeText = x.KeyboxErrorCode != "0" ? x.OperationDescription : "",
                        ErrorSolutionText = ""
                    }).ToList()
                };
            }

            Logger.WriteInformation("Geting access history data completed.");
            return result;
        }
    }
}
