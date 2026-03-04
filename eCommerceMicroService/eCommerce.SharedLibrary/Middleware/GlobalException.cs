using eCommerce.SharedLibrary.Logs;
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
    public class GlobalException(RequestDelegate next)
    {
        public async Task InvokeAync(HttpContext httpsContext)
        {
            //default values
            string message = "Sorry, internal server error";
            int statusCode = (int)HttpStatusCode.InternalServerError;
            string title = "Error";

            try
            {
                await next(httpsContext);

                //check if exception is too many request I.e 429 status code
                if (httpsContext.Response.StatusCode == StatusCodes.Status429TooManyRequests)
                {
                    title = "Warning";
                    message = "Too many request";
                    statusCode = (int)HttpStatusCode.TooManyRequests;
                    await ModifyHeader(httpsContext, title, message, statusCode);
                }

                //check if exception is not found I.e 404 status code



                // check if exception is bad request I.e 400 status code


                //check if exception is unauthorized I.e 401 status code
                if (httpsContext.Response.StatusCode == StatusCodes.Status401Unauthorized)
                {
                    title = "Unauthorized";
                    message = "You are not authorized to access this resource";
                    statusCode = (int)HttpStatusCode.Unauthorized;
                    await ModifyHeader(httpsContext, title, message, statusCode);
                }

                // check if exception is forbidden I.e 403 status code
                if (httpsContext.Response.StatusCode == StatusCodes.Status403Forbidden)
                {
                    title = "Forbidden";
                    message = "You don't have permission to access this resource";
                    statusCode = (int)HttpStatusCode.Forbidden;
                    await ModifyHeader(httpsContext, title, message, statusCode);
                }
            }
            catch (Exception ex)
            {
                // log exception here
                LogExceptions.LogException(ex);
                if (ex is TaskCanceledException || ex is TimeoutException)
                {
                    title = "Request Timeout";
                    message = "The request has timed out. Please try again later.";
                    statusCode = (int)HttpStatusCode.RequestTimeout;
                }

                //If none of the above exceptio is cought then return default values
                await ModifyHeader(httpsContext, title, message, statusCode);
            }
        }

        private static async Task ModifyHeader(HttpContext httpsContext, string title, string message, int statusCode)
        {
            // display understandable message to client
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
