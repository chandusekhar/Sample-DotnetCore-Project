using CoreAccessControl.Domain.Models;
using CoreAccessControl.Domain.RequestModels;
using CoreAccessControl.Domain.ResponseModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CoreAccessControl.Services
{
    public interface IAccountService
    {
        Task<ServiceResponseResult> GetAdminProfile(long locationId, long userId);
        Task<ServiceResponseResult> UpdateAdminProfile(AdminProfileUpdateReqModel model, long userId, long locationId);
    }
}
