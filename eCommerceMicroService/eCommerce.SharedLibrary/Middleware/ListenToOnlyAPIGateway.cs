using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace eCommerce.SharedLibrary.Middleware
{
    public class ListenToOnlyAPIGateway(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext httpContext)
        {
            // Extract specific header from the incoming request
            var signedHeader = httpContext.Request.Headers["X-API-Gateway-Signature"].FirstOrDefault();

            // Null means request is not coming from API Gateway
            if(signedHeader == null)
            {
                httpContext.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                await httpContext.Response.WriteAsync("Service Unavailable");
                return;
            }
            else
            {
                // Proceed to the next middleware/component in the pipeline
                await next(httpContext);
            }
        }
    }
}
