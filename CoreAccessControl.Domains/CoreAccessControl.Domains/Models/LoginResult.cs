using System;
using System.Collections.Generic;
using System.Text;

namespace CoreAccessControl.Domain.Models
{
    public class LoginResult
    {
        public bool HasError { get; set; }
        public string Error { get; set; }
        public string JWT { get; set; }
    }
}
