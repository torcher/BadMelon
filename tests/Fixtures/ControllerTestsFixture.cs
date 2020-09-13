using BadMelon.Data;
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

            _http.DeleteAsync("api/database").Wait();
            Login();
        }

        protected void Login()
        {
            var response = AsyncHelper.RunSync(() => _http.PostAsync("api/auth/login", dataSamples.Users.FirstOrDefault().Item2.GetStringContent()));

            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (Exception x)
            {
                throw x;
            }
            Console.WriteLine(response.StatusCode.ToString());
        }

        protected void Logout()
        {
            var response = AsyncHelper.RunSync(() => _http.PostAsync("api/auth/logout", new StringContent("")));
            if (!response.IsSuccessStatusCode && response.StatusCode != System.Net.HttpStatusCode.Unauthorized)
                throw new Exception("Should not be status code " + response.StatusCode.ToString());
        }
    }
}