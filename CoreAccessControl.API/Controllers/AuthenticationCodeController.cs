using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CoreAccessControl.API.Attributes;
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
    [Route("api/ver{version:apiVersion}/location/{locationId}/authCode")]
    [ApiController]
    [Authorize]
    [CoreAccessAuthorize(PermissionDomain.Admin, PermissionAction.Write)]
    public class AuthenticationCodeController : ControllerBase
    {
        private readonly IAuthenticationCodeService _authenticationCodeService;
        private readonly AuthHelpers _authHelpers;

        public AuthenticationCodeController(IAuthenticationCodeService authenticationCodeService, AuthHelpers authHelpers)
        {
            _authenticationCodeService = authenticationCodeService;
            _authHelpers = authHelpers;
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult> Get([FromRoute][Required] long locationId, [FromQuery]string keySerialNumber, [FromQuery]string deviceSerialNumber)
        {
            var res = await _authenticationCodeService.GetCode(locationId, _authHelpers.GetCurrentUserId().Value, keySerialNumber, deviceSerialNumber);
            return StatusCode(res.GetStatusCode(), res.Result);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromRoute] long locationId, [FromBody][Required] AuthCodeCreateReqModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorModel { Message = string.Join(",", ModelState.Values.SelectMany(v => v.Errors.Select(y => y.ErrorMessage)))});
            }

            var res = await _authenticationCodeService.SendCode(model, _authHelpers.GetCurrentUserId().Value, locationId);
            return StatusCode(res.GetStatusCode(), res.Result);
        }
    }
}