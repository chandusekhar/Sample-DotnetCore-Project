using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CoreAccessControl.Domain.ResponseModels
{
    public class QueryReqModel
    {
        [Required]
        [FromQuery(Name = "skips")]
        public int Skips { get; set; } = 0;
        [FromQuery(Name = "takes")]
        [Required]
        public int Takes { get; set; } = 0;
        [FromQuery(Name = "orderby")]
        public string OrderBy { get; set; }
        [FromQuery(Name = "orderDirection")]
        public string OrderDirection { get; set; }
    }
}
