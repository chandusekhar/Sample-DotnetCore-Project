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
using OfficeOpenXml;

namespace CoreAccessControl.API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/ver{version:apiVersion}/location/{locationId}/keyholder")]
    [ApiController]
    public class KeyholderController : ControllerBase
    {
        private readonly IKeyholderService _keyHolderService;
        private readonly AuthHelpers _authHelpers;

        public KeyholderController(IKeyholderService keyholderService, AuthHelpers authHelpers)
        {
            _keyHolderService = keyholderService;
            _authHelpers = authHelpers;
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult> Get([FromRoute] long locationId, [FromQuery] KeyholderSearchReqModel model)
        {
            var res = await _keyHolderService.SearchKeyholder(locationId, _authHelpers.GetCurrentUserId().Value, model);
            return StatusCode(res.GetStatusCode(), res.Result);
        }
    }
}