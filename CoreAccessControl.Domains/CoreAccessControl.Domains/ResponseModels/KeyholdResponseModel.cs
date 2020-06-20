using System;
using System.Collections.Generic;
using System.Text;

namespace CoreAccessControl.Domain.ResponseModels
{
    public partial class KeyholdResponseModel
    {
        public KeyholdResponseModel()
        {
            Items = new List<KeyholdItem>();
        }
        public long TotalItems { get; set; }
        public List<KeyholdItem> Items { get; set; }
    }

    public partial class KeyholdItem
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
        public int? State { get; set; }
        public StatusResponseModel Status { get; set; }
        public AssociatedSpaces AssociatedSpaces { get; set; }
        public AssociatedDevices AssociatedDevices { get; set; }
    }

    public partial class AssociatedDevices
    {
        public long Id { get; set; }
        public string SerialNumber { get; set; }
        public int? DeviceNameId { get; set; }
        public string Name { get; set; }
        public string State { get; set; }
        public StatusResponseModel Status { get; set; }
    }

    public partial class StatusResponseModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }

    public partial class StatusDetailsResponseModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsDefault { get; set; }
        public bool IsActive { get; set; }
    }

    public partial class AssociatedSpaces
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int? State { get; set; }
        public StatusResponseModel Status { get; set; }
    }
}
