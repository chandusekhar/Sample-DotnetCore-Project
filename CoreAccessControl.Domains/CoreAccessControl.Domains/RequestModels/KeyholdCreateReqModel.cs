using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CoreAccessControl.Domain.RequestModels
{
    public partial class KeyholdCreateReqModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public bool AllowPinReleaseShackle { get; set; }
        public string Description { get; set; }
        [Required]
        public string Pin { get; set; }
        [Required]
        public string Payload { get; set; }
        [Required]
        public int State { get; set; }
        [Required]
        public StatusReqModel Status { get; set; }
    }
}
