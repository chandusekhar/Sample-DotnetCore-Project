using System;
using System.Collections.Generic;
using System.Text;

namespace CoreAccessControl.Domain.ResponseModels
{
    public class AdminProfileRespModel
    {
        public long Id { get; set; }
        public long LocationId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public ToolkitInfoRespModel ToolkitInfo { get; set; }        
    }

    public class ToolkitInfoRespModel
    {
        public long Id { get; set; }
        public string KeySerialNumber { get; set; }
        public string Pin { get; set; }
    }
}
