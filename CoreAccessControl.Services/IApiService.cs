using CoreAccessControl.DataAccess.Ef.Models;
using CoreAccessControl.Domain.Models;
using CoreAccessControl.Domain.ResponseModels;
using System.Threading.Tasks;

namespace CoreAccessControl.Services
{
    public interface IApiService
    {
        Task<ServiceResponseResult> GetKeyAuthCode(string keySerialNumber);
        Task<ServiceResponseResult> GetDeviceAuthCode(string deviceSerial);
        Task<ServiceResponseResult> GetAccessHistory(long locationId, AccessHistorySearchReqModel searchReqModel);
        Task<ServiceResponseResult> SearchKeyholder(long locationId, KeyholderSearchReqModel model);
    }
}
