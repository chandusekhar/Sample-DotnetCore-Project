using CoreAccessControl.DataAccess.Ef.Data;
using CoreAccessControl.DataAccess.Ef.Models;
using CoreAccessControl.DataAccess.Ef.StoreProcs;
using CoreAccessControl.Domain.ApiResponseModel;
using CoreAccessControl.Domain.Configuration;
using CoreAccessControl.Domain.Models;
using CoreAccessControl.Domain.RequestModels;
using CoreAccessControl.Domain.ResponseModels;
using CoreAccessControl.Services.Common;
using CoreAccessControl.Services.Converters;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Supra.LittleLogger;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CoreAccessControl.Services
{
    public class KeyholderService : IKeyholderService
    {
        private readonly CoreaccesscontrolContext _context;
        private readonly IApiService _apiService;

        public KeyholderService(CoreaccesscontrolContext context, IApiService apiService)
        {
            _context = context;
            _apiService = apiService;
        }

        public async Task<ServiceResponseResult> SearchKeyholder(long locationId, long userId, KeyholderSearchReqModel model)
        {
            Logger.WriteInformation("Searching status.");
            var apiResponse = await _apiService.SearchKeyholder(locationId, model);
            if(apiResponse.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return apiResponse;
            }

            KeyholderGetResponseModel keyholderGetResponse = (KeyholderGetResponseModel)apiResponse.Result;
            var response = new KeyholdResponseModel();

            var localQuery = _context.KeyHolder.AsQueryable();
            if (!string.IsNullOrEmpty(model.Email))
            {
                //localQuery = localQuery.Where(x => x.UserKeyMapping.Any(y => y.User.Any(z => z.Email.Contains(model.Email))));
            }
            if (!string.IsNullOrEmpty(model.Pin))
            {
                localQuery = localQuery.Where(x => x.Pin.Contains(model.Pin));
            }
            if (model.State.HasValue)
            {
                localQuery = localQuery.Where(x => x.State == model.State);
            }
            if (model.StatusId.HasValue)
            {
                localQuery = localQuery.Where(x => x.StatusId == model.StatusId);
            }


            var data = await localQuery.Select(x => new
            {
                Keyhold = x,
                Device = x.KeyholderDevice.Where(y => y.Device.LocationId == locationId).Select(y => new
                {
                    Data = y.Device,
                    Status = y.Device.Status
                }).FirstOrDefault(),
                Space = x.KeyholderSpace.Where(y => y.Space.LocationId == locationId).Select(y => new
                {
                    Data = y.Space,
                    Status = y.Space.Status
                }).FirstOrDefault(),
                Status = x.Status
            }).ToListAsync();

            data.ForEach(x =>
            {
                var apiData = keyholderGetResponse.Data.FirstOrDefault(y => y.SerialNumber.ToString() == x.Keyhold.KeySerialNumber);

                if (apiData != null)
                {
                    var respModel = new KeyholdItem
                    {
                        AllowPinReleaseShackle = apiData.AllowPinReleaseShackle,
                        AssociatedDevices = new AssociatedDevices
                        {
                            DeviceNameId = x.Device.Data.DeviceNameId,
                            Name = x.Device.Data.Name,
                            Id = x.Device.Data.Id,
                            SerialNumber = x.Device.Data.SerialNumber,
                            Status = new Domain.ResponseModels.StatusResponseModel
                            {
                                Name = x.Device.Status.Name,
                                Id = x.Device.Status.Id
                            }
                        },
                        AssociatedSpaces = new AssociatedSpaces
                        {
                            Id = x.Space.Data.Id,
                            Name = x.Space.Data.Name,
                            Status = new Domain.ResponseModels.StatusResponseModel
                            {
                                Name = x.Space.Status.Name,
                                Id = x.Space.Status.Id
                            },
                            State = x.Space.Data.State
                        },
                        Description = apiData.Description,
                        DisabledReason = apiData.DisableReason,
                        Enabled = apiData.Enabled,
                        EnableDebugging = apiData.EnableDebugging,
                        Id = x.Keyhold.Id,
                        KeySerialNumber = int.Parse(x.Keyhold.KeySerialNumber),
                        Name = apiData.Name,
                        Payload = apiData.Payload,
                        Pin = apiData.Pin.ToString(),
                        State = x.Keyhold.State,
                        Status = new Domain.ResponseModels.StatusResponseModel
                        {
                            Name = x.Status.Name,
                            Id = x.Status.Id
                        },
                    };
                    response.Items.Add(respModel);
                }
            });

            response.TotalItems = response.Items.Count;

            return new ServiceResponseResult
            {
                Result = response,
                StatusCode = System.Net.HttpStatusCode.OK
            };
        }
       
    }
}
