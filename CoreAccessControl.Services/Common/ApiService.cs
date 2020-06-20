using CoreAccessControl.DataAccess.Ef.Models;
using CoreAccessControl.Domain.ApiResponseModel;
using CoreAccessControl.Domain.Configuration;
using CoreAccessControl.Domain.Models;
using CoreAccessControl.Domain.ResponseModels;
using CoreAccessControl.Services.ApiModel.Response;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;

namespace CoreAccessControl.Services.Common
{
    public class ApiService : IApiService
    {
        private readonly AppSettings _appSettings;
        private readonly IHttpClientFactory _clientFactory;
        private readonly HttpClient _httpClient;

        public ApiService(AppSettings appSettings, IHttpClientFactory clientFactory)
        {
            this._appSettings = appSettings;
            _clientFactory = clientFactory;
            _httpClient = _clientFactory.CreateClient("https");
        }

        private void AddAuthentication(HttpRequestMessage httpRequest)
        {
            httpRequest.Headers.Authorization =
                new AuthenticationHeaderValue(
                    "Basic", Convert.ToBase64String(
                        System.Text.ASCIIEncoding.ASCII.GetBytes(
                           $"{_appSettings.RemoteServer.UserName}:{_appSettings.RemoteServer.Password}")));
        }

        private HttpRequestMessage GetRequest(Uri url, HttpMethod method)
        {
            var request = new HttpRequestMessage()
            {
                RequestUri = url,
                Method = method
            };
            AddAuthentication(request);

            return request;
        }

        public async Task<ServiceResponseResult> GetKeyAuthCode(string keySerialNumber)
        {
            var request = GetRequest(new Uri(_appSettings.RemoteServer.BasePath + $"/api/ver7/AuthenticationCode/{keySerialNumber}"), HttpMethod.Get);
            var response = await _httpClient.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return new ServiceResponseResult
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Result = JsonConvert.DeserializeObject<AuthCodeResponse>(responseContent)
                };
            }
            else
            {
                return new ServiceResponseResult
                {
                    StatusCode = response.StatusCode,
                    Result = JsonConvert.DeserializeObject(responseContent)
                };
            }
        }

        public async Task<ServiceResponseResult> GetAccessHistory(long locationId, AccessHistorySearchReqModel searchReqModel)
        {
            var builder = new UriBuilder(_appSettings.RemoteServer.BasePath + $"/api/ver7/KeyDeviceActivity");
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["deviceOwnerId"] = locationId.ToString();

            PropertyInfo[] props = typeof(AccessHistorySearchReqModel).GetProperties();
            foreach (var prop in props)
            {
                var attr = prop.GetCustomAttributes(true).Where(x => x is FromQueryAttribute).FirstOrDefault();
                var val = prop.GetValue(searchReqModel);
                if(val != null && attr != null)
                {
                    query.Add((attr as FromQueryAttribute).Name, val.ToString());
                }
            }

            builder.Query = query.ToString();
            string url = builder.ToString();
            var request = GetRequest(new Uri(url), HttpMethod.Get);
            var response = await _httpClient.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return new ServiceResponseResult
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Result = JsonConvert.DeserializeObject<AccessHistorySearchResult>(responseContent)
                };
            }
            else
            {
                return new ServiceResponseResult
                {
                    StatusCode = response.StatusCode,
                    Result = JsonConvert.DeserializeObject(responseContent)
                };
            }
        }

        public async Task<ServiceResponseResult> GetDeviceAuthCode(string deviceSerial)
        {
            var request = GetRequest(new Uri(_appSettings.RemoteServer.BasePath + $"/api/ver7/AuthenticationCode?deviceId={deviceSerial}"), HttpMethod.Get);

            var response = await _httpClient.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return new ServiceResponseResult
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Result = JsonConvert.DeserializeObject<AuthCodeResponse>(responseContent)
                };
            }
            else
            {
                return new ServiceResponseResult
                {
                    StatusCode = response.StatusCode,
                    Result = JsonConvert.DeserializeObject(responseContent)
                };
            }
        }

        public async Task<ServiceResponseResult> SearchKeyholder(long locationId, KeyholderSearchReqModel model)
        {
            var request = GetRequest(new Uri(ConstructGetUrl(_appSettings.RemoteServer.BasePath, model, locationId)), HttpMethod.Get);

            var response = await _httpClient.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return new ServiceResponseResult
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Result = JsonConvert.DeserializeObject<KeyholderGetResponseModel>(responseContent)
            };
            }
            else
            {
                return new ServiceResponseResult
                {
                    StatusCode = response.StatusCode,
                    Result = JsonConvert.DeserializeObject(responseContent)
                };
            }
        }

        private string ConstructGetUrl(string remoteServer, KeyholderSearchReqModel model, long locationId)
        {
            var url = remoteServer;
            var query = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(model.Name))
            {
                query.Add("name", model.Name);
            }
            if (!string.IsNullOrEmpty(model.KeySerialNumber))
            {
                query.Add("startKeySerialNumber", model.KeySerialNumber);
                query.Add("endKeySerialNumber", model.KeySerialNumber);
            }
            if (!string.IsNullOrEmpty(model.OrderBy))
            {
                query.Add("orderBy", model.OrderBy);
            }
            if (!string.IsNullOrEmpty(model.OrderDirection))
            {
                query.Add("orderDirection", model.OrderDirection);
            }

            query.Add("skips", model.Skips.ToString());

            if (model.Takes > 0)
            {
                query.Add("takes", model.Takes.ToString());
            }
            query.Add("OwnerId", locationId.ToString());

            return url + "api/ver7/Key?" + string.Join("&", query.Select(x => string.Format("{0}={1}", x.Key, x.Value)));
        }
    }
}
