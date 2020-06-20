using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CoreAccessControl.Domain.RequestModels
{
    public class AuthCodeCreateReqModel
    {
        [Required]
        public string Code { get; set; }
    }
}
