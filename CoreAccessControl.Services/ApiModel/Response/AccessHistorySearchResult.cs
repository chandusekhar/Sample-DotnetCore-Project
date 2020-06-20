using System;
using System.Collections.Generic;
using System.Text;

namespace CoreAccessControl.Services.ApiModel.Response
{
    public class AccessHistorySearchResult
    {
        public AccessHistorySearchResult()
        {
            Data = new List<AccessHistoryModel>();
        }
        public int TotalRecords { get; set; }
        public List<AccessHistoryModel> Data { get; set; }
    }

    public class AccessHistoryModel
    {
        public long KeyDeviceActivityId { get; set; }
        public DateTime TransDate { get; set; }
        public DateTime HostEventDate { get; set; }
        public DateTime KeyDate { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime KeyEntryDate { get; set; }
        public long DownloadType { get; set; }
        public string KeyholderInfo { get; set; }
        public string OperationCode { get; set; }
        public string OperationDescription { get; set; }
        public long OperationDurationMs { get; set; }
        public long OperationCommDurationMs { get; set; }
        public long OperationConnectDurationMs { get; set; }
        public long OperationTotalDurationMs { get; set; }
        public long OperationUserIntentDurationMs { get; set; }
        public string KeyboxErrorCode { get; set; }
        public string FrameworkErrorCode { get; set; }
        public long DeviceSerial { get; set; }
        public long DeviceOwnerId { get; set; }
        public string DeviceSystemCode { get; set; }
        public long DeviceBatteryLevel { get; set; }
        public string DeviceVersion { get; set; }
        public DateTime DeviceProgrammingDate { get; set; }
        public long KeySerial { get; set; }
        public long KeyOwnerId { get; set; }
        public string KeySystemCode { get; set; }
        public string KeyName { get; set; }
        public string KeyDescription { get; set; }
        public bool KeyAllowPinReleaseShackle { get; set; }
        public string KeyProduct { get; set; }
        public string KeyVersion { get; set; }
        public DateTimeOffset KeyCreatedDate { get; set; }
        public long DeviceNameId { get; set; }
        public string DeviceName { get; set; }
        public string DeviceNameDescription { get; set; }
        public string Notes { get; set; }
    }
}
