using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CoreAccessControl.Domain.ResponseModels
{
    public class AdminSearchReqModel
    {
        [FromQuery(Name = "onlyToolKitUser")]
        public bool? OnlyToolKitUser { get; set; }
        [FromQuery(Name = "id")]
        public string Id { get; set; }
        [FromQuery(Name = "name")]
        public string Name { get; set; }
        [FromQuery(Name = "email")]
        public string Email { get; set; }
        [FromQuery(Name = "state")]
        public long? State { get; set; }
        [FromQuery(Name = "statusid")]
        public long? StatusId { get; set; }
        [FromQuery(Name = "type")]
        public string Type { get; set; }
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
