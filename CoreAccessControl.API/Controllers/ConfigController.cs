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
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OfficeOpenXml;

namespace CoreAccessControl.API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/ver{version:apiVersion}/location/{locationId}/config")]
    [ApiController]
    [Authorize]
    [CoreAccessAuthorize(PermissionDomain.Config, PermissionAction.Read)]
    public class ConfigController : ControllerBase
    {
        private readonly IConfigService _configService;
        private readonly AuthHelpers _authHelpers;

        public ConfigController(IConfigService configService, AuthHelpers authHelpers)
        {
            _configService = configService;
            _authHelpers = authHelpers;
        }

        [HttpGet]
        [Route("statuses")]
        public async Task<ActionResult> GetStatuses([FromRoute][Required] long locationId, [FromQuery][BindRequired]string type, [FromQuery] bool isFull)
        {
            var res = await _configService.GetStatuses(locationId, type, isFull);
            return StatusCode(res.GetStatusCode(), res.Result);
        }

        [HttpPost]
        [Route("statuses")]
        [CoreAccessAuthorize(PermissionDomain.Config, PermissionAction.Write)]
        public async Task<ActionResult> SaveStatuses([FromRoute][Required] long locationId, [FromQuery][BindRequired]string type, [FromBody]ConfigStatusReqModel model)
        {
            var res = await _configService.SaveStatuse(locationId, _authHelpers.GetCurrentUserId().Value, type, model);
            return StatusCode(res.GetStatusCode(), res.Result);
        }

        [HttpPut]
        [Route("statuses/{id}")]
        [CoreAccessAuthorize(PermissionDomain.Config, PermissionAction.Write)]
        public async Task<ActionResult> UpdateStatuse([FromRoute][Required] long locationId, [FromRoute][Required] long id, [FromQuery][BindRequired]string type, [FromBody]ConfigStatusReqModel model)
        {
            var res = await _configService.UpdateStatuse(locationId, _authHelpers.GetCurrentUserId().Value, id, type, model);
            return StatusCode(res.GetStatusCode(), res.Result);
        }

        [HttpDelete]
        [Route("statuses/{id}")]
        [CoreAccessAuthorize(PermissionDomain.Config, PermissionAction.Write)]
        public async Task<ActionResult> DeleteStatuse([FromRoute][Required] long locationId, [FromRoute][Required] long id, [FromQuery][BindRequired]string type)
        {
            var res = await _configService.DeleteStatuse(locationId, _authHelpers.GetCurrentUserId().Value, id, type);
            return StatusCode(res.GetStatusCode(), res.Result);
        }
    }
}