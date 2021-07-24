using BadMelon.Data;
using BadMelon.Data.DTOs;
using BadMelon.Data.Extensions;
using BadMelon.Tests.Helpers;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Linq;
using System.Net.Http;

namespace BadMelon.Tests.Fixtures
{
    public class ControllerTestsFixture
    {
        protected readonly HttpClient _http;
        protected DataSamples dataSamples;
        protected readonly TestServer testServer;

        public ControllerTestsFixture()
        {
            testServer = TestServerFactory.GetTestServer();
            using var testClient = testServer.CreateClient();
            _http = new HttpClient(new TestHttpClientHandler(testServer.CreateHandler()));
            _http.BaseAddress = testClient.BaseAddress;
            dataSamples = new DataSamples();

            AsyncHelper.RunSync(() => _http.DeleteAsync("api/database"));
            Login();
        }

        protected void Login()
        {
            Login(dataSamples.Users.FirstOrDefault().Item2);
        }

        protected void Login(Login login)
        {
            var response = AsyncHelper.RunSync(() => _http.PostAsync("api/auth/login", login.GetStringContent()));

            try
            {
                response.EnsureSuccessStatusCode();
                var jwt = response.GetObject<Jwt>().Result;
                _http.SetBearerToken(jwt?.token ?? "");
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void Logout()
        {
            _http.SetBearerToken("");
        }
    }
}