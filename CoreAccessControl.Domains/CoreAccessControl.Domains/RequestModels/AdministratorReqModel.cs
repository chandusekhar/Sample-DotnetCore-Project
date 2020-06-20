using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CoreAccessControl.Domain.RequestModels
{
    public partial class AdministratorReqModel
    {
        [Required(ErrorMessage = "Name is required.")]
        [MaxLength(250, ErrorMessage = "Name max length sould be 250 character.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        [MaxLength(250, ErrorMessage = "Email max length sould be 250 character.")]
        [RegularExpression(@"[a-z0-9!#$%&'*+/=?^_‘{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_‘{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?", ErrorMessage = "Email is not valid")]
        public string Email { get; set; }
        public PermissionsReqModel Permissions { get; set; }
        [Required(ErrorMessage = "KeySerialNumber is required.")]
        [MaxLength(10, ErrorMessage = "KeySerialNumber max length sould be 10 character.")]
        public string KeySerialNumber { get; set; }
    }

    public partial class PermissionsReqModel
    {
        public bool? HasAdminRead { get; set; }
        public bool? HasAdminWrite { get; set; }
        public bool? HasKeyholderRead { get; set; }
        public bool? HasKeyholderWrite { get; set; }
        public bool? HasDeviceRead { get; set; }
        public bool? HasDeviceWrite { get; set; }
        public bool? HasSpaceRead { get; set; }
        public bool? HasSpaceWrite { get; set; }
        public bool? HasConfigRead { get; set; }
        public bool? HasConfigWrite { get; set; }
    }

}
