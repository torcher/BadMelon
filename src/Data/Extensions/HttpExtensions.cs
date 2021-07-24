using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace BadMelon.Data.Extensions
{
    public static class HttpExtensions
    {
        public static void SetResponseNotFound(this HttpContext http)
        {
            http.Response.StatusCode = 404;
        }

        public static void SetResponseServerError(this HttpContext http)
        {
            http.Response.StatusCode = 500;
        }

        public static async Task<T> GetContent<T>(this HttpResponseMessage http)
        {
            return JsonConvert.DeserializeObject<T>(await http.Content.ReadAsStringAsync());
        }

        public static void SetBearerToken(this HttpClient client, string token)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }
}