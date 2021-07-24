using BadMelon.API.Middleware;
using Microsoft.AspNetCore.Builder;

namespace BadMelon.API.Helpers
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseBadMelonErrorHandler(
               this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlerMiddleware>();
        }

        public static IApplicationBuilder UserBadMelonJwtHandler(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<JwtHandlerMiddleware>();
        }
    }
}