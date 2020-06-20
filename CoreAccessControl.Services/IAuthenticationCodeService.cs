using CoreAccessControl.Domain.Models;
using CoreAccessControl.Domain.RequestModels;
using CoreAccessControl.Domain.ResponseModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CoreAccessControl.Services
{
    public interface IAuthenticationCodeService
    {
        Task<ServiceResponseResult> GetCode(long locationId, long userId, string keySerialNumber, string deviceSerialNumber);
        Task<ServiceResponseResult> SendCode(AuthCodeCreateReqModel model, long userId, long locationId);
    }
}
