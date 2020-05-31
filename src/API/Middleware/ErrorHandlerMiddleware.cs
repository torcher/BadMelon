using BadMelon.Data.Exceptions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace BadMelon.API.Middleware
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            string errorMessage = "";
            try
            {
                await _next.Invoke(context);
            }
            catch (EntityNotFoundException ex)
            {
                context.Response.StatusCode = 404;
                errorMessage = JsonConvert.SerializeObject(new { ex.Message });
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                errorMessage = JsonConvert.SerializeObject(new { ex.Message, Detail = ex.InnerException?.Message });
            }

            if (!context.Response.HasStarted)
            {
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(errorMessage);
            }
        }
    }
}