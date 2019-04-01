using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TimeTrackingServer.Exceptions;

namespace TimeTrackingServer.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;
        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context /* other dependencies */)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var code = HttpStatusCode.InternalServerError; // 500 if unexpected

            //if (ex is MyNotFoundException) code = HttpStatusCode.NotFound;
            //else if (ex is MyUnauthorizedException) code = HttpStatusCode.Unauthorized;
            //else if (ex is MyException) code = HttpStatusCode.BadRequest;

            var defaultException = ex;
            if (ex is ApiException)
            {
                defaultException = ex.InnerException;
            }
            var errorCode = (defaultException is TimeTrackingServerException) ? ((TimeTrackingServerException)defaultException).ErrorCode : (int)code;

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            var result = JsonConvert.SerializeObject(new ErrorBase()
            {
                ErrorCode = errorCode,
                FailReason = defaultException.Message,
                ExceptionType = defaultException.GetType().Name
            });

            return context.Response.WriteAsync(result);
        }
    }
}
