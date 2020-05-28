using BadMelon.Data;
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
            testServer = TestServerFactory.TestServer;
            _http = testServer.CreateClient();

            dataSamples = new DataSamples();

            _http.DeleteAsync("api/database").Wait();
        }
    }
}