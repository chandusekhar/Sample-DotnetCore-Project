using System;
using System.Collections.Generic;
using System.Text;

namespace CoreAccessControl.Domain.ResponseModels
{
    public class AccessHistoryRespModel
    {
        public int TotalItems { get; set; }
        public List<AccessHistorySearchRespModel> Items { get; set; }
    }

    public class AccessHistorySearchRespModel
    {
        public DateTime TransDate { get; set; }
        public long KeySerialNumber { get; set; }
        public long DeviceSerialNumber { get; set; }
        public string KeyHolderName { get; set; }
        public string OperationCode { get; set; }
        public string OperationDescription { get; set; }
        public long DeviceNameId { get; set; }
        public string DeviceName { get; set; }
        public string OperationState { get; set; }
        public string OperationErrorCode { get; set; }
        public string ErrorCodeText { get; set; }
        public string ErrorSolutionText { get; set; }
    }
}
