using CoreAccessControl.DataAccess.Ef.Models;
using CoreAccessControl.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreAccessControl.Domain.ResponseModels
{
    public class AdministratorSearchResult
    {
        public int TotalItems { get; set; }
        public List<AdministratorResult> Items { get; set; }
    }

    public class AdministratorResult
    {
        public AdministratorResult()
        {

        }
        public AdministratorResult(User user, UserLocation userLocation)
        {
            Id = user.Id;
            Email = user.Email;
            Name = user.Name;
            State = Enum.Parse<AdministratorState>(userLocation.State.ToString());
            DisabledReason = userLocation.DisabledReason;
        }

        public void AddPermission(UserPermission userPermission)
        {
            Permissions = new UserPermissionResult(userPermission);
        }

        public void AddToolkit(KeyHolder keyHolder)
        {
            ToolkitInfo = new ToolkitInfoRespModel
            {
                Id = keyHolder.Id,
                KeySerialNumber = keyHolder.KeySerialNumber,
                Pin = keyHolder.Pin
            };
        }

        public void AddStatus(UserStatus status)
        {
            Status = new LookupEntityResult
            {
                Id = status.Id,
                Name = status.Name
            };
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public AdministratorState State { get; set; }
        public string DisabledReason { get; set; }
        public LookupEntityResult Status { get; set; }
        public ToolkitInfoRespModel ToolkitInfo { get; set; }
        public UserPermissionResult Permissions { get; set; }
        public ActivityResult RecentActivity { get; set; }
    }

    public class LookupEntityResult
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }

    public class ActivityResult
    {
        public long Id { get; set; }
        public string ActivityText { get; set; }
        public DateTime? ActivityTime { get; set; }
    }

    public class UserPermissionResult
    {
        public UserPermissionResult()
        {

        }
        public UserPermissionResult(UserPermission permission)
        {
            HasAdminRead = permission.HasAdminEdit.HasValue && permission.HasAdminEdit.Value;
            HasAdminRead = permission.HasAdminRead.HasValue && permission.HasAdminRead.Value;
            HasConfigEdit = permission.HasConfigEdit.HasValue && permission.HasConfigEdit.Value;
            HasConfigRead = permission.HasConfigRead.HasValue && permission.HasConfigRead.Value;
            HasDeviceEdit = permission.HasDeviceEdit.HasValue && permission.HasDeviceEdit.Value;
            HasDeviceRead = permission.HasDeviceRead.HasValue && permission.HasDeviceRead.Value;
            HasSpaceEdit = permission.HasSpaceEdit.HasValue && permission.HasSpaceEdit.Value;
            HasSpaceRead = permission.HasSpaceRead.HasValue && permission.HasSpaceRead.Value;
            HasKeyholderEdit = permission.HasKeyholderEdit.HasValue && permission.HasKeyholderEdit.Value;
            HasKeyholderRead = permission.HasKeyholderRead.HasValue && permission.HasKeyholderRead.Value;
        }

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
