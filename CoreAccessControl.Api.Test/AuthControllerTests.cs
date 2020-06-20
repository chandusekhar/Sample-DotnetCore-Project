using CoreAccessControl.Api.Test.Fakes;
using CoreAccessControl.API.Controllers;
using CoreAccessControl.API.Helpers;
using CoreAccessControl.DataAccess.Ef.Data;
using CoreAccessControl.DataAccess.Ef.Models;
using CoreAccessControl.Domain.Configuration;
using CoreAccessControl.Domain.Models;
using CoreAccessControl.Domain.RequestModels;
using CoreAccessControl.Domain.ResponseModels;
using CoreAccessControl.Services;
using CoreAccessControl.Services.ApiModel.Response;
using CoreAccessControl.Services.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Supra.LittleLogger;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CoreAccessControl.Api.Test
{
    public class AuthControllerTests : ControllerTestBase
    {
        public AuthController GetAuthController(CoreaccesscontrolContext context)
        {
            Logger.Init(GetAppSettings().ConnectionStrings.DefaultConnection, "CoreAccessControl.log", "CoreAccessControl", Severity.Information, mock: true);
            var authHelpers = new AuthHelpers(GetMockHttpContextAccessor().Object);
            var authService = new AuthService(context, GetAppSettings(), new FakeEmailSender(), new FakeEmailService());
            return new AuthController(authService, authHelpers);
        }


        [Fact]
        public async Task Register_SuccessRequest()
        {
            var context = GetInMemoryContext();
            var сontroller = GetAuthController(context);
            var response = await сontroller.Register(new RegisterModel { 
                Email = "test@test.com",
                Password = "123456",
                SecurityQuestion = "asd",
                SecurityQuestionAnswer = "asd"
            });

            var okResult = Assert.IsType<ObjectResult>(response);
            Assert.Equal((int)HttpStatusCode.OK, (int)okResult.StatusCode.Value);
        }

        [Fact]
        public async Task Register_BadRequest_ModelState()
        {
            var context = GetInMemoryContext();
            var controller = GetAuthController(context);
            controller.ModelState.AddModelError("Email", "Required");
            var response = await controller.Register(new RegisterModel
            {
                Email = "test@test.com",
                Password = "123456",
                SecurityQuestion = "asd",
                SecurityQuestionAnswer = "asd"
            });

            Assert.IsType<BadRequestObjectResult>(response);
        }

        [Fact]
        public async Task Register_BadRequest_EmailExistsInUser()
        {
            var context = GetInMemoryContext();
            context.Add(new User
            {
                Id = 1,
                Email = "test@test.com"
            });
            context.SaveChanges();
            var controller = GetAuthController(context);
            var response = await controller.Register(new RegisterModel
            {
                Email = "test@test.com",
                Password = "123456",
                SecurityQuestion = "asd",
                SecurityQuestionAnswer = "asd"
            });

            var okResult = Assert.IsType<ObjectResult>(response);
            Assert.Equal((int)HttpStatusCode.BadRequest, (int)okResult.StatusCode.Value);
        }

        [Fact]
        public async Task Register_BadRequest_EmailExistsInChangeEmail()
        {
            var context = GetInMemoryContext();
            context.Add(new ChangeEmailRequest
            {
                Id = 1,
                Email = "test@test.com"
            });
            context.SaveChanges();
            var controller = GetAuthController(context);
            var response = await controller.Register(new RegisterModel
            {
                Email = "test@test.com",
                Password = "123456",
                SecurityQuestion = "asd",
                SecurityQuestionAnswer = "asd"
            });

            var okResult = Assert.IsType<ObjectResult>(response);
            Assert.Equal((int)HttpStatusCode.BadRequest, (int)okResult.StatusCode.Value);
        }


        [Fact]
        public async Task VerifyEmail_SuccessRequest()
        {
            var context = GetInMemoryContext();
            context.Add(new User
            {
                Id = 1,
                Email = "test@test.com",
                IsEmailVerified = false,
                VerificationToken = "123",
                VerificationTokenExpiry = DateTime.UtcNow.AddDays(1)
            });
            context.SaveChanges();

            var controller = GetAuthController(context);
            var response = await controller.VerifyEmail("123", false);

            var okResult = Assert.IsType<ObjectResult>(response);
            Assert.Equal((int)HttpStatusCode.OK, (int)okResult.StatusCode.Value);
        }

        [Fact]
        public async Task VerifyEmail_BadRequest_EmptyToken()
        {
            var context = GetInMemoryContext();
            context.Add(new User
            {
                Id = 1,
                Email = "test@test.com",
                IsEmailVerified = false,
                VerificationToken = "123",
                VerificationTokenExpiry = DateTime.UtcNow.AddDays(1)
            });
            context.SaveChanges();

            var controller = GetAuthController(context);
            var response = await controller.VerifyEmail("", false);

            Assert.IsType<BadRequestObjectResult>(response);
        }

        [Fact]
        public async Task VerifyEmail_BadRequest_TokenNotExists()
        {
            var context = GetInMemoryContext();
            context.Add(new User
            {
                Id = 1,
                Email = "test@test.com",
                IsEmailVerified = false,
                VerificationToken = "123",
                VerificationTokenExpiry = DateTime.UtcNow.AddDays(1)
            });
            context.SaveChanges();

            var controller = GetAuthController(context);
            var response = await controller.VerifyEmail("12", false);

            var okResult = Assert.IsType<ObjectResult>(response);
            Assert.Equal((int)HttpStatusCode.BadRequest, (int)okResult.StatusCode.Value);
        }

        [Fact]
        public async Task VerifyEmail_BadRequest_DatePassed()
        {
            var context = GetInMemoryContext();
            context.Add(new User
            {
                Id = 1,
                Email = "test@test.com",
                IsEmailVerified = false,
                VerificationToken = "123",
                VerificationTokenExpiry = DateTime.UtcNow.AddDays(-1)
            });
            context.SaveChanges();

            var controller = GetAuthController(context);
            var response = await controller.VerifyEmail("123", false);

            var okResult = Assert.IsType<ObjectResult>(response);
            Assert.Equal((int)HttpStatusCode.BadRequest, (int)okResult.StatusCode.Value);
        }

        [Fact]
        public async Task VerifyEmail_Change_SuccessRequest()
        {
            var context = GetInMemoryContext();
            context.Add(new User
            {
                Id = 1,
                Email = "test@test.com",
                VerificationToken = "123",
                VerificationTokenExpiry = DateTime.UtcNow.AddDays(1)
            });

            context.Add(new ChangeEmailRequest
            {
                Id = 1,
                Email = "test@test.com",
                VerificationToken = "123",
                VerificationTokenExpiry = DateTime.UtcNow.AddDays(1),
                UserId = 1
            });
            context.SaveChanges();

            var controller = GetAuthController(context);
            var response = await controller.VerifyEmail("123", true);

            var okResult = Assert.IsType<ObjectResult>(response);
            Assert.Equal((int)HttpStatusCode.OK, (int)okResult.StatusCode.Value);
        }

        [Fact]
        public async Task VerifyEmail_Change_BadRequest_EmptyToken()
        {
            var context = GetInMemoryContext();
            context.Add(new ChangeEmailRequest
            {
                Id = 1,
                Email = "test@test.com",
                VerificationToken = "123",
                VerificationTokenExpiry = DateTime.UtcNow.AddDays(1)
            });
            context.SaveChanges();

            var controller = GetAuthController(context);
            var response = await controller.VerifyEmail("", true);

            Assert.IsType<BadRequestObjectResult>(response);
        }

        [Fact]
        public async Task VerifyEmail_Change_BadRequest_TokenNotExists()
        {
            var context = GetInMemoryContext();
            context.Add(new User
            {
                Id = 1,
                Email = "test@test.com",
                IsEmailVerified = false,
                VerificationToken = "123",
                VerificationTokenExpiry = DateTime.UtcNow.AddDays(1)
            });
            context.SaveChanges();

            var controller = GetAuthController(context);
            var response = await controller.VerifyEmail("12", true);

            var okResult = Assert.IsType<ObjectResult>(response);
            Assert.Equal((int)HttpStatusCode.BadRequest, (int)okResult.StatusCode.Value);
        }

        [Fact]
        public async Task VerifyEmail_Change_BadRequest_DatePassed()
        {
            var context = GetInMemoryContext();
            context.Add(new ChangeEmailRequest
            {
                Id = 1,
                Email = "test@test.com",
                VerificationToken = "123",
                VerificationTokenExpiry = DateTime.UtcNow.AddDays(-1)
            });
            context.SaveChanges();

            var controller = GetAuthController(context);
            var response = await controller.VerifyEmail("123", true);

            var okResult = Assert.IsType<ObjectResult>(response);
            Assert.Equal((int)HttpStatusCode.BadRequest, (int)okResult.StatusCode.Value);
        }

        [Fact]
        public async Task Login_Success()
        {
            var context = GetInMemoryContext();
            context.Add(new User
            {
                Id = 1,
                Email = "test@test.com",
                IsEmailVerified = true,
                VerificationToken = "123",
                VerificationTokenExpiry = DateTime.UtcNow.AddDays(1),
                PasswordHash = HashUtility.CreatePasswordHash("123456", GetAppSettings().Secret),
            });
            context.SaveChanges();

            var controller = GetAuthController(context);
            var response = await controller.Login(new LoginModel
            {
                Email = "test@test.com",
                Password = "123456"
            });

            var okResult = Assert.IsType<ObjectResult>(response);
            Assert.Equal((int)HttpStatusCode.OK, (int)okResult.StatusCode.Value);
        }

        [Fact]
        public async Task Login_BadRequest_ModelState()
        {
            var context = GetInMemoryContext();
            context.Add(new User
            {
                Id = 1,
                Email = "test@test.com",
                IsEmailVerified = true,
                VerificationToken = "123",
                VerificationTokenExpiry = DateTime.UtcNow.AddDays(1)
            });
            context.SaveChanges();

            var controller = GetAuthController(context);
            controller.ModelState.AddModelError("Email", "asd");
            var response = await controller.Login(new LoginModel
            {
                Email = "test@test.com",
                Password = "123456"
            });

            Assert.IsType<BadRequestObjectResult>(response);
            
        }

        [Fact]
        public async Task Login_BadRequest_EmailNotExists()
        {
            var context = GetInMemoryContext();
            context.Add(new User
            {
                Id = 1,
                Email = "test@test.com",
                IsEmailVerified = true,
                VerificationToken = "123",
                VerificationTokenExpiry = DateTime.UtcNow.AddDays(1)
            });
            context.SaveChanges();

            var controller = GetAuthController(context);
            var response = await controller.Login(new LoginModel { 
                Email = "test@test.com",
                Password = "123456"
            });

            var okResult = Assert.IsType<ObjectResult>(response);
            Assert.Equal((int)HttpStatusCode.BadRequest, (int)okResult.StatusCode.Value);
        }

        [Fact]
        public async Task Login_BadRequest_EmailNotVerified()
        {
            var context = GetInMemoryContext();
            context.Add(new User
            {
                Id = 1,
                Email = "test@test.com",
                IsEmailVerified = false,
                VerificationToken = "123",
                VerificationTokenExpiry = DateTime.UtcNow.AddDays(1)
            });
            context.SaveChanges();

            var controller = GetAuthController(context);
            var response = await controller.Login(new LoginModel
            {
                Email = "test@test.com",
                Password = "123456"
            });

            var okResult = Assert.IsType<ObjectResult>(response);
            Assert.Equal((int)HttpStatusCode.BadRequest, (int)okResult.StatusCode.Value);

            context = GetInMemoryContext();
            context.Add(new User
            {
                Id = 1,
                Email = "test@test.com",
                VerificationToken = "123",
                VerificationTokenExpiry = DateTime.UtcNow.AddDays(1)
            });
            context.SaveChanges();

            response = await controller.Login(new LoginModel
            {
                Email = "test@test.com",
                Password = "123456"
            });

            okResult = Assert.IsType<ObjectResult>(response);
            Assert.Equal((int)HttpStatusCode.BadRequest, (int)okResult.StatusCode.Value);
        }

        [Fact]
        public async Task UpdateSecurityQuestion_Success()
        {
            var context = GetInMemoryContext();
            context.Add(new User
            {
                Id = 1,
                Email = "test@test.com",
                IsEmailVerified = true,
                VerificationToken = "123",
                VerificationTokenExpiry = DateTime.UtcNow.AddDays(1),
                PasswordHash = HashUtility.CreatePasswordHash("123456", "test test test test"),
            });
            context.SaveChanges();

            var controller = GetAuthController(context);
            var response = await controller.UpdateSecurityQuestion(new UpdateSecurityQuestionReqModel
            {
                Answer = "asdasd",
                Question = "asdasdasd"
            });

            var okResult = Assert.IsType<ObjectResult>(response);
            Assert.Equal((int)HttpStatusCode.OK, (int)okResult.StatusCode.Value);
        }

        [Fact]
        public async Task UpdateSecurityQuestion_ModelState()
        {
            var context = GetInMemoryContext();
            context.Add(new User
            {
                Id = 1,
                Email = "test@test.com",
                IsEmailVerified = true,
                VerificationToken = "123",
                VerificationTokenExpiry = DateTime.UtcNow.AddDays(1)
            });
            context.SaveChanges();

            var controller = GetAuthController(context);
            controller.ModelState.AddModelError("Email", "asd");
            var response = await controller.UpdateSecurityQuestion(new UpdateSecurityQuestionReqModel
            {
                Answer = "asdasd",
                Question = "asdasdasd"
            });

            Assert.IsType<BadRequestObjectResult>(response);

        }


        [Fact]
        public async Task GetSecurityQuestion_Success()
        {
            var context = GetInMemoryContext();
            context.Add(new User
            {
                Id = 1,
                Email = "test@test.com",
                IsEmailVerified = true,
                VerificationToken = "123",
                VerificationTokenExpiry = DateTime.UtcNow.AddDays(1),
                PasswordHash = HashUtility.CreatePasswordHash("123456", "test test test test"),
                SecurityQuestion = "asdkhagsdjgasd",
                SecurityQuestionAnswer = "asjdhjashd"
            });
            context.SaveChanges();

            var controller = GetAuthController(context);
            var response = await controller.GetSecurityQuestion("test@test.com");

            var okResult = Assert.IsType<ObjectResult>(response);
            Assert.Equal((int)HttpStatusCode.OK, (int)okResult.StatusCode.Value);
        }

        [Fact]
        public async Task GetSecurityQuestion_NotFound_EmailNotExists()
        {
            var context = GetInMemoryContext();
            context.Add(new User
            {
                Id = 1,
                Email = "test@test.com",
                IsEmailVerified = true,
                VerificationToken = "123",
                VerificationTokenExpiry = DateTime.UtcNow.AddDays(1),
                PasswordHash = HashUtility.CreatePasswordHash("123456", "test test test test"),
                SecurityQuestion = "asdkhagsdjgasd",
                SecurityQuestionAnswer = "asjdhjashd"
            });
            context.SaveChanges();

            var controller = GetAuthController(context);
            var response = await controller.GetSecurityQuestion("test1@test.com");

            var okResult = Assert.IsType<ObjectResult>(response);
            Assert.Equal((int)HttpStatusCode.NotFound, (int)okResult.StatusCode.Value);
        }

        [Fact]
        public async Task GetSecurityQuestion_BadRequest_SecurityQuestionNotExists()
        {
            var context = GetInMemoryContext();
            context.Add(new User
            {
                Id = 1,
                Email = "test@test.com",
                IsEmailVerified = true,
                VerificationToken = "123",
                VerificationTokenExpiry = DateTime.UtcNow.AddDays(1),
                PasswordHash = HashUtility.CreatePasswordHash("123456", "test test test test"),
            });
            context.SaveChanges();

            var controller = GetAuthController(context);
            var response = await controller.GetSecurityQuestion("test@test.com");

            var okResult = Assert.IsType<ObjectResult>(response);
            Assert.Equal((int)HttpStatusCode.BadRequest, (int)okResult.StatusCode.Value);
        }

        [Fact]
        public async Task ForgotPassword_Success()
        {
            var context = GetInMemoryContext();
            context.Add(new User
            {
                Id = 1,
                Email = "test@test.com",
                IsEmailVerified = true,
                VerificationToken = "123",
                VerificationTokenExpiry = DateTime.UtcNow.AddDays(1),
                PasswordHash = HashUtility.CreatePasswordHash("123456", "test test test test"),
                SecurityQuestion = "asdkhagsdjgasd",
                SecurityQuestionAnswer = "asjdhjashd"
            });
            context.SaveChanges();

            var controller = GetAuthController(context);
            var response = await controller.ForgotPassword(new PasswordRecoverReqModel
            { 
                Email = "test@test.com",
                SecurityQuestion = "asdkhagsdjgasd",
                SecurityQuestionReply = "asjdhjashd"
            });

            var okResult = Assert.IsType<ObjectResult>(response);
            Assert.Equal((int)HttpStatusCode.OK, (int)okResult.StatusCode.Value);
        }

        [Fact]
        public async Task ForgotPassword_NotFound_EmailNotExists()
        {
            var context = GetInMemoryContext();
            context.Add(new User
            {
                Id = 1,
                Email = "test@test.com",
                IsEmailVerified = true,
                VerificationToken = "123",
                VerificationTokenExpiry = DateTime.UtcNow.AddDays(1),
                PasswordHash = HashUtility.CreatePasswordHash("123456", "test test test test"),
                SecurityQuestion = "asdkhagsdjgasd",
                SecurityQuestionAnswer = "asjdhjashd"
            });
            context.SaveChanges();

            var controller = GetAuthController(context);
            var response = await controller.ForgotPassword(new PasswordRecoverReqModel
            {
                Email = "tes1t@test.com",
                SecurityQuestion = "asdkhagsdjgasd",
                SecurityQuestionReply = "asjdhjashd"
            });


            var okResult = Assert.IsType<ObjectResult>(response);
            Assert.Equal((int)HttpStatusCode.NotFound, (int)okResult.StatusCode.Value);
        }

        [Fact]
        public async Task ForgotPassword_BadRequest_SecurityQuestionNotMatch()
        {
            var context = GetInMemoryContext();
            context.Add(new User
            {
                Id = 1,
                Email = "test@test.com",
                IsEmailVerified = true,
                VerificationToken = "123",
                VerificationTokenExpiry = DateTime.UtcNow.AddDays(1),
                PasswordHash = HashUtility.CreatePasswordHash("123456", "test test test test"),
                SecurityQuestion = "asdkhagsdjgasd1",
                SecurityQuestionAnswer = "asjdhjashd"
            });
            context.SaveChanges();

            var controller = GetAuthController(context);
            var response = await controller.ForgotPassword(new PasswordRecoverReqModel
            {
                Email = "test@test.com",
                SecurityQuestion = "asdkhagsdjgasd",
                SecurityQuestionReply = "asjdhjashd"
            });


            var okResult = Assert.IsType<ObjectResult>(response);
            Assert.Equal((int)HttpStatusCode.BadRequest, (int)okResult.StatusCode.Value);
        }


        [Fact]
        public async Task ForgotPassword_BadRequest_SecurityQuestionAnswerNotMatch()
        {
            var context = GetInMemoryContext();
            context.Add(new User
            {
                Id = 1,
                Email = "test@test.com",
                IsEmailVerified = true,
                VerificationToken = "123",
                VerificationTokenExpiry = DateTime.UtcNow.AddDays(1),
                PasswordHash = HashUtility.CreatePasswordHash("123456", "test test test test"),
                SecurityQuestion = "asdkhagsdjgasd",
                SecurityQuestionAnswer = "asjdhjashd"
            });
            context.SaveChanges();

            var controller = GetAuthController(context);
            var response = await controller.ForgotPassword(new PasswordRecoverReqModel
            {
                Email = "test@test.com",
                SecurityQuestion = "asdkhagsdjgasd",
                SecurityQuestionReply = "asjdhjashd1"
            });


            var okResult = Assert.IsType<ObjectResult>(response);
            Assert.Equal((int)HttpStatusCode.BadRequest, (int)okResult.StatusCode.Value);
        }

        [Fact]
        public async Task ForgotPassword_BadRequest_EmailNotVerified()
        {
            var context = GetInMemoryContext();
            context.Add(new User
            {
                Id = 1,
                Email = "test@test.com",
                IsEmailVerified = false,
                VerificationToken = "123",
                VerificationTokenExpiry = DateTime.UtcNow.AddDays(1),
                PasswordHash = HashUtility.CreatePasswordHash("123456", "test test test test"),
                SecurityQuestion = "asdkhagsdjgasd",
                SecurityQuestionAnswer = "asjdhjashd"
            });
            context.SaveChanges();

            var controller = GetAuthController(context);
            var response = await controller.ForgotPassword(new PasswordRecoverReqModel
            {
                Email = "test@test.com",
                SecurityQuestion = "asdkhagsdjgasd",
                SecurityQuestionReply = "asjdhjashd"
            });


            var okResult = Assert.IsType<ObjectResult>(response);
            Assert.Equal((int)HttpStatusCode.BadRequest, (int)okResult.StatusCode.Value);
        }

        [Fact]
        public async Task ForgotPassword_ModelState()
        {
            var context = GetInMemoryContext();
            context.Add(new User
            {
                Id = 1,
                Email = "test@test.com",
                IsEmailVerified = true,
                VerificationToken = "123",
                VerificationTokenExpiry = DateTime.UtcNow.AddDays(1)
            });
            context.SaveChanges();

            var controller = GetAuthController(context);
            controller.ModelState.AddModelError("Email", "asd");
            var response = await controller.ForgotPassword(new PasswordRecoverReqModel
            {
                Email = "test@test.com",
                SecurityQuestion = "asdkhagsdjgasd",
                SecurityQuestionReply = "asjdhjashd"
            });

            Assert.IsType<BadRequestObjectResult>(response);

        }

        [Fact]
        public async Task ChangePassword_BadRequest_ModelState()
        {
            var context = GetInMemoryContext();
            context.Add(new User
            {
                Id = 1,
                Email = "test@test.com",
                IsEmailVerified = true,
                VerificationToken = "123",
                VerificationTokenExpiry = DateTime.UtcNow.AddDays(1)
            });
            context.SaveChanges();

            var controller = GetAuthController(context);
            controller.ModelState.AddModelError("Email", "asd");
            var response = await controller.ChangePassword("123456");

            Assert.IsType<BadRequestObjectResult>(response);

        }

        [Fact]
        public async Task ChangePassword_Success()
        {
            var context = GetInMemoryContext();
            context.Add(new User
            {
                Id = 1,
                Email = "test@test.com",
                IsEmailVerified = false,
                VerificationToken = "123",
                VerificationTokenExpiry = DateTime.UtcNow.AddDays(1),
                PasswordHash = HashUtility.CreatePasswordHash("123456", "test test test test"),
                SecurityQuestion = "asdkhagsdjgasd",
                SecurityQuestionAnswer = "asjdhjashd"
            });
            context.SaveChanges();

            var controller = GetAuthController(context);
            var response = await controller.ChangePassword("123456");

            var okResult = Assert.IsType<ObjectResult>(response);
            Assert.Equal((int)HttpStatusCode.OK, (int)okResult.StatusCode.Value);

        }

    }
}
