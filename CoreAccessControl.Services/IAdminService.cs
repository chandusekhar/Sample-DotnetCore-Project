using CoreAccessControl.Domain.Models;
using CoreAccessControl.Domain.RequestModels;
using CoreAccessControl.Domain.ResponseModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CoreAccessControl.Services
{
    public interface IAdminService
    {
        Task<ServiceResponseResult> SearchAdmin(long locationId, long userId, AdminSearchReqModel model);
        Task<ServiceResponseResult> CreateAdmin(long locationId, long userId, AdministratorReqModel model);
        Task<ServiceResponseResult> UpdatedAdmin(long locationId, long userId, AdministratorReqModel model);
        Task<ServiceResponseResult> UpdatedAdmin(long locationId, long userId, long adminUserId, AdministratorUpdateReqModel model);
        Task<ServiceResponseResult> DeleteAdmin(long locationId, long userId, long adminUserId);
        Task<ServiceResponseResult> GetActivities(long locationId, long value, long administratorId, QueryReqModel query);
    }
}
