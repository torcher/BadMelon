using BadMelon.Data.DTOs;
using BadMelon.Tests.Fixtures;
using BadMelon.Tests.Helpers;
using System.Threading.Tasks;
using Xunit;

namespace BadMelon.Tests.Controllers
{
    public class AuthControllerTests : ControllerTestsFixture
    {
        public AuthControllerTests() : base()
        {
            Logout().Wait();
        }

        [Fact]
        public async Task PostLogin_WhenGoodLogin_ExpectSuccessAndAccess()
        {
            await Logout();
            var login = dataSamples.RootUserLogin;
            var loginResponse = await _http.PostAsync("api/auth/login", login.GetStringContent());
            loginResponse.EnsureSuccessStatusCode();

            _http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "");
            var ingredientTypesResponse = await _http.GetAsync("api/ingredienttype");
            ingredientTypesResponse.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task PostLogin_WhenBadLogin_ExpectNotFound()
        {
            var login = new Login { Username = "root", Password = "pwdroot" };
            var loginResponse = await _http.PostAsync("api/auth/login", login.GetStringContent());
            Assert.True(loginResponse.StatusCode == System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task PostLogin_WhenInvalidLogin_ExpectBadRequest()
        {
            var login = new Login { Username = "root" };
            var loginResponse = await _http.PostAsync("api/auth/login", login.GetStringContent());
            Assert.True(loginResponse.StatusCode == System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GetDatabaseMigration_WhenLoggedOut_ExpectUnauthorized()
        {
            await Logout();
            var migrationResponse = await _http.GetAsync("api/database/migrate");
            Assert.True(migrationResponse.StatusCode == System.Net.HttpStatusCode.Unauthorized);
        }
    }
}