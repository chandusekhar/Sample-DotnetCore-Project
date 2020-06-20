using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CoreAccessControl.Domain.Models
{
    public enum AdministratorState
    {
        Active = 1,
        Invited,
        Inactive,
        Disabled
    }

    public enum KeyholderState
    {
        Active = 1,
        Inactive,
        Disabled
    }

    public enum DeviceState
    {
        Active = 1, Inactive, Unregistered, Tampered, [Display(Name = "Low Battery")] LowBattery, Error, Disabled
    }

    public enum SpaceState
    {
        Active = 1,
        Inactive,
        Disabled
    }

    public enum UserLocationState
    {
        Invited
    }

    public enum PermissionDomain
    {
        Admin,
        Key,
        Space,
        Device,
        Config
    }

    public enum PermissionAction
    {
        Read,
        Write
    }

    public enum PermissionActionCondition
    {
        And,
        Or
    }
}
