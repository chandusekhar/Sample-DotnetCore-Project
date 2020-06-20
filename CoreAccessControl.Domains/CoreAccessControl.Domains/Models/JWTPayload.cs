using System;
using System.Collections.Generic;
using System.Text;

namespace CoreAccessControl.Domain.Models
{
    public class JWTPayload
    {
        public JWTPayload(bool isLoggedIn, long[] locationIds, string email, long id, string name)
        {
            IsLoggedIn = isLoggedIn;
            LocationIds = locationIds;
            Email = email;
            Id = id;
            Name = name;
        }        

        public string Email { get; set; }
        public long Id { get; set; }
        public string Name { get; set; }
        public long[] LocationIds { get; set; }
        public Permission[] Permissions { get; set; }
        public bool IsLoggedIn { get; set; }
    }

    public class Permission
    {
        public long LocationId { get; set; }
        public bool HasAdminRead { get; set; }
        public bool HasAdminEdit { get; set; }
        public bool HasKeyholderRead { get; set; }
        public bool HasKeyholderEdit { get; set; }
        public bool HasDeviceRead { get; set; }
        public bool HasDeviceEdit { get; set; }
        public bool HasSpaceRead { get; set; }
        public bool HasSpaceEdit { get; set; }
        public bool HasConfigRead { get; set; }
        public bool HasConfigEdit { get; set; }
    }
}
