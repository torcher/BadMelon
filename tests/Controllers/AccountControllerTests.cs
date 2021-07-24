using BadMelon.Data.DTOs;
using BadMelon.Tests.Data;
using BadMelon.Tests.Fixtures;
using BadMelon.Tests.Helpers;
using System;
using System.Threading.Tasks;
using Xunit;

namespace BadMelon.Tests.Controllers
{
    [Collection("Controller collection")]
    public class AccountControllerTests : ControllerTestsFixture
    {
        public AccountControllerTests() : base()
        {
            Logout();
        }

        [Fact]
        public async Task PostRegistration_WhenGoodRegistration_ExpectOk()
        {
            var registration = new Registration
            {
                Username = "testuser",
                EmailAddress = "test@badmelon.com",
                Password = "password is long enough to bypass rules"
            };

            var registrationResponse = await _http.PostAsync("/api/account/register", registration.GetStringContent());
            registrationResponse.EnsureSuccessStatusCode();
        }

        //Bad Registrations
        [Theory]
        [ClassData(typeof(BadRegistrationsTestData))]
        public async Task PostRegistration_WhenBadRegistration_ExpectBadRequest(Registration registration)
        {
            var registrationResponse = await _http.PostAsync("/api/account/register", registration.GetStringContent());
            Assert.Equal(400, (int)registrationResponse.StatusCode);
        }

        //Login without verification
        [Fact]
        public async Task PostRegistration_WhenRegisteredButNotVerified_ExpectNotFound_ThenVerify_ExpectLoginSuccess()
        {
            var registration = new Registration
            {
                Username = "loginwoveri",
                EmailAddress = "loginwithout@verify.com",
                Password = "no more bad melons for this guy"
            };

            var registrationResponse = await _http.PostAsync("/api/account/register", registration.GetStringContent());
            registrationResponse.EnsureSuccessStatusCode();

            var login = new Login { Username = registration.Username, Password = registration.Password };
            var loginResponse = await _http.PostAsync("/api/auth/login", login.GetStringContent());
            Assert.Equal(404, (int)loginResponse.StatusCode);

            var verificationCodeResponse = await _http.GetAsync("/api/testdata/verification-codes/" + login.Username);
            string verification = await verificationCodeResponse.Content.ReadAsStringAsync();

            var verificationResponse = await _http.GetAsync("/api/account/verify/" + verification.Substring(1, verification.Length - 2));
            verificationResponse.EnsureSuccessStatusCode();

            var loginafterVerificationResponse = await _http.PostAsync("/api/auth/login", login.GetStringContent());
            Assert.Equal(200, (int)loginafterVerificationResponse.StatusCode);
        }

        [Fact]
        public async Task PostVerification_WhenInvalid_ExpectNotFound()
        {
            var registration = new Registration
            {
                Username = "verifycd",
                EmailAddress = "code@verify.com",
                Password = "no more bad melons for this guy"
            };

            var registrationResponse = await _http.PostAsync("/api/account/register", registration.GetStringContent());
            registrationResponse.EnsureSuccessStatusCode();

            var verifyResposne = await _http.GetAsync("/api/account/verify/" + Guid.NewGuid());
            Assert.Equal(404, (int)verifyResposne.StatusCode);
        }

        [Fact]
        public async Task PostRegistration_WhenVerifiedWithoutPassword_ExpectLoginNotFound()
        {
            var registration = new Registration
            {
                Username = "ihavenopass",
                EmailAddress = "nopassword@verify.com"
            };

            var registrationResponse = await _http.PostAsync("/api/account/register", registration.GetStringContent());
            registrationResponse.EnsureSuccessStatusCode();

            var verificationCodeResponse = await _http.GetAsync("/api/testdata/verification-codes/" + registration.Username);
            string verification = await verificationCodeResponse.Content.ReadAsStringAsync();

            var verificationResponse = await _http.GetAsync("/api/account/verify/" + verification.Substring(1, verification.Length - 2));
            verificationResponse.EnsureSuccessStatusCode();

            var login = new Login { Username = registration.Username, Password = "" };
            var loginResponse = await _http.PostAsync("/api/auth/login", login.GetStringContent());
            Assert.Equal(404, (int)loginResponse.StatusCode);
        }

        [Fact]
        public async Task PostRegistration_WhenVerifiedWithoutPassword_ExpectEmailLoginSuccess()
        {
            var registration = new Registration
            {
                Username = "ihavenopass2",
                EmailAddress = "nopassword2@verify.com"
            };

            var registrationResponse = await _http.PostAsync("/api/account/register", registration.GetStringContent());
            registrationResponse.EnsureSuccessStatusCode();

            var verificationCodeResponse = await _http.GetAsync("/api/testdata/verification-codes/" + registration.Username);
            string verification = await verificationCodeResponse.Content.ReadAsStringAsync();

            var verificationResponse = await _http.GetAsync("/api/account/verify/" + verification.Substring(1, verification.Length - 2));
            verificationResponse.EnsureSuccessStatusCode();

            var login = new Login { Username = registration.Username, LoginMethod = LoginMethod.EMAIL };
            Login(login);

            var newVerificationCodeResponse = await _http.GetAsync("/api/testdata/verification-codes/" + registration.Username);
            string newVerification = await newVerificationCodeResponse.Content.ReadAsStringAsync();
            newVerification = newVerification.Substring(1, newVerification.Length - 2);

            var codeLoginResponse = await _http.GetAsync("/api/auth/code/" + newVerification);
            Assert.Equal(200, (int)codeLoginResponse.StatusCode);
        }
    }
}