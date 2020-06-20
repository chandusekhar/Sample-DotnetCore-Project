using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CoreAccessControl.API.Helpers;
using CoreAccessControl.Domain.RequestModels;
using CoreAccessControl.Domain.ResponseModels;
using CoreAccessControl.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreAccessControl.API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/ver{version:apiVersion}/auth")]
    [ApiController]
    [Authorize]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly AuthHelpers _authHelpers;

        public AuthController(IAuthService authService, AuthHelpers authHelpers)
        {
            _authService = authService;
            _authHelpers = authHelpers;
        }

        [HttpPost]
        [Route("register")]
        [AllowAnonymous]
        public async Task<ActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorModel { Message = string.Join(",", ModelState.Values.SelectMany(v => v.Errors)) });
            }

            var res = await _authService.Register(model);

            return StatusCode(res.GetStatusCode(), res.Result);
        }

        [HttpGet]
        [Route("verifyEmail")]
        [AllowAnonymous]
        public async Task<ActionResult> VerifyEmail([FromQuery] string token, [FromQuery] bool update)
        {
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest(new ErrorModel { Message = "Not a valid token" });
            }

            var res = await _authService.VerifyEmail(token, update);

            return StatusCode(res.GetStatusCode(), res.Result);
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<ActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorModel { Message = string.Join(",", ModelState.Values.SelectMany(v => v.Errors)) });
            }

            var res = await _authService.Login(model);

            return StatusCode(res.GetStatusCode(), res.Result);
        }

        [HttpPut]
        [Route("securityQuestion")]
        public async Task<ActionResult> UpdateSecurityQuestion([FromBody] UpdateSecurityQuestionReqModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorModel { Message = string.Join(",", ModelState.Values.SelectMany(v => v.Errors)) });
            }


            var res = await _authService.UpdateSecurityQuestion(model, _authHelpers.GetCurrentUserId());

            return StatusCode(res.GetStatusCode(), res.Result);

        }

        [HttpGet]
        [Route("securityQuestion")]
        public async Task<ActionResult> GetSecurityQuestion([FromQuery][Required] string email)
        {
            var res = await _authService.GetSecurityQuestion(email);

            return StatusCode(res.GetStatusCode(), res.Result);

        }

        [HttpPost]
        [Route("forgotPassword")]
        [AllowAnonymous]
        public async Task<ActionResult> ForgotPassword([FromBody] PasswordRecoverReqModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorModel { Message = string.Join(",", ModelState.Values.SelectMany(v => v.Errors)) });
            }

            var res = await _authService.ForgotPassword(model);

            return StatusCode(res.GetStatusCode(), res.Result);

        }


        [HttpPost]
        [Route("changePassword")]
        public async Task<ActionResult> ChangePassword(
        [FromBody]
        [Required(ErrorMessage = "Password is required.")]
        [MaxLength(250, ErrorMessage = "Password max length sould be 250 character.")]
        string password)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorModel { Message = string.Join(",", ModelState.Values.SelectMany(v => v.Errors)) });
            }

            var res = await _authService.ChangePassword(new ChangePasswordReqModel { Password = password }, _authHelpers.GetCurrentUserId());

            return StatusCode(res.GetStatusCode(), res.Result);

        }
    }
}