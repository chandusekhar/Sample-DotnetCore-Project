using CoreAccessControl.Domain.Models;
using CoreAccessControl.Domain.RequestModels;
using CoreAccessControl.Domain.ResponseModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CoreAccessControl.Services
{
    public interface IConfigService
    {
        Task<ServiceResponseResult> GetStatuses(long locationId, string type, bool isFull);
        Task<ServiceResponseResult> SaveStatuse(long locationId, long userId, string type, ConfigStatusReqModel model);
        Task<ServiceResponseResult> UpdateStatuse(long locationId, long userId, long id, string type, ConfigStatusReqModel model);
        Task<ServiceResponseResult> DeleteStatuse(long locationId, long userId, long id, string type);
    }
}
