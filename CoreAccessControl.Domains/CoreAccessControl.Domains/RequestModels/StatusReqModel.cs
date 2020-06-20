using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CoreAccessControl.Domain.RequestModels
{
    public partial class StatusReqModel
    {
        [Required(ErrorMessage = "Id is required.")]
        public long Id { get; set; }
    }

    public partial class ConfigStatusReqModel
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }
        public bool IsDefault { get; set; }
    }
}
