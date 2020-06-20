using CoreAccessControl.DataAccess.Ef.Data;
using CoreAccessControl.DataAccess.Ef.Models;
using CoreAccessControl.Domain.Models;
using CoreAccessControl.Domain.RequestModels;
using CoreAccessControl.Domain.ResponseModels;
using CoreAccessControl.Services.Common;
using CoreAccessControl.Services.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Supra.LittleLogger;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CoreAccessControl.Services
{
    public class LocationService : ILocationService
    {
        private readonly CoreaccesscontrolContext _context;

        public LocationService(CoreaccesscontrolContext context)
        {
            _context = context;
        }

        public async Task<IList<LocationResponseModel>> GetByUser(long userId)
        {
            Logger.WriteInformation("Get location.");
            return await _context.UserLocation.Include(x => x.Location).Where(x => x.UserId == userId).Select(x => new LocationResponseModel
            {
                Id = x.Location.Id,
                Name = x.Location.Name
            }).ToListAsync();
        }
    }
}
