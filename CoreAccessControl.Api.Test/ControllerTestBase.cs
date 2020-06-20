using CoreAccessControl.DataAccess.Ef.Data;
using CoreAccessControl.Domain.Configuration;
using CoreAccessControl.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace CoreAccessControl.Api.Test
{
    public class ControllerTestBase
    {
        public CoreaccesscontrolContext GetInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<CoreaccesscontrolContext>()
                .UseInMemoryDatabase(databaseName: this.GetType().Name)
                .Options;

            var context = new CoreaccesscontrolContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            return context;
        }

        public Mock<IHttpContextAccessor> GetMockHttpContextAccessor()
        {
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            //var context = new DefaultHttpContext();
            //context.User.Claims.in = GetFakeClaims();
            //var fakeTenantId = "abcd";
            //context.Request.Headers["Authorization"] = $"Bearer {CreateFakeToken()}";
            //mockHttpContextAccessor.Setup(_ => _.HttpContext).Returns(context);

            //return mockHttpContextAccessor;

            var claims = GetFakeClaims();
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);
            var routeValues = new RouteValueDictionary();
            
            var mockRequest = new Mock<HttpRequest>();
            mockRequest.Setup(x => x.RouteValues).Returns(routeValues);

            var mockPrincipal = new Mock<IPrincipal>();
            mockPrincipal.Setup(x => x.Identity).Returns(identity);
            mockPrincipal.Setup(x => x.IsInRole(It.IsAny<string>())).Returns(true);

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(m => m.User).Returns(claimsPrincipal);
            mockHttpContext.Setup(m => m.Request).Returns(mockRequest.Object);
            mockHttpContextAccessor.Setup(_ => _.HttpContext).Returns(mockHttpContext.Object);
            return mockHttpContextAccessor;
        }

        public AppSettings GetAppSettings()
        {
            var path = Path.GetFullPath("..\\..\\..\\appsettings.json");
            return JsonConvert.DeserializeObject<AppSettings>(File.ReadAllText(path));
        }

        public Mock<IHttpContextAccessor> GetMockHttpContextAccessor(Claim[] claims, RouteValueDictionary keyValuePairs)
        {
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();

            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);
            var routeValues = keyValuePairs;

            var mockRequest = new Mock<HttpRequest>();
            mockRequest.Setup(x => x.RouteValues).Returns(routeValues);

            var mockPrincipal = new Mock<IPrincipal>();
            mockPrincipal.Setup(x => x.Identity).Returns(identity);
            mockPrincipal.Setup(x => x.IsInRole(It.IsAny<string>())).Returns(true);

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(m => m.User).Returns(claimsPrincipal);
            mockHttpContext.Setup(m => m.Request).Returns(mockRequest.Object);
            mockHttpContextAccessor.Setup(_ => _.HttpContext).Returns(mockHttpContext.Object);
            return mockHttpContextAccessor;
        }

        private Claim[] GetFakeClaims()
        {
            var jwtPayload = new JWTPayload(true, new long[] { 1 }, "test@tesy.com", 1, "test");
            var permission = new Permission
            {
                HasAdminEdit = true,
                HasAdminRead = true,
                HasConfigEdit = true,
                HasConfigRead = true,
                HasDeviceEdit = true,
                HasDeviceRead = true,
                HasKeyholderEdit = true,
                HasKeyholderRead = true,
                HasSpaceEdit = true,
                HasSpaceRead = true,
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

    public static class CoreaccesscontrolContextExtensions
    {

    }
}
