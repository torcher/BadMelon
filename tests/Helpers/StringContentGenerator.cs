using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace BadMelon.Tests.Helpers
{
    public static class StringContentGenerator
    {
        public static StringContent GetJSON(object content)
        {
            return new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
        }
    }
}