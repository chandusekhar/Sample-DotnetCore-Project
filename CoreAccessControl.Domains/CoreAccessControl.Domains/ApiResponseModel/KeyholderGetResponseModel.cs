using System;
using System.Collections.Generic;
using System.Text;

namespace CoreAccessControl.Domain.ApiResponseModel
{
    public partial class KeyholderGetResponseModel
    {
        public long TotalRecords { get; set; }
        public KeyholderData[] Data { get; set; }
    }

    public partial class KeyholderData
    {
        public long SerialNumber { get; set; }
        public Product Product { get; set; }
        public long Pin { get; set; }
        public bool Enabled { get; set; }
        public long OwnerId { get; set; }
        public string KeyVersion { get; set; }
        public bool EnableDebugging { get; set; }
        public string SystemCode { get; set; }
        public DateTimeOffset ProgrammingDate { get; set; }
        public bool AllowPinReleaseShackle { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Payload { get; set; }
        public DateTimeOffset LastSyncDate { get; set; }
        public string DisableReason { get; set; }
    }

    public partial class Product
    {
        public string ProductCode { get; set; }
    }
}
