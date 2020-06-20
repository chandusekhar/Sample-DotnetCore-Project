using System;
using System.Collections.Generic;
using System.Text;

namespace CoreAccessControl.Domain.RequestModels
{
    public class KeyholderPutRequestModel
    {
        public long Id { get; set; }
        public int KeySerialNumber { get; set; }
        public string Name { get; set; }
        public bool AllowPinReleaseShackle { get; set; }
        public string Description { get; set; }
        public int Pin { get; set; }
        public bool Enabled { get; set; }
        public string DisabledReason { get; set; }
        public bool EnableDebugging { get; set; }
        public string Payload { get; set; }
        public int? State { get; set; }
        public StatusReqModel Status { get; set; }
    }
}
