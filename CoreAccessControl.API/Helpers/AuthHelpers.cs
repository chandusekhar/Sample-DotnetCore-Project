using CoreAccessControl.Domain.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreAccessControl.API.Helpers
{
    public class AuthHelpers
    {
        IHttpContextAccessor _httpContextAccessor;
        public AuthHelpers(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        internal long? GetCurrentUserId()
        {
            // handle the request  
            return IsAuthenticated() ? GetJWTPayload().Id : (long?)null;
        }

        internal bool IsAuthenticated()
        {
            // handle the request  
            return _httpContextAccessor.HttpContext.User != null;
        }

        internal JWTPayload GetJWTPayload()
        {
            // handle the request  
            return JsonConvert.DeserializeObject<JWTPayload>(_httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == Constants.JWTPayloadClaim).Value);
        }

        internal Permission GetPermission(long locationId)
        {
            // handle the request  
            return GetJWTPayload().Permissions.FirstOrDefault(x => x.LocationId == locationId);
        }
    }
}
