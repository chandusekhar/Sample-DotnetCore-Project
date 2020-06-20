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
using OfficeOpenXml;

namespace CoreAccessControl.API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/ver{version:apiVersion}/location/{locationId}/administrators")]
    [ApiController]
    [Authorize]
    
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly AuthHelpers _authHelpers;
        private readonly IAuthService _authService;

        public AdminController(IAdminService adminService, AuthHelpers authHelpers, IAuthService authService)
        {
            _adminService = adminService;
            _authHelpers = authHelpers;
            _authService = authService;
        }

        [HttpGet]        
        [CoreAccessAuthorize(PermissionDomain.Admin, PermissionActionCondition.Or, PermissionAction.Read, PermissionAction.Write)]
        public async Task<ActionResult> Get([FromRoute] long locationId, [FromQuery] AdminSearchReqModel model)
        {
            return Ok(await _adminService.SearchAdmin(locationId, _authHelpers.GetCurrentUserId().Value, model));
        }

        [HttpPost]
        [Route("")]
        [CoreAccessAuthorize(PermissionDomain.Admin, PermissionAction.Write)]
        public async Task<ActionResult> Create([FromRoute] long locationId, [FromBody] AdministratorReqModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorModel { Message = string.Join(",", ModelState.Values.SelectMany(v => v.Errors.Select(x => x.ErrorMessage))) });
            }

            if (await _authService.IsEmailExists(model.Email))
            {
                var res = await _adminService.UpdatedAdmin(locationId, _authHelpers.GetCurrentUserId().Value, model);
                return StatusCode(res.GetStatusCode(), res.Result);
            }
            else
            {
                var res = await _adminService.CreateAdmin(locationId, _authHelpers.GetCurrentUserId().Value, model);
                return StatusCode(res.GetStatusCode(), res.Result);
            }


        }

        [HttpPut]
        [Route("{administratorId}")]
        [CoreAccessAuthorize(PermissionDomain.Admin, PermissionAction.Write)]
        public async Task<ActionResult> Update([FromRoute] long locationId, [FromRoute] long administratorId, [FromBody] AdministratorUpdateReqModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorModel { Message = string.Join(",", ModelState.Values.SelectMany(v => v.Errors)) });
            }

            if(model.State.HasValue && model.State.Value == AdministratorState.Disabled && string.IsNullOrEmpty(model.DisabledReason))
            {
                return BadRequest(new ErrorModel { Message = "Must be provide disabled reason for disable state." });
            }

            var res = await _adminService.UpdatedAdmin(locationId, _authHelpers.GetCurrentUserId().Value, administratorId, model);
            return StatusCode(res.GetStatusCode(), res.Result);
        }

        [HttpDelete]
        [Route("{administratorId}")]
        [CoreAccessAuthorize(PermissionDomain.Admin, PermissionAction.Write)]
        public async Task<ActionResult> Delete([FromRoute] long locationId, [FromRoute] long administratorId)
        {
            var res = await _adminService.DeleteAdmin(locationId, _authHelpers.GetCurrentUserId().Value, administratorId);
            return StatusCode(res.GetStatusCode(), res.Result);
        }


        [HttpGet]
        [Route("{administratorId}/activities")]
        [CoreAccessAuthorize(PermissionDomain.Admin, PermissionActionCondition.Or, PermissionAction.Read, PermissionAction.Write)]
        public async Task<ActionResult> Activities([FromRoute] long locationId, [FromRoute] long administratorId, [FromQuery] QueryReqModel query)
        {
            var res = await _adminService.GetActivities(locationId, _authHelpers.GetCurrentUserId().Value, administratorId, query);
            return StatusCode(res.GetStatusCode(), res.Result);
        }

        [HttpGet]
        [Route("export")]
        public async Task<ActionResult> Export([FromRoute] long locationId, [FromRoute] [FromQuery] AdminSearchReqModel query)
        {
            if(query.Type != "excel")
            {
                return BadRequest(new ErrorModel { Message = "Not supported type" });
            }

            var res = await _adminService.SearchAdmin(locationId, _authHelpers.GetCurrentUserId().Value, query);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage excel = new ExcelPackage();
            var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
            workSheet.Cells[1, 1].LoadFromCollection(((AdministratorSearchResult)res.Result).Items, true);
            
            return File(excel.GetAsByteArray(), "application/vnd.ms-excel", "data.xlsx");
        }
    }
}