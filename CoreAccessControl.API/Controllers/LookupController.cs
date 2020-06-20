using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CoreAccessControl.API.Helpers;
using CoreAccessControl.Domain.Models;
using CoreAccessControl.Domain.RequestModels;
using CoreAccessControl.Domain.ResponseModels;
using CoreAccessControl.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Supra.LittleLogger;

namespace CoreAccessControl.API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/ver{version:apiVersion}/lookups/states")]
    [ApiController]
    public class LookupController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public ActionResult GetStates([FromQuery][Required] string type)
        {
            Logger.WriteInformation("Geting lookup data.");
            switch (type)
            {
                case "Administrator":
                    {
                        return Ok(Enum.GetNames(typeof(AdministratorState)));
                    }
                case "Device":
                    {
                        return Ok(Enum.GetNames(typeof(DeviceState)));
                    }
                case "Keyholder":
                    {
                        return Ok(Enum.GetNames(typeof(KeyholderState)));
                    }
                case "Space":
                    {
                        return Ok(Enum.GetNames(typeof(SpaceState)));
                    }
                default:
                    {
                        return BadRequest(new string[] { });
                    }
            }
        }
    }
}