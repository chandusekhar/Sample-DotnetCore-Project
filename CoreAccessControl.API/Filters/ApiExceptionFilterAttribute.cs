using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Supra.LittleLogger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CoreAccessControl.API.Filters
{
    public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private const string UnhandledException = "Unhandeled Exception occured in ApiExceptionFilterAttribute";

        public ApiExceptionFilterAttribute()
        {

        }

        public override void OnException(ExceptionContext context)
        {
            HandleException(context);
            context.HttpContext.Response.StatusCode = 500;
            context.HttpContext.Response.WriteAsync(context.Exception.Message).Wait();
        }

        public override Task OnExceptionAsync(ExceptionContext context)
        {
            HandleException(context);
            context.HttpContext.Response.StatusCode = 500;
            return Task.CompletedTask;
        }

        private void HandleException(ExceptionContext context)
        {
            try
            {
                context.Result = CreateErrorResult(context.Exception.Message, context.Exception);
            }
            catch (Exception exception)
            {
                context.Result = CreateErrorResult(UnhandledException, exception);
            }

            context.ExceptionHandled = true;
        }

        private IActionResult CreateErrorResult(string message, Exception exception)
        {
            Logger.WriteException(exception);

            var content = JsonConvert.SerializeObject(new
            {
                Error = message,
                Result = exception
            }, Formatting.Indented,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                }
            );

            return new ContentResult()
            {
                Content = content,
                ContentType = "application/json",
                StatusCode = (int)HttpStatusCode.InternalServerError
            };
        }
    }
}
