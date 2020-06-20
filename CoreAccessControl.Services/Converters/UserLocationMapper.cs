using CoreAccessControl.DataAccess.Ef.Models;
using CoreAccessControl.Domain.RequestModels;
using CoreAccessControl.Services.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreAccessControl.Services.Converters
{
    public static class UserLocationMapper
    {        
        public static UserLocation UpdateLastUpdatedOn(this UserLocation userLocation, DateTime lastUpdatedOn)
        {
            userLocation.LastUpdatedOn = lastUpdatedOn;
            return userLocation;
        }

        public static UserLocation UpdateLastUpdatedBy(this UserLocation userLocation, long lastUpdatedBy)
        {
            userLocation.LastUpdatedBy = lastUpdatedBy;
            return userLocation;
        }

        public static UserLocation UpdateIsToolKitEnabled(this UserLocation userLocation, bool isToolKitEnabled)
        {
            userLocation.IsToolKitEnabled = isToolKitEnabled;
            return userLocation;
        }

        public static UserLocation UpdateLocationId(this UserLocation userLocation, long locationId)
        {
            userLocation.LocationId = locationId;
            return userLocation;
        }

    }
}
