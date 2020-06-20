using CoreAccessControl.API.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Moq;
using Supra.LittleLogger;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace CoreAccessControl.Api.Test
{
    public class ActionFilterAttributeTest
    {
        [Fact]
        public void Invalid_ModelState_Should_Return_BadRequestObjectResult()
        {
            Logger.Init("", "CoreAccessControl.log", "CoreAccessControl", Severity.Information, mock: true);
            //Arrange
            var modelState = new ModelStateDictionary();
            modelState.AddModelError("", "error");
            var httpContext = new DefaultHttpContext();
            var context = new ActionExecutingContext(
                new ActionContext(
                    httpContext: httpContext,
                    routeData: new RouteData(),
                    actionDescriptor: new ActionDescriptor(),
                    modelState: modelState
                ),
                new List<IFilterMetadata>(),
                new Dictionary<string, object>(),
                new Mock<Controller>().Object);

            var attribute = new ModelStateValidationAttribute();
            attribute.OnActionExecuting(context);

            Assert.IsType<BadRequestObjectResult>(context.Result);
        }
    }
}
