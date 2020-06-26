using BadMelon.Data;
using BadMelon.Tests.Helpers;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace BadMelon.Tests.Fixtures
{
    public class ControllerTestsFixture
    {
        protected readonly HttpClient _http;
        protected DataSamples dataSamples;
        protected readonly TestServer testServer;

        public ControllerTestsFixture()
        {
            testServer = TestServerFactory.TestServer;
            using var testClient = testServer.CreateClient();
            _http = new HttpClient(new TestHttpClientHandler(testServer.CreateHandler()));
            _http.BaseAddress = testClient.BaseAddress;
            dataSamples = new DataSamples();

            _http.DeleteAsync("api/database").Wait();
            Login().Wait();
        }

        protected async Task Login()
        {
            var response = await _http.PostAsync("api/auth/login", StringContentGenerator.GetJSON(dataSamples.RootUserLogin));
            Console.WriteLine(response.StatusCode.ToString());
        }

        protected async Task Logout()
        {
            await _http.PostAsync("api/auth/logout", new StringContent(""));
        }
    }
}