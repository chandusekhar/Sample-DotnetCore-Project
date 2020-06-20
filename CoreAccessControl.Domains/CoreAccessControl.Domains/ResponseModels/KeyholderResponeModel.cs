using System;
using System.Collections.Generic;
using System.Text;

namespace CoreAccessControl.Domain.ResponseModels
{
    public partial class KeyholderResponeModel
    {
        public long Id { get; set; }
        public long KeySerialNumber { get; set; }
        public string Name { get; set; }
        public bool AllowPinReleaseShackle { get; set; }
        public string Description { get; set; }
        public string Pin { get; set; }
        public bool Enabled { get; set; }
        public string DisabledReason { get; set; }
        public bool EnableDebugging { get; set; }
        public string Payload { get; set; }
        public string State { get; set; }
        public StatusResponseModel Status { get; set; }
    }
}
