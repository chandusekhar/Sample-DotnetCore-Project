using CoreAccessControl.DataAccess.Ef.Data;
using CoreAccessControl.Domain.Configuration;
using CoreAccessControl.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace CoreAccessControl.Services.Test
{
    public class ServiceTestBase
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

        public AppSettings GetAppSettings()
        {
            var path = Path.GetFullPath("..\\..\\..\\appsettings.json");
            return JsonConvert.DeserializeObject<AppSettings>(File.ReadAllText(path));
        }
    }
}
