using BadMelon.Data;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System.Net.Http;

namespace BadMelon.Tests.API.Fixtures
{
    public class ControllerTestsFixture
    {
        protected readonly HttpClient _http;
        protected DataSamples dataSamples;
        protected readonly TestServer testServer;

        public ControllerTestsFixture()
        {
            testServer = new TestServer(WebHost.CreateDefaultBuilder().UseStartup<TestStartup>().UseEnvironment("Testing"));
            _http = testServer.CreateClient();

            dataSamples = new DataSamples();

            _http.GetAsync("api/database/seed").Wait();
        }
    }
}