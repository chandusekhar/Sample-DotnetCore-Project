using System;
using System.Collections.Generic;
using System.Text;

namespace CoreAccessControl.DataAccess.Ef.StoreProcs
{
    public class Administrator
    {
        public long UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int? State { get; set; }
        public string DisabledReason { get; set; }
        public long? StatusId { get; set; }
        public string StatusName { get; set; }
        public long? KeyHolderId { get; set; }
        public string KeySerialNumber { get; set; }
        public string Pin { get; set; }
        public bool? HasAdminRead { get; set; }
        public bool? HasAdminEdit { get; set; }
        public bool? HasKeyholderRead { get; set; }
        public bool? HasKeyholderEdit { get; set; }
        public bool? HasDeviceRead { get; set; }
        public bool? HasDeviceEdit { get; set; }
        public bool? HasSpaceRead { get; set; }
        public bool? HasSpaceEdit { get; set; }
        public bool? HasConfigRead { get; set; }
        public bool? HasConfigEdit { get; set; }
        public long? UserActivityId { get; set; }
        public string ActivityText { get; set; }
        public DateTime? ActivityTime { get; set; }
        public long LocationId { get; set; }
    }
}
