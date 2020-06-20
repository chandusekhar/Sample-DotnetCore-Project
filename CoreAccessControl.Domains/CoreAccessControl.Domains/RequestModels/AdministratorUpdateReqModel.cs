using CoreAccessControl.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CoreAccessControl.Domain.RequestModels
{
    public partial class AdministratorUpdateReqModel
    {
        [Required(ErrorMessage = "Name is required.")]
        [MaxLength(250, ErrorMessage = "Name max length sould be 250 character.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        [MaxLength(250, ErrorMessage = "Email max length sould be 250 character.")]
        [RegularExpression(@"[a-z0-9!#$%&'*+/=?^_‘{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_‘{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?", ErrorMessage = "Email is not valid")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Permissions is required.")]
        public PermissionsReqModel Permissions { get; set; }
        public string KeySerialNumber { get; set; }
        public AdministratorState? State { get; set; }
        public string DisabledReason { get; set; }
        public StatusReqModel Status { get; set; }
    }
}
