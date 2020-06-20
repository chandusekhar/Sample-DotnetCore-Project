using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace CoreAccessControl.Domain.Models
{
    public class ServiceResponseResult
    {
        public HttpStatusCode StatusCode { get; set; }

        public int GetStatusCode()
        {
            return (int)StatusCode;
        }

        public object Result { get; set; }
    }

    public class ServiceCollectionResult
    {
        public int TotalItems { get; set; }
        public Object Items { get; set; }
    }
}
