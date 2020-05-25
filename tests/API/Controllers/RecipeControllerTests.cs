using BadMelon.API.Services;
using BadMelon.Data;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace BadMelon.Tests.API.Controllers
{
    public class RecipeControllerTests
    {
        private readonly HttpClient _http;
        private DataSamples dataSamples;
        private readonly TestServer testServer;

        public RecipeControllerTests()
        {
            testServer = new TestServer(WebHost.CreateDefaultBuilder().UseStartup<BadMelon.API.Startup>().UseEnvironment("Testing"));
            _http = testServer.CreateClient();

            dataSamples = new DataSamples();
        }

        [Fact]
        public async Task Get_Recipe_ExpectAllRecipes()
        {
            await _http.GetAsync("api/database/seed");
            var expectedRecipes = dataSamples.Recipes.ConvertToDTOs();
            var response = await _http.GetAsync("api/recipe");
            response.EnsureSuccessStatusCode();

            var recipesJson = await response.Content.ReadAsStringAsync();
            var recipes = (JArray)JsonConvert.DeserializeObject(recipesJson);
            Assert.True(recipes != null && recipes.Count == expectedRecipes.Length, $"Should have retrieved {expectedRecipes.Length} Recipes.");
        }
    }
}