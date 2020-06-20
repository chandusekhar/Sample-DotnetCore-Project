using CoreAccessControl.DataAccess.Ef.Models;
using CoreAccessControl.Domain.RequestModels;
using CoreAccessControl.Domain.ResponseModels;
using CoreAccessControl.Services.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreAccessControl.Services.Converters
{
    public static class KeyHolderMapper
    {
        public static KeyHolder UpdatePin(this KeyHolder userPermission, int Pin)
        {
            userPermission.Pin = Pin.ToString();
            return userPermission;
        }

        public static KeyHolder UpdateState(this KeyHolder userPermission, int? State)
        {
            userPermission.State = State;
            return userPermission;
        }

        public static KeyHolder UpdateName(this KeyHolder userPermission, string Name)
        {
            userPermission.Name = Name;
            return userPermission;
        }

        public static KeyHolder UpdateKeySerialNumber(this KeyHolder userPermission, int KeySerialNumber)
        {
            userPermission.KeySerialNumber = KeySerialNumber.ToString();
            return userPermission;
        }

        public static KeyHolder UpdateLocationId(this KeyHolder userPermission, long LocationId)
        {
            userPermission.LocationId = LocationId;
            return userPermission;
        }

        public static KeyHolder UpdateId(this KeyHolder userPermission, long id)
        {
            userPermission.Id = id;
            return userPermission;
        }

    }
}
