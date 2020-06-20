using CoreAccessControl.DataAccess.Ef.Models;
using CoreAccessControl.Domain.RequestModels;
using CoreAccessControl.Domain.ResponseModels;
using CoreAccessControl.Services.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreAccessControl.Services.Converters
{
    public static class UserPermissionMapper
    {


        public static UserPermission ToUserPermission(PermissionsReqModel model)
        {
            return new UserPermission
            {
                HasAdminEdit = model.HasAdminWrite,
                HasAdminRead = model.HasAdminRead,
                HasConfigEdit = model.HasConfigWrite,
                HasConfigRead = model.HasConfigRead,
                HasDeviceEdit = model.HasDeviceWrite,
                HasDeviceRead = model.HasDeviceRead,
                HasKeyholderEdit = model.HasKeyholderWrite,
                HasKeyholderRead = model.HasKeyholderRead,
                HasSpaceEdit = model.HasSpaceWrite,
                HasSpaceRead = model.HasSpaceRead,
                LastUpdatedOn = DateTime.UtcNow
            };
        }

        public static UserPermissionResult ToUserPermissionResult(UserPermission model)
        {
            return new UserPermissionResult
            {
                HasAdminEdit = !model.HasAdminEdit.HasValue || model.HasAdminEdit.Value,
                HasAdminRead = !model.HasAdminRead.HasValue || model.HasAdminRead.Value,
                HasConfigEdit = !model.HasConfigEdit.HasValue || model.HasConfigEdit.Value,
                HasConfigRead = !model.HasConfigRead.HasValue || model.HasConfigRead.Value,
                HasDeviceEdit = !model.HasDeviceEdit.HasValue || model.HasDeviceEdit.Value,
                HasDeviceRead = !model.HasDeviceRead.HasValue || model.HasDeviceRead.Value,
                HasKeyholderEdit = !model.HasKeyholderEdit.HasValue || model.HasKeyholderEdit.Value,
                HasKeyholderRead = !model.HasKeyholderRead.HasValue || model.HasKeyholderRead.Value,
                HasSpaceEdit = !model.HasSpaceEdit.HasValue || model.HasSpaceEdit.Value,
                HasSpaceRead = !model.HasSpaceRead.HasValue || model.HasSpaceRead.Value,
            };
        }


        public static UserPermission UpdateLastUpdatedOn(this UserPermission userPermission, DateTime lastUpdatedOn)
        {
            userPermission.LastUpdatedOn = lastUpdatedOn;
            return userPermission;
        }

        public static UserPermission UpdateLastUpdatedBy(this UserPermission userPermission, long lastUpdatedBy)
        {
            userPermission.LastUpdatedBy = lastUpdatedBy;
            return userPermission;
        }

        public static UserPermission UpdateUserLocationId(this UserPermission userPermission, long userPermissionLocationId)
        {
            userPermission.UserLocationId = userPermissionLocationId;
            return userPermission;
        }

        public static UserPermission UpdateId(this UserPermission userPermission, long id)
        {
            userPermission.Id = id;
            return userPermission;
        }

    }
}
