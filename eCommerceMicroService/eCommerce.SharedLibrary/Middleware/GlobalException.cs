using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SharedLibrary.Middleware
{
    public class GlobalException(RequestDelegate next)
    {
        public async Task InvokeAync(HttpContext httpsContext)
        {
            string message = "Sorry, internal server error";
            int statusCode = (int)HttpStatusCode.InternalServerError;
            string title = "Error";

            try
            {
                await next(httpsContext);

                //check if exception is too many request I.e 429 status code
                if(httpsContext.Response.StatusCode == StatusCodes.Status429TooManyRequests)
                {
                    title = "Warning";
                    message = "Too many request";
                    statusCode = (int)HttpStatusCode.TooManyRequests;
                    await ModifyHeader(httpsContext,title,message,statusCode);
                }
            }
            catch
            {

            }
        }

        private static async Task ModifyHeader(HttpContext httpsContext, string title, string message, int statusCode)
        {
            // display understandable message to clint
            httpsContext.Response.ContentType = "application/json";
            await httpsContext.Response.WriteAsync(JsonSerializer.Serialize(new ProblemDetails()
            {
                Detail = message,
                Status = statusCode,
                Title = title  
            }), CancellationToken.None);
            return;
        }
    }
}
