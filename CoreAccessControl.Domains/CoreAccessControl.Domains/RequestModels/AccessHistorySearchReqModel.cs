using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CoreAccessControl.Domain.ResponseModels
{
    public class AccessHistorySearchReqModel
    {
        [FromQuery(Name = "deviceName")]
        public string DeviceName { get; set; }
        [FromQuery(Name = "operationCode")]
        public string OperationCode { get; set; }
        [FromQuery(Name = "entryDateSEnd")]
        public DateTime? EntryDateSEnd { get; set; }
        [FromQuery(Name = "entryDateStart")]
        public DateTime? EntryDateStart { get; set; }
        [FromQuery(Name = "tranDateEnd")]
        public DateTime? TranDateEnd { get; set; }
        [FromQuery(Name = "tranDateStart")]
        public DateTime? TranDateStart { get; set; }
        [FromQuery(Name = "deviceSerialNumber")]
        public string DeviceSerialNumber { get; set; }
        [FromQuery(Name = "keySerialNumber")]
        public string KeySerialNumber { get; set; }
        [FromQuery(Name = "onlyToolKitUser")]
        public bool OnlyToolKitUser { get; set; }
        [BindRequired]
        [FromQuery(Name = "skips")]
        public int Skips { get; set; } = 0;
        [FromQuery(Name = "takes")]
        [BindRequired]
        public int Takes { get; set; } = 0;
        [FromQuery(Name = "orderby")]
        public string OrderBy { get; set; }
        [FromQuery(Name = "orderDirection")]
        public string OrderDirection { get; set; }
    }
}
