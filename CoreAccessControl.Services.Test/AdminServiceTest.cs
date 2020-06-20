using CoreAccessControl.Api.Service.Fakes;
using CoreAccessControl.DataAccess.Ef.Data;
using CoreAccessControl.DataAccess.Ef.Models;
using CoreAccessControl.Domain.Models;
using CoreAccessControl.Domain.RequestModels;
using CoreAccessControl.Domain.ResponseModels;
using CoreAccessControl.Services.Test.Fakes;
using Microsoft.EntityFrameworkCore;
using Supra.LittleLogger;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CoreAccessControl.Services.Test
{
    public class AdminServiceTest : ServiceTestBase
    {
        public AdminService GetAdminService(CoreaccesscontrolContext context)
        {
            Logger.Init(GetAppSettings().ConnectionStrings.DefaultConnection, "CoreAccessControl.log", "CoreAccessControl", Severity.Information, mock: true);
            return new AdminService(context, GetAppSettings(), new FakeEmailSender(), new FakeEmailService());
        }


        [Fact]
        public async Task CreateAdmin_SuccessRequest()
        {
            var context = GetInMemoryContext();
            context.Add(new User
            {
                Email = "test@test.com",
                Id = 1,
                IsEmailVerified = true,
                IsTemporaryPassword = false,
                Name = "test"
            });
            context.Add(new Location
            {
                Id = 1,                
                Name = "test"
            });
            context.Add(new KeyHolder
            {
                Id = 1,
                Name = "test",
                KeySerialNumber = "1234",
                Pin = "1234",
                State = 1,
                LocationId = 1
            });
            context.Add(new UserKeyMapping
            {
                Id = 1,
                UserId = 1,
                KeySerialNumber = "1234",
                LocationId = 1
            });
            context.SaveChanges();

            var service = GetAdminService(context);
            var response = await service.CreateAdmin(1, 1, new AdministratorReqModel
            {
                Email = "test1@test.com",
                Name = "test1",
                KeySerialNumber = "1234",
                Permissions = new PermissionsReqModel
                {
                    HasAdminRead = true,
                    HasConfigRead = true,
                    HasDeviceRead = true
                }
            });

            var result = Assert.IsType<AdministratorResult>(response.Result);
            Assert.Equal(2, result.Id);
            Assert.Equal("test1@test.com", result.Email);
            Assert.Equal("test1", result.Name);

            var tookit = Assert.IsType<ToolkitInfoRespModel>(result.ToolkitInfo);

            Assert.Equal(1, tookit.Id);
            Assert.Equal("1234", tookit.KeySerialNumber);
            Assert.Equal("1234", tookit.Pin);

            var permission = Assert.IsType<UserPermissionResult>(result.Permissions);

            Assert.True(permission.HasAdminRead);
            Assert.True(permission.HasConfigRead);

            var user = await context.User.FirstOrDefaultAsync(x => x.Id == result.Id);
            Assert.NotNull(user.PasswordHash);
            Assert.True(user.IsTemporaryPassword);
            Assert.True(user.IsEmailVerified);

            var userLocation = await context.UserLocation.FirstOrDefaultAsync(x => x.UserId == result.Id && x.LocationId == 1);
            Assert.Equal((int)AdministratorState.Invited, userLocation.State);
        }

        [Fact]
        public async Task UpdateAdminProfile_Sucess()
        {
            var context = GetInMemoryContext();
            context.Add(new User
            {
                Email = "test@test.com",
                Id = 1,
                IsEmailVerified = true,
                IsTemporaryPassword = false,
                Name = "test"
            });
            context.Add(new Location
            {
                Id = 1,
                Name = "test"
            });
            context.Add(new UserLocation
            {
                Id = 1,
                UserId = 1,
                LocationId = 1,
                State = (int)AdministratorState.Active
            });
            context.Add(new KeyHolder
            {
                Id = 1,
                Name = "test",
                KeySerialNumber = "1234",
                Pin = "1234",
                State = 1,
                LocationId = 1
            });
            context.Add(new UserKeyMapping
            {
                Id = 1,
                UserId = 1,
                KeySerialNumber = "1234",
                LocationId = 1
            });
            context.SaveChanges();

            var service = GetAdminService(context);
            var response = await service.UpdatedAdmin(1, 1, new AdministratorReqModel
            {
                Email = "test@test.com",
                Name = "test1",
                KeySerialNumber = "1234",
                Permissions = new PermissionsReqModel
                {
                    HasAdminRead = true
                }
            });

            var result = Assert.IsType<AdministratorResult>(response.Result);
            Assert.Equal(1, result.Id);
            Assert.Equal("test@test.com", result.Email);
            Assert.Equal("test1", result.Name);

            var tookit = Assert.IsType<ToolkitInfoRespModel>(result.ToolkitInfo);

            Assert.Equal(1, tookit.Id);
            Assert.Equal("1234", tookit.KeySerialNumber);
            Assert.Equal("1234", tookit.Pin);

            var permission = Assert.IsType<UserPermissionResult>(result.Permissions);

            Assert.True(permission.HasAdminRead);
        }

        [Fact]
        public async Task UpdateAdminProfile_Put_Sucess()
        {
            var context = GetInMemoryContext();
            context.Add(new User
            {
                Email = "test@test.com",
                Id = 1,
                IsEmailVerified = true,
                IsTemporaryPassword = false,
                Name = "test"
            });
            context.Add(new Location
            {
                Id = 1,
                Name = "test"
            });
            context.Add(new UserLocation
            {
                Id = 1,
                UserId = 1,
                LocationId = 1,
                State = (int)AdministratorState.Active
            });
            context.Add(new KeyHolder
            {
                Id = 1,
                Name = "test",
                KeySerialNumber = "1234",
                Pin = "1234",
                State = 1,
                LocationId = 1
            });
            context.Add(new UserKeyMapping
            {
                Id = 1,
                UserId = 1,
                KeySerialNumber = "1234",
                LocationId = 1
            });
            context.Add(new UserStatus
            {
                Id = 1,
                Name = "1",                
                LocationId = 1,
                IsActive = true,
                IsDefault = true
            });
            context.SaveChanges();

            var service = GetAdminService(context);
            var response = await service.UpdatedAdmin(1, 2, 1, new AdministratorUpdateReqModel
            {
                Email = "test2@test.com",
                Name = "test1",
                KeySerialNumber = "1234",
                Permissions = new PermissionsReqModel
                {
                    HasConfigRead = true
                },
                State = AdministratorState.Active,
                Status = new StatusReqModel
                {
                    Id = 1
                }
            });

            var result = Assert.IsType<AdministratorResult>(response.Result);
            Assert.Equal(1, result.Id);
            Assert.Equal("test@test.com", result.Email);
            Assert.Equal("test1", result.Name);

            var tookit = Assert.IsType<ToolkitInfoRespModel>(result.ToolkitInfo);

            Assert.Equal(1, tookit.Id);
            Assert.Equal("1234", tookit.KeySerialNumber);
            Assert.Equal("1234", tookit.Pin);

            var permission = Assert.IsType<UserPermissionResult>(result.Permissions);

            Assert.True(permission.HasConfigRead);

            var status = Assert.IsType<LookupEntityResult>(result.Status);
            Assert.Equal(1, status.Id);
            Assert.Equal("1", status.Name);

            var changeEmail = await context.ChangeEmailRequest.FirstOrDefaultAsync(x => x.UserId == 1);
            Assert.Equal("test2@test.com", changeEmail.Email);
            Assert.NotNull(changeEmail.VerificationToken);

        }

        [Fact]
        public async Task DeleteAdmin_Sucess()
        {
            var context = GetInMemoryContext();
            context.Add(new User
            {
                Email = "test@test.com",
                Id = 1,
                IsEmailVerified = true,
                IsTemporaryPassword = false,
                Name = "test"
            });
            context.Add(new Location
            {
                Id = 1,
                Name = "test"
            });
            context.Add(new UserLocation
            {
                Id = 1,
                UserId = 1,
                LocationId = 1,
                State = (int)AdministratorState.Active
            });
            context.Add(new KeyHolder
            {
                Id = 1,
                Name = "test",
                KeySerialNumber = "1234",
                Pin = "1234",
                State = 1,
                LocationId = 1
            });
            context.Add(new UserKeyMapping
            {
                Id = 1,
                UserId = 1,
                KeySerialNumber = "1234",
                LocationId = 1
            });
            context.Add(new UserStatus
            {
                Id = 1,
                Name = "1",
                LocationId = 1,
                IsActive = true,
                IsDefault = true
            });
            context.SaveChanges();

            var service = GetAdminService(context);
            var response = await service.DeleteAdmin(1, 2, 1);

            var userLocation = await context.UserLocation.FirstOrDefaultAsync(x => x.UserId == 1 && x.LocationId == 1);
            Assert.Equal((int)AdministratorState.Inactive, userLocation.State);

        }

        [Fact]
        public async Task GetActivities_Success()
        {
            var context = GetInMemoryContext();
            context.Add(new User
            {
                Email = "test@test.com",
                Id = 1,
                IsEmailVerified = true,
                IsTemporaryPassword = false,
                Name = "test"
            });
            context.Add(new Location
            {
                Id = 1,
                Name = "test"
            });
            context.Add(new UserLocation
            {
                Id = 1,
                UserId = 1,
                LocationId = 1,
                State = (int)AdministratorState.Active
            });
            context.Add(new KeyHolder
            {
                Id = 1,
                Name = "test",
                KeySerialNumber = "1234",
                Pin = "1234",
                State = 1,
                LocationId = 1
            });
            context.Add(new UserKeyMapping
            {
                Id = 1,
                UserId = 1,
                KeySerialNumber = "1234",
                LocationId = 1
            });
            context.Add(new UserStatus
            {
                Id = 1,
                Name = "1",
                LocationId = 1,
                IsActive = true,
                IsDefault = true
            });
            context.Add(new UserActivity
            {
                ActivityText = "asd",
                ActivityTime = DateTime.UtcNow,
                Id = 1,
                LocationId = 1,
                UserId = 1
            });
            context.Add(new UserActivity
            {
                ActivityText = "klj",
                ActivityTime = DateTime.UtcNow,
                Id = 2,
                LocationId = 1,
                UserId = 1
            });
            context.Add(new UserActivity
            {
                ActivityText = "asxd",
                ActivityTime = DateTime.UtcNow,
                Id = 3,
                LocationId = 1,
                UserId = 1
            });
            context.Add(new UserActivity
            {
                ActivityText = "ads",
                ActivityTime = DateTime.UtcNow,
                Id = 4,
                LocationId = 1,
                UserId = 1
            });
            context.SaveChanges();

            var service = GetAdminService(context);
            var response = await service.GetActivities(1, 2, 1, new QueryReqModel { 
            
                OrderBy = "userid",
                OrderDirection = "asc",
                Skips = 2,
                Takes = 2
            });


            var result = Assert.IsType<ServiceCollectionResult>(response.Result);
            var data = Assert.IsType<List<ActivityResult>>(result.Items);
            Assert.Equal(2, result.TotalItems);
            Assert.Equal(3, data[0].Id);
        }
    }
}
