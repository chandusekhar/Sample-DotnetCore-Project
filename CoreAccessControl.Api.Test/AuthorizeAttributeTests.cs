using CoreAccessControl.API.Attributes;
using CoreAccessControl.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using Supra.LittleLogger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using Xunit;

namespace CoreAccessControl.Api.Test
{
    public class AuthorizeAttributeTests
    {
        [Fact]
        public void CoreAccessAuthorizeAttribute_When_RouteData_NotHave_Location_ReturnsForbidden()
        {
            Logger.Init("", "CoreAccessControl.log", "CoreAccessControl", Severity.Information, mock: true);
            var accessor = new ControllerTestBase().GetMockHttpContextAccessor();
            ActionContext actionContext = new ActionContext(
                    httpContext: accessor.Object.HttpContext,
                    routeData: new RouteData(),
                    actionDescriptor: new ActionDescriptor()
                );
            AuthorizationFilterContext filterContext = new AuthorizationFilterContext(actionContext, new List<IFilterMetadata>());
            // Act
            CoreAccessAuthorizeAttribute authAttr = new CoreAccessAuthorizeAttribute(Domain.Models.PermissionDomain.Admin, Domain.Models.PermissionAction.Read);
            authAttr.OnAuthorization(filterContext);

            // Assert
            var result = Assert.IsType<ObjectResult>(filterContext.Result);
            Assert.Equal((int)System.Net.HttpStatusCode.Forbidden, (int)result.StatusCode);
        }

        [Fact]
        public void CoreAccessAuthorizeAttribute_When_DoNotHaveEnoughPermission()
        {
            Logger.Init("", "CoreAccessControl.log", "CoreAccessControl", Severity.Information, mock: true);
            var keyVal = new RouteValueDictionary();
            keyVal.Add("locationId", 1);
            var accessor = new ControllerTestBase().GetMockHttpContextAccessor(GetFakeClaims(), keyVal);
            ActionContext actionContext = new ActionContext(
                    httpContext: accessor.Object.HttpContext,
                    routeData: new RouteData(),
                    actionDescriptor: new ActionDescriptor()
                );
            AuthorizationFilterContext filterContext = new AuthorizationFilterContext(actionContext, new List<IFilterMetadata>());
            // Act
            CoreAccessAuthorizeAttribute authAttr = new CoreAccessAuthorizeAttribute(PermissionDomain.Admin, PermissionAction.Read);
            authAttr.OnAuthorization(filterContext);

            // Assert
            var result = Assert.IsType<ObjectResult>(filterContext.Result);
            Assert.Equal((int)System.Net.HttpStatusCode.Forbidden, (int)result.StatusCode);

            // Act
            authAttr = new CoreAccessAuthorizeAttribute(PermissionDomain.Admin, PermissionActionCondition.And, PermissionAction.Read, PermissionAction.Write);
            authAttr.OnAuthorization(filterContext);

            // Assert
            result = Assert.IsType<ObjectResult>(filterContext.Result);
            Assert.Equal((int)System.Net.HttpStatusCode.Forbidden, (int)result.StatusCode);

            // Act
            authAttr = new CoreAccessAuthorizeAttribute(PermissionDomain.Config, PermissionActionCondition.Or, PermissionAction.Read, PermissionAction.Write);
            authAttr.OnAuthorization(filterContext);

            // Assert
            result = Assert.IsType<ObjectResult>(filterContext.Result);
            Assert.Equal((int)System.Net.HttpStatusCode.Forbidden, (int)result.StatusCode);
        }

        private Claim[] GetFakeClaims()
        {
            var jwtPayload = new JWTPayload(true, new long[] { 1 }, "test@tesy.com", 1, "test");
            var permission = new Permission
            {
                HasAdminEdit = true,
                HasAdminRead = false,
                HasConfigEdit = false,
                HasConfigRead = false,
                HasDeviceEdit = false,
                HasDeviceRead = false,
                HasKeyholderEdit = false,
                HasKeyholderRead = false,
                HasSpaceEdit = false,
                HasSpaceRead = false,
                LocationId = 1
            };
            jwtPayload.Permissions = (new Permission[] { permission }).Select(x => new Permission
            {
                HasAdminEdit = x.HasAdminEdit,
                HasAdminRead = x.HasAdminRead,
                HasConfigEdit = x.HasConfigEdit,
                HasConfigRead = x.HasConfigRead,
                HasDeviceEdit = x.HasDeviceEdit,
                HasDeviceRead = x.HasDeviceRead,
                HasKeyholderEdit = x.HasKeyholderEdit,
                HasKeyholderRead = x.HasKeyholderRead,
                HasSpaceEdit = x.HasSpaceEdit,
                HasSpaceRead = x.HasSpaceRead,
                LocationId = x.LocationId
            }).ToArray();

            return new Claim[]
                 {
                    new Claim(ClaimTypes.Email, "test@tesy.com"),
                    new Claim(Constants.JWTPayloadClaim,JsonConvert.SerializeObject(jwtPayload))
                 };
        }
    }
}
