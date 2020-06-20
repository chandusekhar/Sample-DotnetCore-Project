using CoreAccessControl.Api.Test.Fakes;
using CoreAccessControl.API.Controllers;
using CoreAccessControl.API.Helpers;
using CoreAccessControl.DataAccess.Ef.Data;
using CoreAccessControl.DataAccess.Ef.Models;
using CoreAccessControl.Domain.Configuration;
using CoreAccessControl.Domain.Models;
using CoreAccessControl.Domain.RequestModels;
using CoreAccessControl.Domain.ResponseModels;
using CoreAccessControl.Services;
using CoreAccessControl.Services.ApiModel.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Supra.LittleLogger;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CoreAccessControl.Api.Test
{
    public class AdminControllerTests : ControllerTestBase
    {
        public AdminController GetAdminController(CoreaccesscontrolContext context)
        {
            Logger.Init(GetAppSettings().ConnectionStrings.DefaultConnection, "CoreAccessControl.log", "CoreAccessControl", Severity.Information, mock: true);
            var authHelpers = new AuthHelpers(GetMockHttpContextAccessor().Object);
            var service = new AdminService(context, GetAppSettings(), new FakeEmailSender(), new FakeEmailService());
            var authService = new AuthService(context, GetAppSettings(), new FakeEmailSender(), new FakeEmailService());
            return new AdminController(service, authHelpers, authService);
        }

        [Fact]
        public async Task Create_Update_SuccessRequest()
        {
            var context = GetInMemoryContext();
            context.Location.Add(new Location
            {
                Id = 1
            });
            context.UserLocation.Add(new UserLocation
            {
                Id = 1,
                UserId = 1,
                LocationId = 1
            });
            context.User.Add(new User
            {
                Id = 1,
                Email = "test@test.com"
            });
            context.SaveChanges();
            var сontroller = GetAdminController(context);
            var response = await сontroller.Create(1, new AdministratorReqModel
            {
                Email = "test@test.com",
                KeySerialNumber = "123",
                Name = "qewew",
                Permissions = new PermissionsReqModel
                {
                    HasAdminRead = true
                }
            });

            var okResult = Assert.IsType<ObjectResult>(response);
            Assert.Equal((int)HttpStatusCode.OK, (int)okResult.StatusCode.Value);
        }

        [Fact]
        public async Task Create_Create_SuccessRequest()
        {
            var context = GetInMemoryContext();
            context.Location.Add(new Location
            {
                Id = 1
            });
            context.KeyHolder.Add(new KeyHolder
            {
                Id = 1,
                KeySerialNumber = "123"
            });
            context.SaveChanges();
            var сontroller = GetAdminController(context);
            var response = await сontroller.Create(1, new AdministratorReqModel
            {
                Email = "test@test.com",
                KeySerialNumber = "123",
                Name = "qewew",
                Permissions = new PermissionsReqModel
                {
                    HasAdminRead = true
                }
            });

            var okResult = Assert.IsType<ObjectResult>(response);
            Assert.Equal((int)HttpStatusCode.OK, (int)okResult.StatusCode.Value);
        }

        [Fact]
        public async Task Create_BadRequest_ModelState()
        {
            var context = GetInMemoryContext();
            context.Location.Add(new Location
            {
                Id = 1
            });
            context.UserLocation.Add(new UserLocation
            {
                Id = 1,
                UserId = 1,
                LocationId = 1
            });
            context.User.Add(new User
            {
                Id = 1,
                Email = "test@test.com"
            });
            context.SaveChanges();
            var сontroller = GetAdminController(context);
            сontroller.ModelState.AddModelError("email", "Email is required");
            var response = await сontroller.Create(1, new AdministratorReqModel
            {
                Email = "test@test.com",
                KeySerialNumber = "123",
                Name = "qewew",
                Permissions = new PermissionsReqModel
                {
                    HasAdminRead = true
                }
            });
            Assert.IsType<BadRequestObjectResult>(response);
        }

        [Fact]
        public async Task Create_BadRequest_UserPermissionAdded()
        {
            var context = GetInMemoryContext();
            context.Location.Add(new Location
            {
                Id = 1
            });
            context.UserLocation.Add(new UserLocation
            {
                Id = 1,
                UserId = 1,
                LocationId = 1
            });
            context.User.Add(new User
            {
                Id = 1,
                Email = "test@test.com"
            });
            context.UserPermission.Add(new UserPermission
            {
                Id = 1,
                UserLocationId = 1
            });

            context.SaveChanges();
            var сontroller = GetAdminController(context);
            var response = await сontroller.Create(1, new AdministratorReqModel
            {
                Email = "test@test.com",
                KeySerialNumber = "123",
                Name = "qewew",
                Permissions = new PermissionsReqModel
                {
                    HasAdminRead = true
                }
            });


            var okResult = Assert.IsType<ObjectResult>(response);
            Assert.Equal((int)HttpStatusCode.BadRequest, (int)okResult.StatusCode.Value);
        }

        [Fact]
        public async Task Create_NotFound()
        {
            var context = GetInMemoryContext();
            context.Location.Add(new Location
            {
                Id = 2
            });
            context.UserLocation.Add(new UserLocation
            {
                Id = 1,
                UserId = 1,
                LocationId = 2
            });
            context.User.Add(new User
            {
                Id = 1,
                Email = "test@test.com"
            });
            context.SaveChanges();
            var сontroller = GetAdminController(context);
            var response = await сontroller.Create(1, new AdministratorReqModel
            {
                Email = "test@test.com",
                KeySerialNumber = "123",
                Name = "qewew",
                Permissions = new PermissionsReqModel
                {
                    HasAdminRead = true
                }
            });


            var okResult = Assert.IsType<ObjectResult>(response);
            Assert.Equal((int)HttpStatusCode.NotFound, (int)okResult.StatusCode.Value);
        }

        [Fact]
        public async Task Update_BadRequest_ModelState()
        {
            var context = GetInMemoryContext();
            context.Location.Add(new Location
            {
                Id = 1
            });
            context.UserLocation.Add(new UserLocation
            {
                Id = 1,
                UserId = 1,
                LocationId = 1
            });
            context.User.Add(new User
            {
                Id = 1,
                Email = "test@test.com"
            });
            context.SaveChanges();
            var сontroller = GetAdminController(context);
            сontroller.ModelState.AddModelError("email", "Email is required");
            var response = await сontroller.Update(1, 1, new AdministratorUpdateReqModel
            {
                Email = "test@test.com",
                KeySerialNumber = "123",
                Name = "qewew",
                Permissions = new PermissionsReqModel
                {
                    HasAdminRead = true
                }
            });
            Assert.IsType<BadRequestObjectResult>(response);
        }

        [Fact]
        public async Task Update_BadRequest_DisableState_NoReason()
        {
            var context = GetInMemoryContext();
            context.Location.Add(new Location
            {
                Id = 1
            });
            context.UserLocation.Add(new UserLocation
            {
                Id = 1,
                UserId = 1,
                LocationId = 1
            });
            context.User.Add(new User
            {
                Id = 1,
                Email = "test@test.com"
            });
            context.SaveChanges();
            var сontroller = GetAdminController(context);
            var response = await сontroller.Update(1, 1, new AdministratorUpdateReqModel
            {
                Email = "test@test.com",
                KeySerialNumber = "123",
                Name = "qewew",
                Permissions = new PermissionsReqModel
                {
                    HasAdminRead = true
                },
                State = AdministratorState.Disabled
            });
            Assert.IsType<BadRequestObjectResult>(response);
        }

        [Fact]
        public async Task Update_NotFound()
        {
            var context = GetInMemoryContext();
            context.Location.Add(new Location
            {
                Id = 2
            });
            context.UserLocation.Add(new UserLocation
            {
                Id = 1,
                UserId = 1,
                LocationId = 2
            });
            context.User.Add(new User
            {
                Id = 1,
                Email = "test@test.com"
            });
            context.SaveChanges();
            var сontroller = GetAdminController(context);
            var response = await сontroller.Update(1, 1, new AdministratorUpdateReqModel
            {
                Email = "test@test.com",
                KeySerialNumber = "123",
                Name = "qewew",
                Permissions = new PermissionsReqModel
                {
                    HasAdminRead = true
                },
                State = AdministratorState.Active
            });


            var okResult = Assert.IsType<ObjectResult>(response);
            Assert.Equal((int)HttpStatusCode.NotFound, (int)okResult.StatusCode.Value);
        }

        [Fact]
        public async Task Update_BadRequest_DuplicateEmail()
        {
            var context = GetInMemoryContext();
            context.Location.Add(new Location
            {
                Id = 1
            });
            context.UserLocation.Add(new UserLocation
            {
                Id = 1,
                UserId = 1,
                LocationId = 1
            });
            context.User.Add(new User
            {
                Id = 1,
                Email = "test@test.com"
            });
            context.User.Add(new User
            {
                Id = 2,
                Email = "test1@test.com"
            });
            context.UserPermission.Add(new UserPermission
            {
                Id = 1,
                UserLocationId = 1
            });

            context.SaveChanges();
            var сontroller = GetAdminController(context);
            var response = await сontroller.Update(1, 1, new AdministratorUpdateReqModel
            {
                Email = "test1@test.com",
                KeySerialNumber = "123",
                Name = "qewew",
                Permissions = new PermissionsReqModel
                {
                    HasAdminRead = true
                }
            });


            var okResult = Assert.IsType<ObjectResult>(response);
            Assert.Equal((int)HttpStatusCode.BadRequest, (int)okResult.StatusCode.Value);
        }

        [Fact]
        public async Task Update_SuccessRequest()
        {
            var context = GetInMemoryContext();
            context.Location.Add(new Location
            {
                Id = 1
            });
            context.UserLocation.Add(new UserLocation
            {
                Id = 1,
                UserId = 1,
                LocationId = 1
            });
            context.User.Add(new User
            {
                Id = 1,
                Email = "test@test.com"
            });
            context.User.Add(new User
            {
                Id = 2,
                Email = "test1@test.com"
            });
            context.UserPermission.Add(new UserPermission
            {
                Id = 1,
                UserLocationId = 1
            });

            context.SaveChanges();
            var сontroller = GetAdminController(context);
            var response = await сontroller.Update(1, 1, new AdministratorUpdateReqModel
            {
                Email = "test@test.com",
                KeySerialNumber = "123",
                Name = "qewew",
                Permissions = new PermissionsReqModel
                {
                    HasAdminRead = true
                }
            });


            var okResult = Assert.IsType<ObjectResult>(response);
            Assert.Equal((int)HttpStatusCode.OK, (int)okResult.StatusCode.Value);
        }

        [Fact]
        public async Task Delete_SuccessRequest()
        {
            var context = GetInMemoryContext();
            context.Location.Add(new Location
            {
                Id = 1
            });
            context.UserLocation.Add(new UserLocation
            {
                Id = 1,
                UserId = 1,
                LocationId = 1
            });
            context.User.Add(new User
            {
                Id = 1,
                Email = "test@test.com"
            });
            context.User.Add(new User
            {
                Id = 2,
                Email = "test1@test.com"
            });
            context.UserPermission.Add(new UserPermission
            {
                Id = 1,
                UserLocationId = 1
            });

            context.SaveChanges();
            var сontroller = GetAdminController(context);
            var response = await сontroller.Delete(1, 1);


            var okResult = Assert.IsType<ObjectResult>(response);
            Assert.Equal((int)HttpStatusCode.OK, (int)okResult.StatusCode.Value);
        }

        [Fact]
        public async Task Delete_NotFound()
        {
            var context = GetInMemoryContext();
            context.Location.Add(new Location
            {
                Id = 2
            });
            context.UserLocation.Add(new UserLocation
            {
                Id = 1,
                UserId = 1,
                LocationId = 2
            });
            context.User.Add(new User
            {
                Id = 1,
                Email = "test@test.com"
            });
            context.SaveChanges();
            var сontroller = GetAdminController(context);
            var response = await сontroller.Delete(1, 1);


            var okResult = Assert.IsType<ObjectResult>(response);
            Assert.Equal((int)HttpStatusCode.NotFound, (int)okResult.StatusCode.Value);
        }

        [Fact]
        public async Task Activities_SuccessRequest()
        {
            var context = GetInMemoryContext();
            context.Location.Add(new Location
            {
                Id = 1
            });
            context.UserLocation.Add(new UserLocation
            {
                Id = 1,
                UserId = 1,
                LocationId = 1
            });
            context.User.Add(new User
            {
                Id = 1,
                Email = "test@test.com"
            });
            context.User.Add(new User
            {
                Id = 2,
                Email = "test1@test.com"
            });
            context.UserPermission.Add(new UserPermission
            {
                Id = 1,
                UserLocationId = 1
            });

            context.SaveChanges();
            var сontroller = GetAdminController(context);
            var response = await сontroller.Activities(1, 1, new QueryReqModel
            {
            });


            var okResult = Assert.IsType<ObjectResult>(response);
            Assert.Equal((int)HttpStatusCode.OK, (int)okResult.StatusCode.Value);
        }

        [Fact]
        public async Task Activities_NotFound()
        {
            var context = GetInMemoryContext();
            context.Location.Add(new Location
            {
                Id = 2
            });
            context.UserLocation.Add(new UserLocation
            {
                Id = 1,
                UserId = 1,
                LocationId = 2
            });
            context.User.Add(new User
            {
                Id = 1,
                Email = "test@test.com"
            });
            context.SaveChanges();
            var сontroller = GetAdminController(context);
            var response = await сontroller.Activities(1, 1, new QueryReqModel { });


            var okResult = Assert.IsType<ObjectResult>(response);
            Assert.Equal((int)HttpStatusCode.NotFound, (int)okResult.StatusCode.Value);
        }

    }
}
