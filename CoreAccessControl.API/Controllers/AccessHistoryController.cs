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
    [Route("api/ver{version:apiVersion}/location/{locationId}/accessHistory")]
    [ApiController]
    [Authorize]
    public class AccessHistoryController : ControllerBase
    {
        private readonly IAccessHistoryService _accessHistoryService;
        private readonly AuthHelpers _authHelpers;

        public AccessHistoryController(IAccessHistoryService ccessHistoryService, AuthHelpers authHelpers)
        {
            _accessHistoryService = ccessHistoryService;
            _authHelpers = authHelpers;
        }

        [HttpGet]
        public async Task<ActionResult> Get([FromRoute][Required] long locationId, [FromQuery]AccessHistorySearchReqModel model)
        {
            var res = await _accessHistoryService.Get(locationId, model);
            return StatusCode(res.GetStatusCode(), res.Result);
        }

        [HttpGet]
        [Route("export")]
        public async Task<ActionResult> Export([FromRoute][Required] long locationId, [FromQuery]AccessHistorySearchReqModel model, [FromQuery][Required] string type)
        {
            if (type != "excel")
            {
                return BadRequest(new ErrorModel { Message = "Not supported type" });
            }
            var res = await _accessHistoryService.Get(locationId, model);

            if(res.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return StatusCode(res.GetStatusCode(), res.Result);
            }

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage excel = new ExcelPackage();
            
            var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
            workSheet.Cells[1, 1].LoadFromCollection(((AccessHistoryRespModel)res.Result).Items, true);

            return File(excel.GetAsByteArray(), "application/vnd.ms-excel", "data.xlsx");
        }
    }
}