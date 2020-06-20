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

namespace CoreAccessControl.API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/ver{version:apiVersion}/location/{locationId}/accounts")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly AuthHelpers _authHelpers;

        public AccountController(IAccountService accountService, AuthHelpers authHelpers)
        {
            _accountService = accountService;
            _authHelpers = authHelpers;
        }

        [HttpGet]
        [Route("profile")]
        public async Task<ActionResult> GetProfile([FromRoute] long locationId)
        {
            var res = await _accountService.GetAdminProfile(locationId, _authHelpers.GetCurrentUserId().Value);
            return StatusCode(res.GetStatusCode(), res.Result);
        }

        [HttpPatch]
        [Route("profile")]
        public async Task<ActionResult> UpdateProfile([FromRoute] long locationId, [FromBody] AdminProfileUpdateReqModel model)
        {
            var res = await _accountService.UpdateAdminProfile(model, _authHelpers.GetCurrentUserId().Value, locationId);
            return StatusCode(res.GetStatusCode(), res.Result);
        }
    }
}