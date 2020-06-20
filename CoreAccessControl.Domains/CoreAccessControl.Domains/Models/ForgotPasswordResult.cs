using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace CoreAccessControl.Domain.Models
{
    public class ForgotPasswordResult
    {
        public bool HasError { get; set; }
        public string Error { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }
}
