using BadMelon.Data.DTOs;
using BadMelon.Tests.Fixtures;
using BadMelon.Tests.Helpers;
using System;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xunit;

namespace BadMelon.Tests.Controllers
{
    [Collection("Controller collection")]
    public class AuthControllerTests : ControllerTestsFixture
    {
        private string GoodLongPassword = "long enough password to not be complex";
        private string WrongLongPassword = "not the right password";
        private string BadPassword = "password";

        public AuthControllerTests() : base()
        {
            Logout();
        }

        [Fact]
        public async Task PostLogin_WhenGoodLogin_ExpectSuccessAndAccess()
        {
            Login(dataSamples.Users.FirstOrDefault().Item2);

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
            var login = new Login { Password = "root" };
            var loginResponse = await _http.PostAsync("api/auth/login", login.GetStringContent());
            Assert.Equal(400, (int)loginResponse.StatusCode);
        }

        [Fact]
        public async Task GetDatabaseMigration_WhenLoggedOut_ExpectUnauthorized()
        {
            var migrationResponse = await _http.GetAsync("api/database/migrate");
            Assert.True(migrationResponse.StatusCode == System.Net.HttpStatusCode.Unauthorized);
        }

        //Password not confirmed
        [Fact]
        public async Task PostReset_WhenPasswordNotConfirmed_ExpectSuccess()
        {
            Logout();
            var user = dataSamples.Users.FirstOrDefault(u => !u.Item1.IsPasswordSet);
            user.Item2.Password = string.Empty;
            user.Item2.LoginMethod = LoginMethod.EMAIL;
            var emailLoginResponse = await _http.PostAsync("api/auth/login", user.Item2.GetStringContent());
            Assert.Equal(204, (int)emailLoginResponse.StatusCode);
            var loginCode = await _http.GetObject<Guid>($"api/testdata/verification-codes/{user.Item1.UserName}");
            var loginResponse = await _http.GetAsync($"api/auth/code/{loginCode}");
            Assert.Equal(200, (int)loginResponse.StatusCode);

            var jwt = await loginResponse.GetObject<Jwt>();
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt.token);

            var reset = new PasswordReset { NewPassword = GoodLongPassword };
            var resetResponse = await _http.PostAsync("api/auth/reset-password", reset.GetStringContent());
            resetResponse.StatusCode.AssertStatusCode(200);

            Logout();
            Login();
        }

        //Password confirmed
        [Fact]
        public async Task PostReset_WhenPasswordConfirmed_ExpectSuccess()
        {
            Login();
            var reset = new PasswordReset { CurrentPassword = GoodLongPassword, NewPassword = GoodLongPassword };
            var resetResponse = await _http.PostAsync("api/auth/reset-password", reset.GetStringContent());
            resetResponse.StatusCode.AssertStatusCode(200);
        }

        //Password confirmed - no password
        [Fact]
        public async Task PostReset_WhenPasswordConfirmedButNoPassword_ExpectError()
        {
            Login();
            var reset = new PasswordReset { CurrentPassword = GoodLongPassword, NewPassword = "" };
            var resetResponse = await _http.PostAsync("api/auth/reset-password", reset.GetStringContent());
            resetResponse.StatusCode.AssertStatusCode(400);
        }

        //Password confirmed - bad current password
        [Fact]
        public async Task PostReset_WhenPasswordConfirmedButBadCurrentPassword_ExpectError()
        {
            Login();
            var reset = new PasswordReset { CurrentPassword = WrongLongPassword, NewPassword = GoodLongPassword };
            var resetResponse = await _http.PostAsync("api/auth/reset-password", reset.GetStringContent());
            resetResponse.StatusCode.AssertStatusCode(400);
        }

        //Password confirmed - bad new password
        [Fact]
        public async Task PostReset_WhenPasswordConfirmedButBadNewPassword_ExpectError()
        {
            Login();
            var reset = new PasswordReset { CurrentPassword = GoodLongPassword, NewPassword = BadPassword };
            var resetResponse = await _http.PostAsync("api/auth/reset-password", reset.GetStringContent());
            resetResponse.StatusCode.AssertStatusCode(400);
        }
    }
}