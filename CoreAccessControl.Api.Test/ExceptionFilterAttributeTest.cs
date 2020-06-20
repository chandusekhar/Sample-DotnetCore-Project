using CoreAccessControl.API.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Moq;
using Supra.LittleLogger;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CoreAccessControl.Api.Test
{
    public class ExceptionFilterAttributeTest
    {
        [Fact]
        public void OnExceptionTests()
        {
            Logger.Init("", "CoreAccessControl.log", "CoreAccessControl", Severity.Information, mock: true);
            var actionContext = new ActionContext()
            {
                HttpContext = new DefaultHttpContext(),
                RouteData = new RouteData(),
                ActionDescriptor = new ActionDescriptor()
            };

            // The stacktrace message and source member variables are virtual and so we can stub them here.
            var mockException = new Mock<Exception>();

            mockException.Setup(e => e.StackTrace)
              .Returns("Test stacktrace");
            mockException.Setup(e => e.Message)
              .Returns("Test message");
            mockException.Setup(e => e.Source)
              .Returns("Test source");

            // the List<FilterMetadata> here doesn't have much relevance in the test but is required 
            // for instantiation. So we instantiate a new instance of it with no members to ensure
            // it does not effect the test.
            var exceptionContext = new ExceptionContext(actionContext, new List<IFilterMetadata>())
            {
                Exception = mockException.Object
            };

            var filter = new ApiExceptionFilterAttribute();

            filter.OnException(exceptionContext);

            // Assumption here that your exception filter modifies status codes.
            // Just used as an example of how you can assert in this test.
            Assert.Equal(500, (int)exceptionContext.HttpContext.Response.StatusCode);
        }

        [Fact]
        public async Task OnAsyncExceptionTests()
        {
            Logger.Init("", "CoreAccessControl.log", "CoreAccessControl", Severity.Information, mock: true);
            var actionContext = new ActionContext()
            {
                HttpContext = new DefaultHttpContext(),
                RouteData = new RouteData(),
                ActionDescriptor = new ActionDescriptor()
            };

            // The stacktrace message and source member variables are virtual and so we can stub them here.
            var mockException = new Mock<Exception>();

            mockException.Setup(e => e.StackTrace)
              .Returns("Test stacktrace");
            mockException.Setup(e => e.Message)
              .Returns("Test message");
            mockException.Setup(e => e.Source)
              .Returns("Test source");

            // the List<FilterMetadata> here doesn't have much relevance in the test but is required 
            // for instantiation. So we instantiate a new instance of it with no members to ensure
            // it does not effect the test.
            var exceptionContext = new ExceptionContext(actionContext, new List<IFilterMetadata>())
            {
                Exception = mockException.Object
            };

            var filter = new ApiExceptionFilterAttribute();

            await filter.OnExceptionAsync(exceptionContext);

            // Assumption here that your exception filter modifies status codes.
            // Just used as an example of how you can assert in this test.
            Assert.Equal(500, (int)exceptionContext.HttpContext.Response.StatusCode);
        }
    }
}
