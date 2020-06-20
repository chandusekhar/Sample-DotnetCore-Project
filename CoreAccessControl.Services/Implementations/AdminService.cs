using CoreAccessControl.DataAccess.Ef.Data;
using CoreAccessControl.DataAccess.Ef.Models;
using CoreAccessControl.DataAccess.Ef.StoreProcs;
using CoreAccessControl.Domain.Configuration;
using CoreAccessControl.Domain.Models;
using CoreAccessControl.Domain.RequestModels;
using CoreAccessControl.Domain.ResponseModels;
using CoreAccessControl.Services.Common;
using CoreAccessControl.Services.Converters;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Supra.LittleLogger;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CoreAccessControl.Services
{
    public class AdminService : IAdminService
    {
        private readonly CoreaccesscontrolContext _context;
        private readonly AppSettings _appSettings;
        private readonly IEmailSender _emailSender;
        private readonly IEmailService _emailService;

        public AdminService(CoreaccesscontrolContext context, AppSettings appSettings, IEmailSender emailSender, IEmailService emailService)
        {
            _context = context;
            _appSettings = appSettings;
            _emailSender = emailSender;
            _emailService = emailService;
        }

        public async Task<ServiceResponseResult> CreateAdmin(long locationId, long userId, AdministratorReqModel model)
        {
            Logger.WriteInformation("Creating admin data.");
            var tempPwd = Guid.NewGuid().ToString();
            var user = UserMapper.ToUser(model, tempPwd, _appSettings.Secret);
            user = user.UpdateLastUpdatedBy(userId);
            _context.Add(user);
            await _context.SaveChangesAsync();

            var userLocation = new UserLocation
            {
                CreatedBy = userId,
                CreatedOn = DateTime.UtcNow,
                LocationId = locationId,
                LastUpdatedBy = userId,
                LastUpdatedOn = DateTime.UtcNow,
                UserId = user.Id,
                State = (int)AdministratorState.Invited
            };

            _context.Add(userLocation);
            await _context.SaveChangesAsync();
            var response = new AdministratorResult(user, userLocation);

            if (model.Permissions != null)
            {
                var userPermission = UserPermissionMapper.ToUserPermission(model.Permissions);
                userPermission = userPermission.UpdateLastUpdatedBy(userId)
                    .UpdateLastUpdatedOn(DateTime.UtcNow)
                    .UpdateUserLocationId(userLocation.Id);

                _context.Add(userPermission);
                await _context.SaveChangesAsync();

                response.AddPermission(userPermission);
            }


            var key = await _context.KeyHolder.FirstOrDefaultAsync(x => x.KeySerialNumber == model.KeySerialNumber);

            if (key != null)
            {
                var userKeyMapping = new UserKeyMapping
                {
                    AppliedOn = DateTime.UtcNow,
                    KeySerialNumber = key.KeySerialNumber,
                    LocationId = locationId,
                    UserId = user.Id
                };

                userLocation.UpdateIsToolKitEnabled(true);
                _context.Update(userLocation);

                _context.Add<UserKeyMapping>(userKeyMapping);
                await _context.SaveChangesAsync();

                response.AddToolkit(key);
            }

            var emailData = await _emailService.ConstructResetPassword(tempPwd);
            await _emailSender.SendMailViaSmtpClientAsync(new string[] { user.Email }, new string[] { }, new string[] { }, emailData);

            Logger.WriteInformation("Creating admin data completed.");
            return new ServiceResponseResult
            {
                Result = response,
                StatusCode = System.Net.HttpStatusCode.OK
            };
        }

        public async Task<ServiceResponseResult> DeleteAdmin(long locationId, long userId, long adminUserId)
        {
            Logger.WriteInformation("Deleting admin data.");
            var userLoc = await _context.UserLocation.FirstOrDefaultAsync(x => x.UserId == adminUserId && x.LocationId == locationId);
            if (userLoc == null)
            {
                return new ServiceResponseResult
                {
                    Result = new { Message = $"User does not exists in this location {locationId}" },
                    StatusCode = System.Net.HttpStatusCode.NotFound
                };
            }

            var user = await _context.User.FindAsync(adminUserId);           

            userLoc.State = (int)AdministratorState.Inactive;
            userLoc.LastUpdatedBy = userId;
            userLoc.LastUpdatedOn = DateTime.UtcNow;

            _context.Update(userLoc);
            await _context.SaveChangesAsync();

            Logger.WriteInformation("Deleting admin data completed.");
            return new ServiceResponseResult
            {
                StatusCode = System.Net.HttpStatusCode.OK
            };
        }

        public async Task<ServiceResponseResult> GetActivities(long locationId, long userId, long adminUserId, QueryReqModel query)
        {
            Logger.WriteInformation("Getiing admin activity data.");
            query.Takes = query.Takes > _appSettings.Limit ? _appSettings.Limit : query.Takes;
            query.OrderDirection = string.IsNullOrEmpty(query.OrderDirection) ? "asc" : query.OrderDirection.ToLower();
            query.OrderBy = string.IsNullOrEmpty(query.OrderBy) ? "" : query.OrderBy.ToLower();
            var userLoc = await _context.UserLocation.FirstOrDefaultAsync(x => x.UserId == adminUserId && x.LocationId == locationId);
            if (userLoc == null)
            {
                return new ServiceResponseResult
                {
                    Result = new { Message = $"User does not exists in this location {locationId}" },
                    StatusCode = System.Net.HttpStatusCode.NotFound
                };
            }

            var user = await _context.User.FindAsync(adminUserId);
            var activityQuery = _context.UserActivity.Where(x => x.UserId == adminUserId && x.LocationId == locationId);
            if (query.OrderDirection == "asc")
            {
                switch (query.OrderBy.ToLower())
                {
                    case "activitytext":
                        {
                            activityQuery = activityQuery.OrderBy(x => x.ActivityText);
                            break;
                        }
                    case "locationid":
                        {
                            activityQuery = activityQuery.OrderBy(x => x.LocationId);
                            break;
                        }
                    case "userid":
                        {
                            activityQuery = activityQuery.OrderBy(x => x.UserId);
                            break;
                        }
                    case "activitytime":
                        {
                            activityQuery = activityQuery.OrderBy(x => x.ActivityTime);
                            break;
                        }
                    default:
                        {
                            activityQuery = activityQuery.OrderByDescending(x => x.ActivityTime);
                            break;
                        }
                }
            }
            else
            {
                switch (query.OrderBy.ToLower())
                {
                    case "activitytext":
                        {
                            activityQuery = activityQuery.OrderByDescending(x => x.ActivityText);
                            break;
                        }
                    case "locationid":
                        {
                            activityQuery = activityQuery.OrderByDescending(x => x.LocationId);
                            break;
                        }
                    case "userid":
                        {
                            activityQuery = activityQuery.OrderByDescending(x => x.UserId);
                            break;
                        }
                    case "activitytime":
                        {
                            activityQuery = activityQuery.OrderByDescending(x => x.ActivityTime);
                            break;
                        }
                    default:
                        {
                            activityQuery = activityQuery.OrderByDescending(x => x.ActivityTime);
                            break;
                        }
                }
            }

            activityQuery = activityQuery.Skip(query.Skips).Take(query.Takes);            

            var result = await activityQuery.Select(x => new ActivityResult
            {
                ActivityText = x.ActivityText,
                ActivityTime = x.ActivityTime,
                Id = x.Id
            }).ToListAsync();

            Logger.WriteInformation("Getiing admin activity completed.");
            return new ServiceResponseResult
            {
                Result = new ServiceCollectionResult
                {
                    TotalItems = result.Count,
                    Items = result
                },
                StatusCode = System.Net.HttpStatusCode.OK
            };
        }

        public async Task<ServiceResponseResult> SearchAdmin(long locationId, long userId, AdminSearchReqModel model)
        {
            Logger.WriteInformation("Searching admin data.");
            model.Takes = model.Takes == 0 || model.Takes > _appSettings.Limit ? _appSettings.Limit : model.Takes;
            model.OrderBy = string.IsNullOrEmpty(model.OrderBy) ? "id" : model.OrderBy;
            model.OrderDirection = string.IsNullOrEmpty(model.OrderDirection) ? "desc" : model.OrderDirection;
            model.Name = string.IsNullOrEmpty(model.Name) ? null : model.Name;
            model.Id = string.IsNullOrEmpty(model.Id) ? null : model.Id;
            model.Email = string.IsNullOrEmpty(model.Email) ? null : model.Email;

            var param = new SqlParameter[] { new SqlParameter("id", (object)model.Id ?? DBNull.Value){ SqlDbType = SqlDbType.BigInt, Direction = ParameterDirection.Input, IsNullable = true,  },
                                                new SqlParameter("onlyToolKitUser", (object)model.OnlyToolKitUser ?? DBNull.Value){ SqlDbType = SqlDbType.Bit, Direction = ParameterDirection.Input, IsNullable = true },
                                                new SqlParameter("name", (object)model.Name ?? DBNull.Value){ SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, IsNullable = true },
                                                new SqlParameter("email", (object)model.Email ?? DBNull.Value){ SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, IsNullable = true },
                                                new SqlParameter("state",(object)model.State ?? DBNull.Value){ SqlDbType = SqlDbType.BigInt, Direction = ParameterDirection.Input, IsNullable = true },
                                                new SqlParameter("statusid", (object)model.StatusId ?? DBNull.Value){ SqlDbType = SqlDbType.BigInt, Direction = ParameterDirection.Input, IsNullable = true },
                                                new SqlParameter("skip", model.Skips){ SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input, IsNullable = true },
                                                new SqlParameter("take", model.Takes){ SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input, IsNullable = true },
                                                new SqlParameter("orderby", model.OrderBy){ SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, IsNullable = true },
                                                new SqlParameter("orderdir", model.OrderDirection){ SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, IsNullable = true },
            };

            var sql = @"EXEC [dbo].[AdminSearch]";
            var data = await _context.Set<Administrator>().FromSqlRaw(sql, param).ToListAsync();

            var response = new AdministratorSearchResult
            {
                Items = data.Select(x =>
                {
                    var data = new AdministratorResult
                    {
                        DisabledReason = x.DisabledReason,
                        Email = x.Email,
                        Id = x.UserId,
                        Name = x.Name,
                        Permissions = new UserPermissionResult
                        {
                            HasAdminEdit = x.HasAdminEdit.HasValue && x.HasAdminEdit.Value,
                            HasAdminRead = x.HasAdminRead.HasValue && x.HasAdminRead.Value,
                            HasConfigEdit = x.HasConfigEdit.HasValue && x.HasConfigEdit.Value,
                            HasConfigRead = x.HasConfigRead.HasValue && x.HasConfigRead.Value,
                            HasDeviceEdit = x.HasDeviceEdit.HasValue && x.HasDeviceEdit.Value,
                            HasDeviceRead = x.HasDeviceRead.HasValue && x.HasDeviceRead.Value,
                            HasKeyholderEdit = x.HasKeyholderEdit.HasValue && x.HasKeyholderEdit.Value,
                            HasKeyholderRead = x.HasKeyholderRead.HasValue && x.HasKeyholderRead.Value,
                            HasSpaceEdit = x.HasSpaceEdit.HasValue && x.HasSpaceEdit.Value,
                            HasSpaceRead = x.HasSpaceRead.HasValue && x.HasSpaceRead.Value
                        },                        
                    };
                    if(x.State.HasValue)
                    {
                        data.State = Enum.Parse<AdministratorState>(x.State.Value.ToString()); 
                    }

                    if(x.UserActivityId.HasValue)
                    {
                        data.RecentActivity = new ActivityResult
                        {
                            ActivityText = x.ActivityText,
                            ActivityTime = x.ActivityTime,
                            Id = x.UserActivityId.Value
                        };
                    }
                    if(x.StatusId.HasValue)
                    {
                        data.Status = new LookupEntityResult
                        {
                            Id = x.StatusId.Value,
                            Name = x.StatusName
                        };
                    }
                    if(x.KeyHolderId.HasValue)
                    {
                        data.ToolkitInfo = new ToolkitInfoRespModel
                        {
                            Id = x.KeyHolderId.Value,
                            KeySerialNumber = x.KeySerialNumber,
                            Pin = x.Pin
                        };
                    }
                    return data;
                }).ToList(),
                TotalItems = data.Count()
            };

            Logger.WriteInformation("Searching admin data.");
            return new ServiceResponseResult
            {
                Result = response,
                StatusCode = System.Net.HttpStatusCode.OK
            };
        }

        public async Task<ServiceResponseResult> UpdatedAdmin(long locationId, long userId, AdministratorReqModel model)
        {
            Logger.WriteInformation("Updating admin data.");
            var user = await _context.User.FirstOrDefaultAsync(x => x.Email == model.Email);
            var userLoc = await _context.UserLocation.FirstOrDefaultAsync(x => x.UserId == user.Id && x.LocationId == locationId);
            if (userLoc == null)
            {
                return new ServiceResponseResult
                {
                    Result = new { Message = $"User does not exists in this location {locationId}" },
                    StatusCode = System.Net.HttpStatusCode.NotFound
                };
            }

            user = user.UpdateName(model.Name)
                .UpdateLastUpdatedBy(userId)
                .UpdateLastUpdatedOn(DateTime.UtcNow);

            _context.Update<User>(user);

            var userPermission = await _context.UserPermission.FirstOrDefaultAsync(x => x.UserLocation.LocationId == locationId && x.UserLocation.UserId == user.Id);
            if (userPermission != null)
            {
                return new ServiceResponseResult
                {
                    Result = new { Message = "User permission for location already added" },
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };
            }

            await _context.SaveChangesAsync();

            var response = new AdministratorResult(user, userLoc);

            if (model.Permissions != null)
            {
                userPermission = UserPermissionMapper.ToUserPermission(model.Permissions);
                userPermission = userPermission.UpdateLastUpdatedBy(userId)
                    .UpdateLastUpdatedOn(DateTime.UtcNow)
                    .UpdateUserLocationId(userLoc.Id);

                _context.Add<UserPermission>(userPermission);
                await _context.SaveChangesAsync();
                response.AddPermission(userPermission);
            }


            var key = await _context.KeyHolder.FirstOrDefaultAsync(x => x.KeySerialNumber == model.KeySerialNumber);

            if (key != null)
            {
                var userKeyMapping = new UserKeyMapping
                {
                    AppliedOn = DateTime.UtcNow,
                    KeySerialNumber = key.KeySerialNumber,
                    LocationId = locationId,
                    UserId = user.Id
                };

                userLoc.UpdateIsToolKitEnabled(true);
                _context.Update(userLoc);

                _context.Add<UserKeyMapping>(userKeyMapping);
                await _context.SaveChangesAsync();

                response.AddToolkit(key);
            }
            Logger.WriteInformation("Updating admin activity data completed.");
            return new ServiceResponseResult
            {
                Result = response,
                StatusCode = System.Net.HttpStatusCode.OK
            };
        }

        public async Task<ServiceResponseResult> UpdatedAdmin(long locationId, long userId, long adminUserId, AdministratorUpdateReqModel model)
        {
            Logger.WriteInformation("Getiing admin activity data.");
            var userLoc = await _context.UserLocation.FirstOrDefaultAsync(x => x.UserId == adminUserId && x.LocationId == locationId);
            if (userLoc == null)
            {
                return new ServiceResponseResult
                {
                    Result = new { Message = $"User does not exists in this location {locationId}" },
                    StatusCode = System.Net.HttpStatusCode.NotFound
                };
            }

            var user = await _context.User.FindAsync(adminUserId);

            if (!string.IsNullOrEmpty(model.Email) && model.Email != user.Email)
            {
                var doesEmailExists = await _context.ChangeEmailRequest.AnyAsync(x => x.Email == model.Email.ToLower()) || await _context.User.AnyAsync(x => x.Email == model.Email.ToLower());
                if(doesEmailExists)
                {
                    return new ServiceResponseResult
                    {
                        Result = new { Message = $"Could not update the email as it is already exists" },
                        StatusCode = System.Net.HttpStatusCode.BadRequest
                    };
                }


                var emailChange = new ChangeEmailRequest
                {
                    Email = model.Email,
                    RequestedOn = DateTime.UtcNow,
                    UserId = adminUserId,
                    VerificationToken = Guid.NewGuid().ToString(),
                    VerificationTokenExpiry = DateTime.UtcNow.AddHours(24)
                };
                _context.ChangeEmailRequest.Add(emailChange);

                var emailData = await _emailService.ConstructEmailVerification(emailChange.VerificationToken);
                await _emailSender.SendMailViaSmtpClientAsync(new string[] { emailChange.Email }, new string[] { }, new string[] { }, emailData);
            }

            if (!string.IsNullOrEmpty(model.Name))
            {
                user = user.UpdateName(model.Name).UpdateLastUpdatedBy(userId)
                .UpdateLastUpdatedOn(DateTime.UtcNow);
            }

            _context.Update<User>(user);

            if (model.State.HasValue)
            {
                if (model.State.Value == AdministratorState.Disabled && string.IsNullOrEmpty(model.DisabledReason))
                {
                    return new ServiceResponseResult
                    {
                        Result = new { Message = $"DisabledReason is required for disable state." },
                        StatusCode = System.Net.HttpStatusCode.BadRequest
                    };
                }

                userLoc.State = (int)model.State.Value;
                userLoc.LastUpdatedBy = userId;
                userLoc.LastUpdatedOn = DateTime.UtcNow;
                _context.Update(userLoc);
            }

            var response = new AdministratorResult(user, userLoc);
            if (model.Status != null)
            {
                var status = await _context.UserStatus.FirstOrDefaultAsync(x => x.Id == model.Status.Id && x.LocationId == locationId);
                if (status != null)
                {
                    userLoc.StatusId = status.Id;
                    userLoc.LastUpdatedBy = userId;
                    userLoc.LastUpdatedOn = DateTime.UtcNow;
                    _context.Update(userLoc);
                    response.AddStatus(status);
                }
            }

            var userPermission = await _context.UserPermission.FirstOrDefaultAsync(x => x.UserLocationId == userLoc.Id);
            if (userPermission == null)
            {
                userPermission = UserPermissionMapper.ToUserPermission(model.Permissions)
                                                        .UpdateLastUpdatedOn(DateTime.UtcNow)
                                                        .UpdateUserLocationId(userLoc.Id);
                _context.Add<UserPermission>(userPermission);
            }
            else
            {
                userPermission = UserPermissionMapper.ToUserPermission(model.Permissions);
                userPermission = userPermission.UpdateLastUpdatedBy(userId)
                    .UpdateId(userPermission.Id)
                    .UpdateLastUpdatedOn(DateTime.UtcNow)
                    .UpdateUserLocationId(userLoc.Id);

                _context.Update<UserPermission>(userPermission);
            }

            response.AddPermission(userPermission);

            var key = await _context.KeyHolder.FirstOrDefaultAsync(x => x.KeySerialNumber == model.KeySerialNumber);
            var userKeyMapping = await _context.UserKeyMapping
                .FirstOrDefaultAsync(x => x.UserId == adminUserId && x.LocationId == locationId && x.KeySerialNumber == model.KeySerialNumber);

            if (key != null)
            {
                if (userKeyMapping == null)
                {
                    userKeyMapping = new UserKeyMapping
                    {
                        AppliedOn = DateTime.UtcNow,
                        KeySerialNumber = key.KeySerialNumber,
                        LocationId = locationId,
                        UserId = user.Id
                    };

                    userLoc.UpdateIsToolKitEnabled(true);
                    _context.Update(userLoc);

                    _context.Add<UserKeyMapping>(userKeyMapping);
                }
                response.AddToolkit(key);
            }
            await _context.SaveChangesAsync();

            Logger.WriteInformation("Updating admin activity data completed.");

            return new ServiceResponseResult
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Result = response
            };
        }
    }
}
