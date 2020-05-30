using BadMelon.API.Middleware;
using Microsoft.AspNetCore.Builder;

namespace BadMelon.API.Helpers
{
    public static class ErrorHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseBadMelonErrorHandler(
               this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlerMiddleware>();
        }
    }
}