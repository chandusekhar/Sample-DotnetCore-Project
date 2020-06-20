using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CoreAccessControl.Domain.RequestModels
{
    public class ChangePasswordReqModel
    {
        [Required(ErrorMessage = "Password is required.")]
        [MaxLength(250, ErrorMessage = "Password max length sould be 250 character.")]
        public string Password { get; set; }
    }
}
