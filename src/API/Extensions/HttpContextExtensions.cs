using Microsoft.AspNetCore.Http;

namespace BadMelon.API.Extensions
{
    public static class HttpContextExtensions
    {
        public static void SetResponseNotFound(this HttpContext http)
        {
            http.Response.StatusCode = 404;
        }

        public static void SetResponseServerError(this HttpContext http)
        {
            http.Response.StatusCode = 500;
        }
    }
}