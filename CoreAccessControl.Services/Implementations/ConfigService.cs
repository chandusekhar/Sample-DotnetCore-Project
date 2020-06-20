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
    public class ConfigService : IConfigService
    {
        private readonly CoreaccesscontrolContext _context;
        private readonly AppSettings _appSettings;
        private readonly IEmailSender _emailSender;
        private readonly IEmailService _emailService;

        public ConfigService(CoreaccesscontrolContext context, AppSettings appSettings, IEmailSender emailSender, IEmailService emailService)
        {
            _context = context;
            _appSettings = appSettings;
            _emailSender = emailSender;
            _emailService = emailService;
        }

        public async Task<ServiceResponseResult> GetStatuses(long locationId, string type, bool isFull)
        {
            Logger.WriteInformation("Getting status.");
            switch (type.ToLower())
            {
                default:
                    {
                        return new ServiceResponseResult
                        {
                            Result = new { Message = "Provide a valid type: Administrator, Device, Keyholder, Space" },
                            StatusCode = System.Net.HttpStatusCode.BadRequest
                        };
                    }
                case "administrator":
                    {
                        return await GetAdminStatus(locationId, isFull);
                    }
                case "device":
                    {
                        return await GetDeviceStatus(locationId, isFull);
                    }
                case "keyholder":
                    {
                        return await GetKeyHolderStatus(locationId, isFull);
                    }
                case "space":
                    {
                        return await GetSpaceStatus(locationId, isFull);
                    }
            }
        }

        public async Task<ServiceResponseResult> SaveStatuse(long locationId, long userId, string type, ConfigStatusReqModel model)
        {
            Logger.WriteInformation("Saving status.");
            switch (type.ToLower())
            {
                default:
                    {
                        return new ServiceResponseResult
                        {
                            Result = new { Message = "Provide a valid type: Administrator, Device, Keyholder, Space" },
                            StatusCode = System.Net.HttpStatusCode.BadRequest
                        };
                    }
                case "administrator":
                    {
                        return await SaveAdminStatus(locationId, userId, model);
                    }
                case "device":
                    {
                        return await SaveDeviceStatus(locationId, userId, model);
                    }
                case "keyholder":
                    {
                        return await SaveKeyHolderStatus(locationId, userId, model);
                    }
                case "space":
                    {
                        return await SaveSpaceStatus(locationId, userId, model);
                    }
            }
        }

        public async Task<ServiceResponseResult> UpdateStatuse(long locationId, long userId, long id, string type, ConfigStatusReqModel model)
        {
            switch (type.ToLower())
            {
                default:
                    {
                        return new ServiceResponseResult
                        {
                            Result = new { Message = "Provide a valid type: Administrator, Device, Keyholder, Space" },
                            StatusCode = System.Net.HttpStatusCode.BadRequest
                        };
                    }
                case "administrator":
                    {
                        return await UpdateAdminStatus(locationId, id, userId, model);
                    }
                case "device":
                    {
                        return await UpdateDeviceStatus(locationId, id, userId, model);
                    }
                case "keyholder":
                    {
                        return await UpdateKeyHolderStatus(locationId, id, userId, model);
                    }
                case "space":
                    {
                        return await UpdateSpaceStatus(locationId, id, userId, model);
                    }
            }
        }

        public async Task<ServiceResponseResult> DeleteStatuse(long locationId, long userId, long id, string type)
        {
            switch (type.ToLower())
            {
                default:
                    {
                        return new ServiceResponseResult
                        {
                            Result = new { Message = "Provide a valid type: Administrator, Device, Keyholder, Space" },
                            StatusCode = System.Net.HttpStatusCode.BadRequest
                        };
                    }
                case "administrator":
                    {
                        var status = await _context.UserStatus.FindAsync(id);
                        if (status == null)
                        {
                            return new ServiceResponseResult
                            {
                                Result = new { Message = "User status not found" },
                                StatusCode = System.Net.HttpStatusCode.NotFound
                            };
                        }

                        status.IsActive = false;
                        status.LastUpdatedBy = userId;
                        status.LastUpdatedOn = DateTime.UtcNow;

                        _context.UserStatus.Update(status);
                        await _context.SaveChangesAsync();

                        return new ServiceResponseResult
                        {
                            StatusCode = System.Net.HttpStatusCode.OK
                        };
                    }
                case "device":
                    {
                        var status = await _context.Devicestatus.FindAsync(id);
                        if (status == null)
                        {
                            return new ServiceResponseResult
                            {
                                Result = new { Message = "Device status not found" },
                                StatusCode = System.Net.HttpStatusCode.NotFound
                            };
                        }

                        status.IsActive = false;
                        status.LastUpdatedBy = userId;
                        status.LastUpdatedOn = DateTime.UtcNow;

                        _context.Devicestatus.Update(status);
                        await _context.SaveChangesAsync();

                        return new ServiceResponseResult
                        {
                            StatusCode = System.Net.HttpStatusCode.OK
                        };
                    }
                case "keyholder":
                    {
                        var status = await _context.KeyholderStatus.FindAsync(id);
                        if (status == null)
                        {
                            return new ServiceResponseResult
                            {
                                Result = new { Message = "Keyholder status not found" },
                                StatusCode = System.Net.HttpStatusCode.NotFound
                            };
                        }

                        status.IsActive = false;
                        status.LastUpdatedBy = userId;
                        status.LastUpdatedOn = DateTime.UtcNow;

                        _context.KeyholderStatus.Update(status);
                        await _context.SaveChangesAsync();

                        return new ServiceResponseResult
                        {
                            StatusCode = System.Net.HttpStatusCode.OK
                        };
                    }
                case "space":
                    {
                        var status = await _context.SpaceStatus.FindAsync(id);
                        if (status == null)
                        {
                            return new ServiceResponseResult
                            {
                                Result = new { Message = "Space status not found" },
                                StatusCode = System.Net.HttpStatusCode.NotFound
                            };
                        }

                        status.IsActive = false;
                        status.LastUpdatedBy = userId;
                        status.LastUpdatedOn = DateTime.UtcNow;

                        _context.SpaceStatus.Update(status);
                        await _context.SaveChangesAsync();

                        return new ServiceResponseResult
                        {
                            StatusCode = System.Net.HttpStatusCode.OK
                        };
                    }
            }
        }

        private async Task<ServiceResponseResult> UpdateDeviceStatus(long locationId, long id, long userId, ConfigStatusReqModel model)
        {
            var status = await _context.Devicestatus.FindAsync(id);
            if (status == null)
            {
                return new ServiceResponseResult
                {
                    Result = new { Message = "Device status not found" },
                    StatusCode = System.Net.HttpStatusCode.NotFound
                };
            }

            status.Name = model.Name;
            status.Description = model.Description;
            status.IsDefault = model.IsDefault;
            status.IsActive = true;
            status.LastUpdatedBy = userId;
            status.LastUpdatedOn = DateTime.UtcNow;

            if (model.IsDefault)
            {
                await _context.Set<Devicestatus>().FromSqlRaw($"Update dbo.Devicestatus SET IsDefault = 0 OUTPUT INSERTED.* WHERE LocationId = {locationId} and id != {id}").ToListAsync();
            }

            _context.Devicestatus.Update(status);
            await _context.SaveChangesAsync();

            return new ServiceResponseResult
            {
                Result = status == null ? null : new StatusDetailsResponseModel
                {
                    Id = status.Id,
                    Name = status.Name,
                    Description = status.Description,
                    IsActive = status.IsActive,
                    IsDefault = status.IsDefault
                },
                StatusCode = System.Net.HttpStatusCode.OK
            };
        }

        private async Task<ServiceResponseResult> UpdateKeyHolderStatus(long locationId, long id, long userId, ConfigStatusReqModel model)
        {
            var status = await _context.KeyholderStatus.FindAsync(id);
            if (status == null)
            {
                return new ServiceResponseResult
                {
                    Result = new { Message = "Keyholder status not found" },
                    StatusCode = System.Net.HttpStatusCode.NotFound
                };
            }

            status.Name = model.Name;
            status.Description = model.Description;
            status.IsDefault = model.IsDefault;
            status.IsActive = true;
            status.LastUpdatedBy = userId;
            status.LastUpdatedOn = DateTime.UtcNow;

            if (model.IsDefault)
            {
                await _context.Set<KeyholderStatus>().FromSqlRaw($"Update dbo.KeyholderStatus SET IsDefault = 0 OUTPUT INSERTED.* WHERE LocationId = {locationId} and id != {id}").ToListAsync();
            }

            _context.KeyholderStatus.Update(status);
            await _context.SaveChangesAsync();

            return new ServiceResponseResult
            {
                Result = status == null ? null : new StatusDetailsResponseModel
                {
                    Id = status.Id,
                    Name = status.Name,
                    Description = status.Description,
                    IsActive = status.IsActive,
                    IsDefault = status.IsDefault
                },
                StatusCode = System.Net.HttpStatusCode.OK
            };
        }

        private async Task<ServiceResponseResult> UpdateSpaceStatus(long locationId, long id, long userId, ConfigStatusReqModel model)
        {
            var status = await _context.SpaceStatus.FindAsync(id);
            if (status == null)
            {
                return new ServiceResponseResult
                {
                    Result = new { Message = "Keyholder status not found" },
                    StatusCode = System.Net.HttpStatusCode.NotFound
                };
            }

            status.Name = model.Name;
            status.Description = model.Description;
            status.IsDefault = model.IsDefault;
            status.IsActive = true;
            status.LastUpdatedBy = userId;
            status.LastUpdatedOn = DateTime.UtcNow;

            if (model.IsDefault)
            {
                await _context.Set<SpaceStatus>().FromSqlRaw($"Update dbo.SpaceStatus SET IsDefault = 0 OUTPUT INSERTED.* WHERE LocationId = {locationId} and id != {id}").ToListAsync();
            }

            _context.SpaceStatus.Update(status);
            await _context.SaveChangesAsync();

            return new ServiceResponseResult
            {
                Result = status == null ? null : new StatusDetailsResponseModel
                {
                    Id = status.Id,
                    Name = status.Name,
                    Description = status.Description,
                    IsActive = status.IsActive,
                    IsDefault = status.IsDefault
                },
                StatusCode = System.Net.HttpStatusCode.OK
            };
        }

        private async Task<ServiceResponseResult> UpdateAdminStatus(long locationId, long id, long userId, ConfigStatusReqModel model)
        {
            var status = await _context.UserStatus.FindAsync(id);
            if (status == null)
            {
                return new ServiceResponseResult
                {
                    Result = new { Message = "Keyholder status not found" },
                    StatusCode = System.Net.HttpStatusCode.NotFound
                };
            }

            status.Name = model.Name;
            status.Description = model.Description;
            status.IsDefault = model.IsDefault;
            status.IsActive = true;
            status.LastUpdatedBy = userId;
            status.LastUpdatedOn = DateTime.UtcNow;

            if (model.IsDefault)
            {
                await _context.Set<UserStatus>().FromSqlRaw($"Update dbo.UserStatus SET IsDefault = 0 OUTPUT INSERTED.* WHERE LocationId = {locationId} and id != {id}").ToListAsync();
            }

            _context.UserStatus.Update(status);
            await _context.SaveChangesAsync();

            return new ServiceResponseResult
            {
                Result = status == null ? null : new StatusDetailsResponseModel
                {
                    Id = status.Id,
                    Name = status.Name,
                    Description = status.Description,
                    IsActive = status.IsActive,
                    IsDefault = status.IsDefault
                },
                StatusCode = System.Net.HttpStatusCode.OK
            };
        }

        private async Task<ServiceResponseResult> GetAdminStatus(long locationId, bool isFull)
        {
            var status = await _context.UserStatus.Where(x => x.LocationId == locationId && x.IsActive).ToListAsync();

            if (isFull)
            {
                return new ServiceResponseResult
                {
                    Result = status.Select(x => new StatusDetailsResponseModel
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description,
                        IsActive = x.IsActive,
                        IsDefault = x.IsDefault
                    }),
                    StatusCode = System.Net.HttpStatusCode.OK
                };
            }
            else
            {
                return new ServiceResponseResult
                {
                    Result = status.Select(x => new StatusResponseModel
                    {
                        Id = x.Id,
                        Name = x.Name
                    }),
                    StatusCode = System.Net.HttpStatusCode.OK
                };
            }
        }
        private async Task<ServiceResponseResult> GetDeviceStatus(long locationId, bool isFull)
        {
            var status = await _context.Devicestatus.Where(x => x.LocationId == locationId && x.IsActive).ToListAsync();

            if (isFull)
            {
                return new ServiceResponseResult
                {
                    Result = status.Select(x => new StatusDetailsResponseModel
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description,
                        IsActive = x.IsActive,
                        IsDefault = x.IsDefault
                    }),
                    StatusCode = System.Net.HttpStatusCode.OK
                };
            }
            else
            {
                return new ServiceResponseResult
                {
                    Result = status.Select(x => new StatusResponseModel
                    {
                        Id = x.Id,
                        Name = x.Name
                    }),
                    StatusCode = System.Net.HttpStatusCode.OK
                };
            }
        }
        private async Task<ServiceResponseResult> GetKeyHolderStatus(long locationId, bool isFull)
        {
            var status = await _context.KeyholderStatus.Where(x => x.LocationId == locationId && x.IsActive).ToListAsync();

            if (isFull)
            {
                return new ServiceResponseResult
                {
                    Result = status.Select(x => new StatusDetailsResponseModel
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description,
                        IsActive = x.IsActive,
                        IsDefault = x.IsDefault
                    }),
                    StatusCode = System.Net.HttpStatusCode.OK
                };
            }
            else
            {
                return new ServiceResponseResult
                {
                    Result = status.Select(x => new StatusResponseModel
                    {
                        Id = x.Id,
                        Name = x.Name
                    }),
                    StatusCode = System.Net.HttpStatusCode.OK
                };
            }
        }
        private async Task<ServiceResponseResult> GetSpaceStatus(long locationId, bool isFull)
        {
            var status = await _context.SpaceStatus.Where(x => x.LocationId == locationId && x.IsActive).ToListAsync();

            if (isFull)
            {
                return new ServiceResponseResult
                {
                    Result = status.Select(x => new StatusDetailsResponseModel
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description,
                        IsActive = x.IsActive,
                        IsDefault = x.IsDefault
                    }),
                    StatusCode = System.Net.HttpStatusCode.OK
                };
            }
            else
            {
                return new ServiceResponseResult
                {
                    Result = status.Select(x => new StatusResponseModel
                    {
                        Id = x.Id,
                        Name = x.Name
                    }),
                    StatusCode = System.Net.HttpStatusCode.OK
                };
            }
        }

        private async Task<ServiceResponseResult> SaveAdminStatus(long locationId, long userId, ConfigStatusReqModel model)
        {
            var status = new UserStatus
            {
                CreatedBy = userId,
                CreatedOn = DateTime.UtcNow,
                Description = model.Description,
                IsActive = true,
                IsDefault = model.IsDefault,
                LastUpdatedBy = userId,
                LastUpdatedOn = DateTime.UtcNow,
                LocationId = locationId,
                Name = model.Name
            };

            if (model.IsDefault)
            {
                await _context.Set<UserStatus>().FromSqlRaw($"Update dbo.UserStatus SET IsDefault = 0 OUTPUT INSERTED.* WHERE LocationId = {locationId}").ToListAsync();
            }

            _context.UserStatus.Add(status);
            await _context.SaveChangesAsync();

            return new ServiceResponseResult
            {
                Result = status == null ? null : new StatusDetailsResponseModel
                {
                    Id = status.Id,
                    Name = status.Name,
                    Description = status.Description,
                    IsActive = status.IsActive,
                    IsDefault = status.IsDefault
                },
                StatusCode = System.Net.HttpStatusCode.OK
            };
        }
        private async Task<ServiceResponseResult> SaveDeviceStatus(long locationId, long userId, ConfigStatusReqModel model)
        {
            var status = new Devicestatus
            {
                CreatedBy = userId,
                CreatedOn = DateTime.UtcNow,
                Description = model.Description,
                IsActive = true,
                IsDefault = model.IsDefault,
                LastUpdatedBy = userId,
                LastUpdatedOn = DateTime.UtcNow,
                LocationId = locationId,
                Name = model.Name
            };

            if (model.IsDefault)
            {
                await _context.Set<Devicestatus>().FromSqlRaw($"Update dbo.Devicestatus SET IsDefault = 0 OUTPUT INSERTED.* WHERE LocationId = {locationId}").ToListAsync();
                await _context.SaveChangesAsync();
            }

            _context.Devicestatus.Add(status);
            await _context.SaveChangesAsync();

            return new ServiceResponseResult
            {
                Result = status == null ? null : new StatusDetailsResponseModel
                {
                    Id = status.Id,
                    Name = status.Name,
                    Description = status.Description,
                    IsActive = status.IsActive,
                    IsDefault = status.IsDefault
                },
                StatusCode = System.Net.HttpStatusCode.OK
            };
        }
        private async Task<ServiceResponseResult> SaveKeyHolderStatus(long locationId, long userId, ConfigStatusReqModel model)
        {
            var status = new KeyholderStatus
            {
                CreatedBy = userId,
                CreatedOn = DateTime.UtcNow,
                Description = model.Description,
                IsActive = true,
                IsDefault = model.IsDefault,
                LastUpdatedBy = userId,
                LastUpdatedOn = DateTime.UtcNow,
                LocationId = locationId,
                Name = model.Name
            };

            if (model.IsDefault)
            {
                await _context.Set<KeyholderStatus>().FromSqlRaw($"Update dbo.KeyholderStatus SET IsDefault = 0 OUTPUT INSERTED.* WHERE LocationId = {locationId}").ToListAsync();
            }

            _context.KeyholderStatus.Add(status);
            await _context.SaveChangesAsync();

            return new ServiceResponseResult
            {
                Result = status == null ? null : new StatusDetailsResponseModel
                {
                    Id = status.Id,
                    Name = status.Name,
                    Description = status.Description,
                    IsActive = status.IsActive,
                    IsDefault = status.IsDefault
                },
                StatusCode = System.Net.HttpStatusCode.OK
            };
        }
        private async Task<ServiceResponseResult> SaveSpaceStatus(long locationId, long userId, ConfigStatusReqModel model)
        {
            var status = new SpaceStatus
            {
                CreatedBy = userId,
                CreatedOn = DateTime.UtcNow,
                Description = model.Description,
                IsActive = true,
                IsDefault = model.IsDefault,
                LastUpdatedBy = userId,
                LastUpdatedOn = DateTime.UtcNow,
                LocationId = locationId,
                Name = model.Name
            };

            if (model.IsDefault)
            {
                await _context.Set<SpaceStatus>().FromSqlRaw($"Update dbo.SpaceStatus SET IsDefault = 0 WHERE OUTPUT INSERTED.* LocationId = {locationId}").ToListAsync();
            }

            _context.SpaceStatus.Add(status);
            await _context.SaveChangesAsync();

            return new ServiceResponseResult
            {
                Result = status == null ? null : new StatusDetailsResponseModel
                {
                    Id = status.Id,
                    Name = status.Name,
                    Description = status.Description,
                    IsActive = status.IsActive,
                    IsDefault = status.IsDefault
                },
                StatusCode = System.Net.HttpStatusCode.OK
            };
        }
    }
}
